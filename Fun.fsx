#light

#r @"../packages/FSharp.Data.2.4.6/lib/net45/Fsharp.Data.dll"

// Possible compilation error with FSharp.Plotly -> Hard reference as fix. Seems fixed for now.
//#r @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\Microsoft\Microsoft.NET.Build.Extensions\net461\lib\netstandard.dll"

#load "Types.fs"
open MyTypes.Types

open FSharp.Data
open FSharp.Plotly

// ##################### FOR PERSEUS #####################

//// Parses csv (Perseus standard output)
//let perseusMatrixParser (input:seq<string>) =     
//    let en = input.GetEnumerator()     
//    let rec loop (en:System.Collections.Generic.IEnumerator<string>) msc acc =
//        if en.MoveNext() then
//           match en.Current with
//           | (h:string) when h.StartsWith "#" -> let nh = (h.Remove(0,1)).Split ('\t')   
//                                                 loop en (nh::msc) acc  

//           | (h:string) -> let nh = h.Split ('\t') |> Array.map (fun x -> float x) //x.AsFloat(System.Globalization.CultureInfo.InvariantCulture) )
//                           loop en msc (nh::acc)
//        else (msc |> List.rev),(acc |> List.rev)
//    loop en [] []


//// Check Ploidy specify deregulation, 1N/2N vs 3N/4N
//let pOnepTwoRawFilter (threshold:string) (values:seq<ProcessedProteinData>) =
//    values |> Seq.filter ( fun x -> let diffofdiff = abs (x.diffP3toP4 - x.diffP1toP2)
//                                    if (float threshold) <= diffofdiff then true else false) 

//// Check "odd ploidy effect" negative side
//let lookForNegOddPloidy (values:seq<ProcessedProteinData>) =
//    values |> Seq.filter ( fun x -> if x.diffP1toP2 <= 0. then false
//                                       elif x.diffP2toP3 >= 0. then false
//                                         else x.diffP3toP4 >= 0. )

//// Check "odd ploidy effect" positive side
//let lookForPosOddPloidy (values:seq<ProcessedProteinData>) =
//    values |> Seq.filter ( fun x -> if x.diffP1toP2 >= 0. then false
//                                       elif x.diffP2toP3 <= 0. then false
//                                         else x.diffP3toP4 <= 0. )       
      
//######################################################################

/// Call CsvTypeProvider for tab seperated values, parse Dataset
let myParsedValues = CsvFile.Load(".\public\DataSets\Combined_Proteome_RNA.txt", "\t")

// Choose parsed raw proteome values
let myValues = 
    [ for row in myParsedValues.Rows do
         yield (float (row.GetColumn "1N_Protein")), (float (row.GetColumn "2N_Protein")), (float (row.GetColumn "3N_Protein")), (float (row.GetColumn "4N_Protein")),(row.GetColumn "Gene names"), (int (row.GetColumn "id"))] |> Seq.ofList

// Calculate differences between ploidies
let processProteinData (proteinData: seq<float*float*float*float*string*int>) =
    
    proteinData |> Seq.map (fun (f,s,t,fo,n,i) -> let diff1 = (s - f)
                                                  let diff2 = (t - s)
                                                  let diff3 = (fo - t)
                                                  let tend = diff1+diff2+diff3
                                                  createProcessedProteinData f s t fo diff1 diff2 diff3 tend n i)
// Wrap to ProcessedProteinData type
let myProcessedProteinData = processProteinData myValues 
                             
// Exclude all values with tendencies below a set threshold
let biggerThanThreshold (threshold:float) (values:seq<ProcessedProteinData>) =
    values |> Seq.filter ( fun (x) -> x.tendency >= threshold) 

// Exclude all values with tendencies above a set threshold
let smallerThanThreshold (threshold:float) (values:seq<ProcessedProteinData>) =
    values |> Seq.filter ( fun (x) -> x.tendency <= threshold) 

// Exclude all values with tendencies above a set threshold
let biggerThanAbsThreshold (threshold:float) (values:seq<ProcessedProteinData>) =
    values |> Seq.filter ( fun (x) -> (abs x.tendency) >= threshold) 

// Combined filter, bigger than positive threshold, smaller than negative one
let combinedThresholdFilter (threshold) (values:seq<ProcessedProteinData>) =
    if (threshold |> System.String.IsNullOrWhiteSpace) then values 
    else
        if (float threshold) >= 0. then values |> Seq.filter ( fun (x) -> x.tendency >= (float threshold)) 
        else values |> Seq.filter ( fun (x) -> x.tendency <= (float threshold) ) 

// Exclude all values with single absolute differences above a set threshold (spikes) 
let smoothFilterWithThreshold (threshold) (values:seq<ProcessedProteinData>) =
    if (threshold |> System.String.IsNullOrWhiteSpace) then values |> Seq.sortByDescending (fun x -> x.tendency)
    else
        values |> Seq.filter ( fun (x) -> if (abs x.diffP1toP2) >= (float threshold) then false
                                          elif (abs x.diffP2toP3) >= (float threshold) then false
                                            else (abs x.diffP3toP4) <= (float threshold) )
               |> Seq.sortByDescending (fun x -> x.tendency)

// Tuple two filtered sets
let filterBothDataSets threshold (set1:seq<ProcessedProteinData>) (set2:seq<ProcessedProteinData>) =
    
    let fset1 = set1
                |> combinedThresholdFilter threshold

    let fset2 = set2
                |> combinedThresholdFilter threshold

    fset1,fset2

// TryFind overlap between two sets
let findOverlap (fData:seq<ProcessedProteinData>) (sfData:seq<ProcessedProteinData>) =
    sfData |> Seq.map (fun x -> fData |> Seq.tryFind ( fun y -> y.id = x.id)) |> Seq.filter ( fun x -> x <> None)
    
// Choose annotated proteome values
let myAnnotatedBPCC = 
    [ for row in myParsedValues.Rows do
         yield (float (row.GetColumn "1N_Protein")), (float (row.GetColumn "2N_Protein")), (float (row.GetColumn "3N_Protein")), (float (row.GetColumn "4N_Protein")),(row.GetColumn "Gene names"(* + " - " + (row.``Protein names``.Split [|';'|] |> Array.head)*)),(int (row.GetColumn "id")),row.GetColumn "GOBP",row.GetColumn "GOCC"] |> Seq.ofList

// Search GOCC annotation column, warp to ProcessedProteinData type
let searchGOCCTerms (term:string) (annotatedSet:seq<float*float*float*float*string*int*string*string>) = 
    if (term |> System.String.IsNullOrWhiteSpace) then Seq.empty |> processProteinData
    else
    annotatedSet |> Seq.filter (fun (a,b,c,d,e,f,g,h) -> h.Contains term) |> Seq.map (fun (a,b,c,d,e,f,g,h) -> (a,b,c,d,e,f)) |> processProteinData

// Search GOBP annotation column, wrap to ProcessedProteinData type [ToDo: Include toggle]
let searchGOBPTerms (term:string) (annotatedSet:seq<float*float*float*float*string*int*string*string>) = 
    if (term |> System.String.IsNullOrWhiteSpace) then Seq.empty |> processProteinData
    else
    annotatedSet |> Seq.filter (fun (a,b,c,d,e,f,g,h) -> g.Contains term) |> Seq.map (fun (a,b,c,d,e,f,g,h) -> (a,b,c,d,e,f)) |> processProteinData

//let neighborhoodFilter (threshold) (values:seq<ProcessedProteinData>) =
//    if (threshold |> System.String.IsNullOrWhiteSpace) then values
//    else
//        values |> Seq.filter ( fun (x) -> if (abs x.diffP1toP2) >= (float threshold) then false
//                                          elif (abs x.diffP2toP3) >= (float threshold) then false
//                                            else (abs x.diffP3toP4) <= (float threshold) )

// Filter set for gene names
let filterbyGeneName (term:string) (values:seq<ProcessedProteinData>) = 
    let searchValues = (term.Split [|',';';'|]) |> Array.filter (fun x -> (x|> System.String.IsNullOrWhiteSpace) |> not) |> Array.map ( fun x -> x.Trim()) |> List.ofArray
    if searchValues.IsEmpty then Seq.empty
    else
    seq [for value in searchValues do 
            yield values |> Seq.filter (fun x -> x.geneName.Contains (value.ToUpper() ))] |> Seq.concat |> Seq.sortByDescending (fun x -> x.tendency)


//######################################################################


// Choose parsed raw mRNA values
let myRNAValues = 
    [ for row in myParsedValues.Rows do
         yield (float (row.GetColumn "1N_RNA")), (float (row.GetColumn "2N_RNA")), (float (row.GetColumn "3N_RNA")), (float (row.GetColumn "4N_RNA")),row.GetColumn "Gene names", row.GetColumn "Chromosome/scaffold name"] |> Seq.ofList

// Calculate differences between ploidies
let processRNAData (proteinData: seq<float*float*float*float*string*string>) =
    
    proteinData |> Seq.map (fun (f,s,t,fo,n,c) -> let diff1 = (s - f)
                                                  let diff2 = (t - s)
                                                  let diff3 = (fo - t)
                                                  let tend = diff1+diff2+diff3
                                                  createProcessedRNAData f s t fo diff1 diff2 diff3 tend n c)
// Wrap to ProcessedRNAData type
let myProcessedRNAData = (processRNAData myRNAValues)  

// Filter set for gene names  
let filterbyGeneNameRNA (term:string) (values:seq<ProcessedRNAData>) = 
    let searchValues = (term.Split [|',';';'|]) |> Array.filter (fun x -> (x|> System.String.IsNullOrWhiteSpace) |> not) |> Array.map ( fun x -> x.Trim()) |> List.ofArray
    if searchValues.IsEmpty then Seq.empty
    else
    seq [for value in searchValues do 
            yield values |> Seq.filter (fun x -> x.geneName.Contains (value.ToUpper() ))] |> Seq.concat |> Seq.sortByDescending (fun x -> x.tendency)
    
//let neighborhoodFilterRNA (threshold) (values:seq<ProcessedRNAData>) =
//    if (threshold |> System.String.IsNullOrWhiteSpace) then values
//    else
//        values |> Seq.filter ( fun (x) -> if (abs x.diffP1toP2) >= (float threshold) then false
//                                            elif (abs x.diffP2toP3) >= (float threshold) then false
//                                            else (abs x.diffP3toP4) <= (float threshold) )

// Search GOCC annotation column, warp to ProcessedProteinData type    
let searchGOCCTermsRNA (term:string) (annotatedSet:seq<float*float*float*float*string*string*string*string>) = 
    if (term |> System.String.IsNullOrWhiteSpace) then Seq.empty |> processRNAData
    else    
    annotatedSet |> Seq.filter (fun (a,b,c,d,e,g,h,i) -> h.Contains term) |> Seq.map (fun (a,b,c,d,e,g,h,i) -> (a,b,c,d,e,i)) |> processRNAData

// Search GOBP annotation column, wrap to ProcessedProteinData type [ToDo: Include toggle]    
let searchGOBPTermsRNA (term:string) (annotatedSet:seq<float*float*float*float*string*string*string*string>) = 
    if (term |> System.String.IsNullOrWhiteSpace) then Seq.empty |> processRNAData
    else
    annotatedSet |> Seq.filter (fun (a,b,c,d,e,g,h,i) -> g.Contains term) |> Seq.map (fun (a,b,c,d,e,g,h,i) -> (a,b,c,d,e,i)) |> processRNAData

//Choose parsed annotated RNA values
let myAnnotatedRNAValues = 
    [ for row in myParsedValues.Rows do
         yield (float (row.GetColumn "1N_RNA")), (float (row.GetColumn "2N_RNA")), (float (row.GetColumn "3N_RNA")), (float (row.GetColumn "4N_RNA")),row.GetColumn "Gene names",row.GetColumn "GOBP", row.GetColumn "GOCC",row.GetColumn "Chromosome/scaffold name"] |> Seq.ofList

// Adaptive filter, bigger than positive threshold, smaller than negative one
let combinedThresholdFilterRNA (threshold) (values:seq<ProcessedRNAData>) =
    if (threshold |> System.String.IsNullOrWhiteSpace) then values
    else
        if (float threshold) >= 0. then values |> Seq.filter ( fun (x) -> x.tendency >= (float threshold)) 
        else values |> Seq.filter ( fun (x) -> x.tendency <= (float threshold) ) 
        
// Exclude all values with single absolute differences above a set threshold (spikes) 
let smoothFilterWithThresholdRNA (threshold) (values:seq<ProcessedRNAData>) =
    if (threshold |> System.String.IsNullOrWhiteSpace) then values |> Seq.sortByDescending (fun x -> x.tendency)
    else
        values |> Seq.filter ( fun (x) -> if (abs x.diffP1toP2) >= (float threshold) then false
                                          elif (abs x.diffP2toP3) >= (float threshold) then false
                                            else (abs x.diffP3toP4) <= (float threshold) )
               |> Seq.sortByDescending (fun x -> x.tendency)
// Merges any input, adds average line, reduces opacity of traces relative to distance to average
let mergeMyPlot (aList:linePlotData list) =
    let myY = aList |> List.map (fun x -> x.y )
    
    let mergeY (yList: float list list) =
        let rec loop (myYList:float list list) y1 y2 y3 y4 =
            match myYList with
            | head::tail -> loop tail (head.[0]::y1) (head.[1]::y2) (head.[2]::y3) (head.[3]::y4)
            | _ -> [(y1|> List.filter (System.Double.IsNaN >> not)|> List.average);(y2|> List.filter (System.Double.IsNaN >> not)|> List.average);(y3|> List.filter (System.Double.IsNaN >> not)|> List.average);(y4|> List.filter (System.Double.IsNaN >> not)|> List.average)]
        loop myY [] [] [] []

    let newY = (mergeY myY)

    let updatedOpaDat = let p1,p2,p3,p4 = newY.[0],newY.[1],newY.[2],newY.[3]
                        let diffForOp = aList |> List.map ( fun x -> createLinePlotData x.x x.y x.name x.line x.mode ((x.opacity - ([abs ((p1-x.y.[0]));(abs (p2-x.y.[1]));(abs (p3-x.y.[2]));(abs (p4-x.y.[3]))] |> List.average)) |> (fun x -> if x <= 0. then 0.1 else x)) ) 
                        diffForOp

    let myMergedTrace = createLinePlotData ["1N";"2N";"3N";"4N"] newY "Average" (createmyLine "blue" 4 "spline") "scatter" 1.

    (myMergedTrace::updatedOpaDat)    
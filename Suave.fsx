#r @"..\packages\Suave.2.4.3\lib\net461\Suave.dll"
#r @"..\packages\Newtonsoft.Json.11.0.2\lib\net45\Newtonsoft.Json.dll"

#load "Types.fs"

// FSharp Systems
open System.Net
open System.IO

// Suave
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Files

// JSON Serializer
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

// Functions and Types
open Fun
open MyTypes.Types


// -----------------------------------------------------------------------------------------------------------------------------------------------------


// JSON serializer to wrap request
let JSON v =
    let jsonSerializerSettings = new JsonSerializerSettings()
    jsonSerializerSettings.ContractResolver <- new CamelCasePropertyNamesContractResolver()

    JsonConvert.SerializeObject(v, jsonSerializerSettings)
    |> OK
    >=> Writers.setMimeType "application/json; charset=utf-8"

// Handle proteome/mRNA combined filtering, style profile plot, serialze to JSON
let toPEData mys =
    let myFirst,mySecond,myThird = mys
    match myThird with

    | "Experiment" -> "" |> JSON
    | "Proteome" -> let lineData = myProcessedProteinData |> combinedThresholdFilter myFirst |> smoothFilterWithThreshold mySecond
                    let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 1.) |> Seq.toList
                    (linePlot |> createLinePlot) |> JSON 

    | "Transcriptome" -> let lineData = myProcessedRNAData |> combinedThresholdFilterRNA myFirst |> smoothFilterWithThresholdRNA mySecond
                         let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 1.) |> Seq.toList
                         (linePlot |> createLinePlot) |> JSON 

    // Empty string to handle random button presses
    | _ -> "" |> JSON

// Handle proteome/mRNA combined filtering, style profile plot, serialze to JSON
let toPEDataHeat mys =
    let myFirst,mySecond,myThird = mys
    match myThird with

    | "Experiment" -> "" |> JSON
    | "Proteome" -> let heatData = myProcessedProteinData |> combinedThresholdFilter myFirst |> smoothFilterWithThreshold mySecond
                    let heatPlotY,heatPlotZ = heatData |> Seq.map (fun x -> x.geneName,[x.p1;x.p2;x.p3;x.p4]) |> Seq.toList |> List.unzip
                    let heatPlot = createHeatMap ["1N";"2N";"3N";"4N"] heatPlotY  heatPlotZ
                    heatPlot |> JSON

    | "Transcriptome" -> let heatData = myProcessedRNAData |> combinedThresholdFilterRNA myFirst |> smoothFilterWithThresholdRNA mySecond
                         let heatPlotY,heatPlotZ = heatData |> Seq.map (fun x -> x.geneName,[x.p1;x.p2;x.p3;x.p4]) |> Seq.toList |> List.unzip
                         let heatPlot = createHeatMap ["1N";"2N";"3N";"4N"] heatPlotY  heatPlotZ
                         heatPlot |> JSON

    // Empty string to handle random button presses
    | _ -> "" |> JSON

// Handle proteome/mRNA GOBP search, style profile plot, serialze to JSON
let togoBP mys =
    let myFirst,mySecond,myThird,myFourth = mys
    match myFourth with

    | "Experiment" -> "" |> JSON

    | "Proteome" -> let lineData = searchGOBPTerms myThird myAnnotatedBPCC |> combinedThresholdFilter myFirst |> smoothFilterWithThreshold mySecond
                    let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 1.) |> Seq.toList
                    (linePlot |> createLinePlot) |> JSON

    | "Transcriptome" -> let lineData = searchGOBPTermsRNA myThird myAnnotatedRNAValues |> combinedThresholdFilterRNA myFirst |> smoothFilterWithThresholdRNA mySecond
                         let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 1.) |> Seq.toList
                         (linePlot |> createLinePlot) |> JSON

    // Empty string to handle random button presses.
    | _ -> "" |> JSON

// Handle proteome/mRNA GOCC search, style profile plot, serialze to JSON
let togoCC mys =
    let myFirst,mySecond,myThird,myFourth = mys
    match myFourth with

    | "Experiment" -> "" |> JSON

    | "Proteome" -> let lineData = searchGOCCTerms myThird myAnnotatedBPCC |> combinedThresholdFilter myFirst |> smoothFilterWithThreshold mySecond
                    let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 1.) |> Seq.toList
                    (linePlot |> createLinePlot) |> JSON

    | "Transcriptome" ->  let lineData = searchGOCCTermsRNA myThird myAnnotatedRNAValues |> combinedThresholdFilterRNA myFirst |> smoothFilterWithThresholdRNA mySecond
                          let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 1.) |> Seq.toList
                          (linePlot |> createLinePlot) |> JSON

    // Empty string to handle random button presses.
    | _ -> "" |> JSON

// Handle proteome/mRNA GOBP search, style heatmap, serialze to JSON
let toGoBPHEAT mys =
    let myFirst,mySecond,myThird,myFourth = mys
    match myFourth with

    | "Experiment" -> "" |> JSON

    | "Proteome" -> let heatData = searchGOBPTerms myThird myAnnotatedBPCC |> combinedThresholdFilter myFirst |> smoothFilterWithThreshold mySecond
                    let heatPlotY,heatPlotZ = heatData |> Seq.map (fun x -> x.geneName,[x.p1;x.p2;x.p3;x.p4]) |> Seq.toList |> List.unzip
                    let heatPlot = createHeatMap ["1N";"2N";"3N";"4N"] heatPlotY heatPlotZ
                    heatPlot |> JSON

    | "Transcriptome" -> let heatData = searchGOBPTermsRNA myThird myAnnotatedRNAValues |> combinedThresholdFilterRNA myFirst |> smoothFilterWithThresholdRNA mySecond
                         let heatPlotY,heatPlotZ = heatData |> Seq.map (fun x -> x.geneName,[x.p1;x.p2;x.p3;x.p4]) |> Seq.toList |> List.unzip
                         let heatPlot = createHeatMap ["1N";"2N";"3N";"4N"] heatPlotY  heatPlotZ
                         heatPlot |> JSON

    // Empty string to handle random button presses.
    | _ -> "" |> JSON

// Handle proteome/mRNA GOCC search, style heatmap, serialze to JSON   
let toGoCCHEAT mys =
    let myFirst,mySecond,myThird,myFourth = mys
    match myFourth with

    | "Experiment" -> "" |> JSON

    | "Proteome" -> let heatData = searchGOCCTerms myThird myAnnotatedBPCC |> combinedThresholdFilter myFirst |> smoothFilterWithThreshold mySecond
                    let heatPlotY,heatPlotZ = heatData |> Seq.map (fun x -> x.geneName,[x.p1;x.p2;x.p3;x.p4]) |> Seq.toList |> List.unzip
                    let heatPlot = createHeatMap ["1N";"2N";"3N";"4N"] heatPlotY heatPlotZ
                    heatPlot |> JSON

    | "Transcriptome" -> let heatData = searchGOCCTermsRNA myThird myAnnotatedRNAValues |> combinedThresholdFilterRNA myFirst |> smoothFilterWithThresholdRNA mySecond
                         let heatPlotY,heatPlotZ = heatData |> Seq.map (fun x -> x.geneName,[x.p1;x.p2;x.p3;x.p4]) |> Seq.toList |> List.unzip
                         let heatPlot = createHeatMap ["1N";"2N";"3N";"4N"] heatPlotY  heatPlotZ
                         heatPlot |> JSON

    // Empty string to handle random button presses.
    | _ -> "" |> JSON

// Handle specific searches with or without filtering and/or GOBP/GOCC background, style profile plot, serialize to JSON
let searchForSpecific mys =
    let myFirst,mySecond,myThird,myFourth,myFif,mySix = mys
    if (myThird |> System.String.IsNullOrWhiteSpace) then

        match mySecond with

        |"Experiment" -> "" |> JSON

        |"Proteome" -> let lineData = filterbyGeneName myFirst myProcessedProteinData 
                       let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "black" 2 "spline") "scatter" 1.) |> Seq.toList 
                       let lineData2 = searchGOCCTerms myFourth myAnnotatedBPCC |> combinedThresholdFilter myFif |> smoothFilterWithThreshold mySix
                       let linePlot2 = lineData2 |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 0.3) |> Seq.toList 
                       let lineComplete = (List.append linePlot linePlot2) |> List.distinctBy (fun x -> x.name)
                       (lineComplete |> createLinePlot) |> JSON
        
        //|"Placeholder" -> let lineData = CompletePeon.filterbyGeneName myFirst CompletePeon.myProcessedProteinData 
        //                  let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1.";"2.";"3.";"4."] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "red" 2 "spline") "scatter" 1.) |> Seq.toList 
        //                  let lineData2 = CompletePeon.searchGOCCTerms myFourth CompletePeon.myAnnotatedBPCC |> CompletePeon.combinedThresholdFilter myFif |> CompletePeon.smoothFilterWithThreshold mySix
        //                  let linePlot2 = lineData2 |> Seq.map (fun x -> createLinePlotData ["1.";"2.";"3.";"4."] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 0.2) |> Seq.toList 
        //                  let lineComplete = (List.append linePlot linePlot2) |> List.distinctBy (fun x -> x.name)
        //                  (lineComplete |> createLinePlot) |> JSON
        
        // Matches to mRNA.
        | _ -> let lineData = filterbyGeneNameRNA myFirst myProcessedRNAData
               let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "black" 2 "spline") "scatter" 1.) |> Seq.toList 
               let lineData2 = searchGOCCTermsRNA myFourth myAnnotatedRNAValues |> combinedThresholdFilterRNA myFif |> smoothFilterWithThresholdRNA mySix
               let linePlot2 = lineData2 |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 0.3) |> Seq.toList
               let lineComplete = (List.append linePlot linePlot2) |> List.distinctBy (fun x -> x.name)
               (lineComplete |> createLinePlot) |> JSON
    else
        match mySecond with

        |"Experiment" -> "" |> JSON

        |"Proteome" -> let lineData = filterbyGeneName myFirst myProcessedProteinData 
                       let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "black" 2 "spline") "scatter" 1.) |> Seq.toList 
                       let lineData2 = searchGOBPTerms myThird myAnnotatedBPCC |> combinedThresholdFilter myFif |> smoothFilterWithThreshold mySix
                       let linePlot2 = lineData2 |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 0.3) |> Seq.toList 
                       let lineComplete = (List.append linePlot linePlot2) |> List.distinctBy (fun x -> x.name)
                       (lineComplete |> createLinePlot) |> JSON

        //|"Placeholder" -> let lineData = CompletePeon.filterbyGeneName myFirst CompletePeon.myProcessedProteinData 
        //                  let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1.";"2.";"3.";"4."] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "red" 2 "spline") "scatter" 1.) |> Seq.toList 
        //                  let lineData2 = CompletePeon.searchGOBPTerms myThird CompletePeon.myAnnotatedBPCC |> CompletePeon.combinedThresholdFilter myFif |> CompletePeon.smoothFilterWithThreshold mySix
        //                  let linePlot2 = lineData2 |> Seq.map (fun x -> createLinePlotData ["1.";"2.";"3.";"4."] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 0.2) |> Seq.toList 
        //                  let lineComplete = (List.append linePlot linePlot2) |> List.distinctBy (fun x -> x.name)
        //                  (lineComplete |> createLinePlot) |> JSON

        // Matches to mRNA.
        | _ ->  let lineData = filterbyGeneNameRNA myFirst myProcessedRNAData
                let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "black" 2 "spline") "scatter" 1.) |> Seq.toList 
                let lineData2 = searchGOBPTermsRNA myThird myAnnotatedRNAValues |> combinedThresholdFilterRNA myFif |> smoothFilterWithThresholdRNA mySix
                let linePlot2 = lineData2 |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 0.3) |> Seq.toList
                let lineComplete = (List.append linePlot linePlot2) |> List.distinctBy (fun x -> x.name)
                (lineComplete |> createLinePlot) |> JSON

// Handle specific searches with or without filtering and/or GOBP/GOCC background, style heatmap, serialize to JSON
let searchForSpecificHeat mys =
    let myFirst,mySecond = mys
    match mySecond with

    |"Experiment" -> "" |> JSON

    |"Proteome" -> let heatData = filterbyGeneName myFirst myProcessedProteinData 
                   let heatPlotY,heatPlotZ = heatData |> Seq.map (fun x -> x.geneName,[x.p1;x.p2;x.p3;x.p4]) |> Seq.toList |> List.unzip
                   let heatPlot = createHeatMap ["1N";"2N";"3N";"4N"] heatPlotY heatPlotZ
                   heatPlot |> JSON

    //|"PlaceHolder" -> let heatData = CompletePeon.filterbyGeneName myFirst CompletePeon.myProcessedSigProteinData 
    //                  let heatPlotY,heatPlotZ = heatData |> Seq.map (fun x -> x.geneName,[x.p1;x.p2;x.p3;x.p4]) |> Seq.toList |> List.unzip
    //                  let heatPlot = createHeatMap ["1.";"2.";"3.";"4."] heatPlotY heatPlotZ
    //                  heatPlot |> JSON

    // Matches to mRNA.
    | _ ->  let heatData = filterbyGeneNameRNA myFirst myProcessedRNAData
            let heatPlotY,heatPlotZ = heatData |> Seq.map (fun x -> x.geneName,[x.p1;x.p2;x.p3;x.p4]) |> Seq.toList |> List.unzip
            let heatPlot = createHeatMap ["1N";"2N";"3N";"4N"] heatPlotY heatPlotZ
            heatPlot |> JSON

// Merges any input, adds average line, reduces opacity of traces relative to distance to average, serializes to JSON
let mergeMyGO mys =
    let myFirst,mySecond,myThird,myFourth,myFif,mySix = mys
    if (myThird |> System.String.IsNullOrWhiteSpace) then
        match mySecond with

        |"Experiment" -> "" |> JSON

        |"Proteome" -> let lineData = filterbyGeneName myFirst myProcessedProteinData 
                       let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "red" 2 "spline") "scatter" 0.3) |> Seq.toList 
                       let lineData2 = searchGOCCTerms myFourth myAnnotatedBPCC |> combinedThresholdFilter myFif |> smoothFilterWithThreshold mySix
                       let linePlot2 = lineData2 |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 0.6) |> Seq.toList 
                       let lineComplete = (List.append linePlot linePlot2) |> List.distinctBy (fun x -> x.name)
                       (lineComplete |> mergeMyPlot |> createLinePlot ) |> JSON
         // Matches to mRNA.
        | _ -> let lineData = filterbyGeneNameRNA myFirst myProcessedRNAData
               let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "red" 2 "spline") "scatter" 0.3) |> Seq.toList 
               let lineData2 = searchGOCCTermsRNA myFourth myAnnotatedRNAValues |> combinedThresholdFilterRNA myFif |> smoothFilterWithThresholdRNA mySix
               let linePlot2 = lineData2 |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 0.6) |> Seq.toList
               let lineComplete = (List.append linePlot linePlot2) |> List.distinctBy (fun x -> x.name)
               (lineComplete |> mergeMyPlot  |> createLinePlot) |> JSON

    else
        match mySecond with

        |"Experiment" -> "" |> JSON

        |"Proteome" -> let lineData = filterbyGeneName myFirst myProcessedProteinData 
                       let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "red" 2 "spline") "scatter" 0.3) |> Seq.toList 
                       let lineData2 = searchGOBPTerms myThird myAnnotatedBPCC |> combinedThresholdFilter myFif |> smoothFilterWithThreshold mySix
                       let linePlot2 = lineData2 |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 0.6) |> Seq.toList 
                       let lineComplete = (List.append linePlot linePlot2) |> List.distinctBy (fun x -> x.name)
                       (lineComplete |> mergeMyPlot  |> createLinePlot) |> JSON

        //| "Placeholder" ->  let lineData = CompletePeon.filterbyGeneNameDeg myFirst CompletePeon.myProcessedDegData
        //                    let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1.";"2.";"3.";"4."] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "red" 2 "spline") "scatter" 0.3) |> Seq.toList 
        //                    let lineData2 = CompletePeon.searchGOBPTermsDeg myThird CompletePeon.myAnnotatedDegValues |> CompletePeon.combinedThresholdFilterDeg myFif |> CompletePeon.smoothFilterWithThresholdDeg mySix
        //                    let linePlot2 = lineData2 |> Seq.map (fun x -> createLinePlotData ["1.";"2.";"3.";"4."] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "black" 1 "spline") "scatter" 0.6) |> Seq.toList
        //                    let lineComplete = (List.append linePlot linePlot2) |> List.distinctBy (fun x -> x.name)
        //                    (lineComplete |> mergeMyPlot  |> createLinePlot) |> JSON

         // Matches to mRNA.
        | _ ->  let lineData = filterbyGeneNameRNA myFirst myProcessedRNAData
                let linePlot = lineData |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "red" 2 "spline") "scatter" 0.3) |> Seq.toList 
                let lineData2 = searchGOBPTermsRNA myThird myAnnotatedRNAValues |> combinedThresholdFilterRNA myFif |> smoothFilterWithThresholdRNA mySix
                let linePlot2 = lineData2 |> Seq.map (fun x -> createLinePlotData ["1N";"2N";"3N";"4N"] [x.p1;x.p2;x.p3;x.p4] x.geneName (createmyLine "grey" 1 "spline") "scatter" 0.6) |> Seq.toList
                let lineComplete = (List.append linePlot linePlot2) |> List.distinctBy (fun x -> x.name)
                (lineComplete |> mergeMyPlot  |> createLinePlot) |> JSON


// -----------------------------------------------------------------------------------------------------------------------------------------------------


// Provide HTML   
let plot : WebPart =
    choose [
        GET >=> file (".\public\Plot\Plot.html")
        ]  


// App pathing structure, suave.io is sweet!
let app : WebPart =
  choose [

    // Request Handler
    path "/YeastPloidy/" >=> plot
    
    pathScan "/YeastPloidy/PEData/%s/%s/%s" toPEData
    pathScan "/YeastPloidy/PEDataHeat/%s/%s/%s" toPEDataHeat
    pathScan "/YeastPloidy/goBP/%s/%s/%s/%s" togoBP
    pathScan "/YeastPloidy/goBPHeat/%s/%s/%s/%s" toGoBPHEAT
    pathScan "/YeastPloidy/goCCHeat/%s/%s/%s/%s" toGoCCHEAT
    pathScan "/YeastPloidy/goCC/%s/%s/%s/%s" togoCC
    pathScan "/YeastPloidy/goSearch/%s/%s/%s/%s/%s/%s" searchForSpecific
    pathScan "/YeastPloidy/mergeMe/%s/%s/%s/%s/%s/%s" mergeMyGO
    pathScan "/YeastPloidy/goSpecHeat/%s/%s" searchForSpecificHeat

    Files.browseHome
    RequestErrors.NOT_FOUND "How did you end up here? Page not found :("     

  ] 

// Root port to 8083
let port = Sockets.Port.Parse "8083"

// Set homefolder to public
let homeFolder =  Some (Path.GetFullPath "./public")

// Set additional MimeType for JSON strings
let mimeTypes =
  Writers.defaultMimeTypesMap
    @@ (function | ".json" -> Writers.createMimeType "application/json" true | _ -> None)

// Bin port and homefolder to simple server config 
let serverConfig = 
    { defaultConfig with
       homeFolder = homeFolder;
       bindings = [ HttpBinding.create HTTP IPAddress.Loopback port ]
       mimeTypesMap = mimeTypes}

// Open page on execute in the users default browser.
System.Diagnostics.Process.Start("http://localhost:8083/YeastPloidy/") |> ignore

// Run App

startWebServer serverConfig app
module MyTypes

// Plotly compatible, simple types, translateable by JSON serialization
module Types =
    
    type ProcessedProteinData = {

        p1:float
        p2:float
        p3:float
        p4:float
        diffP1toP2:float
        diffP2toP3:float
        diffP3toP4:float
        tendency:float
        geneName:string
        id:int
        }

    let createProcessedProteinData f s t fo d1 d2 d3 tend n i = { p1 = f; p2 = s; p3 = t; p4 = fo; diffP1toP2 = d1; diffP2toP3 = d2; diffP3toP4 = d3; tendency = tend; geneName = n; id = i}

    type ProcessedRNAData = {

         p1:float
         p2:float
         p3:float
         p4:float
         diffP1toP2:float
         diffP2toP3:float
         diffP3toP4:float
         tendency:float
         geneName:string
         chromosome:string
         }

    let createProcessedRNAData f s t fo d1 d2 d3 tend n c = { p1 = f; p2 = s; p3 = t; p4 = fo; diffP1toP2 = d1; diffP2toP3 = d2; diffP3toP4 = d3; tendency = tend; geneName = n; chromosome = c;}

    type myLine ={
        color:string
        width:int
        shape: string
        }

    let createmyLine c w s = { color = c; width = w; shape = s}

    type linePlotData = {
        x: string list
        y: float list 
        name: string
        line: myLine
        mode: string
        opacity:float
        }
    
    type linePlot = {
         data:linePlotData list
        } 
    
    let createLinePlotData lx ly lname lline lmode o = { x = lx; y = ly; name = lname; line = lline; mode = lmode; opacity = o }
    let createLinePlot lpD = { data = lpD}
    
    type heatmap = {
     x: string list
     y: string list
     z: float list list
     }
    
    let createHeatMap xx yy zz = { x = xx; y = yy; z = zz}


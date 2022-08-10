$('.dropdown-menu a').click(function () {
    $('#drop').text($(this).text());
});

$('#initLine').on('click', function () {
    var initLines = '/YeastPloidy/PEData/ ' + first.value + '/ ' + second.value + '/' + drop.textContent;
    Plotly.d3.json(initLines, function (error, figure) {
        if (error) throw error;

        var layout = {
            showlegend: true,
            hovermode : 'closest',
            xaxis: {
                dtick: 1,
                //tickvals: figure.map(function(a) {return a.y;}),
                //ticktext: figure.map(function (a) { return a.y; }),
                title: 'Ploidy'
            },
            yaxis: {
                title: 'Ratio (log2)'
            }
            //legend: {
            //    "orientation": "h",
            //},
            
        };

        Plotly.newPlot('myDiv', figure.data, layout, { showLink: true, plotlyServerURL: "https://chart-studio.plotly.com" });
    });
});

$('#initHeat').on('click', function () {
    var initHeats = '/YeastPloidy/PEDataHeat/ ' + first.value + '/ ' + second.value + '/' + drop.textContent;
    Plotly.d3.json(initHeats, function (error, figure) {

        if (error) throw error;
        var data = [
            {
                x: figure.x,
                y: figure.y,
                z: figure.z,
                type: 'heatmap',
                colorscale: [
    ['0.0', 'rgb(5,48,97)'],
    ['0.111111111111', 'rgb(33,102,172)'],
    ['0.222222222222', 'rgb(67,147,195)'],
    ['0.333333333333', 'rgb(209,229,240)'],
    ['0.444444444444', 'rgb(247,247,247)'],
    ['0.555555555556', 'rgb(253,219,199)'],
    ['0.666666666667', 'rgb(244,165,130)'],
    ['0.777777777778', 'rgb(214,86,77)'],
    ['0.888888888889', 'rgb(178,24,43)'],
    ['1.0', 'rgb(103,0,31)']
  ]
            }
        ];
        var layout = {
            showlegend: true,
            xaxis: {
                dtick: 1
            },
			yaxis: {
		    	automargin: true
		    },
			width: 750,
			height: 810,
        };

        Plotly.newPlot('myDiv', data, layout, { showLink: true, plotlyServerURL: "https://chart-studio.plotly.com" });
    });
});

$('#searchGOBP').on('click', function () {
    var initLines = '/YeastPloidy/goBP/ ' + first.value + '/ ' + second.value + '/' + gobp.value + '/' + drop.textContent;
    Plotly.d3.json(initLines, function (error, figure) {
        if (error) throw error;

        var layout = {
            showlegend: true,
            hovermode: 'closest',

            xaxis: {
                dtick: 1,
                //tickvals: figure.map(function(a) {return a.y;}),
                //ticktext: figure.map(function (a) { return a.y; }),
                title: 'Ploidy'
            },
            yaxis: {
                title: 'Ratio (log2)'
            }
            //legend: {
            //    "orientation": "h",
            //},

        };

        Plotly.newPlot('myDiv', figure.data, layout, { showLink: true, plotlyServerURL: "https://chart-studio.plotly.com" });
    });
});

$('#searchGOBPHeat').on('click', function () {
    var initHeats = '/YeastPloidy/goBPHeat/ ' + first.value + '/ ' + second.value + '/' + gobp.value + '/' + drop.textContent;
    Plotly.d3.json(initHeats, function (error, figure) {

        if (error) throw error;
        var data = [
            {
                x: figure.x,
                y: figure.y,
                z: figure.z,
                type: 'heatmap',
                colorscale: [
    ['0.0', 'rgb(5,48,97)'],
    ['0.111111111111', 'rgb(33,102,172)'],
    ['0.222222222222', 'rgb(67,147,195)'],
    ['0.333333333333', 'rgb(209,229,240)'],
    ['0.444444444444', 'rgb(247,247,247)'],
    ['0.555555555556', 'rgb(253,219,199)'],
    ['0.666666666667', 'rgb(244,165,130)'],
    ['0.777777777778', 'rgb(214,86,77)'],
    ['0.888888888889', 'rgb(178,24,43)'],
    ['1.0', 'rgb(103,0,31)']
  ]
            }
        ];
        var layout = {
            showlegend: true,
            xaxis: {
                dtick: 1
            },
			yaxis: {
		    	automargin: true
		    },
			width: 750,
			height: 810,
        };

        Plotly.newPlot('myDiv', data, layout, { showLink: true, plotlyServerURL: "https://chart-studio.plotly.com" });
    });

});
$('#searchGOCC').on('click', function () {
    var initLines = '/YeastPloidy/goCC/ ' + first.value + '/ ' + second.value + '/' + gocc.value + '/' + drop.textContent;
    Plotly.d3.json(initLines, function (error, figure) {
        if (error) throw error;

        var layout = {
            showlegend: true,
            hovermode: 'closest',

            xaxis: {

                dtick: 1,
                //tickvals: figure.map(function(a) {return a.y;}),
                //ticktext: figure.map(function (a) { return a.y; }),
                title: 'Ploidy'
            },
            yaxis: {
                title: 'Ratio (log2)'
            }
            //legend: {
            //    "orientation": "h",
            //},
        };

        Plotly.newPlot('myDiv', figure.data, layout, { showLink: true, plotlyServerURL: "https://chart-studio.plotly.com" });
    });
});

$('#searchGOCCHeat').on('click', function () {
    var initHeats = '/YeastPloidy/goCCHeat/ ' + first.value + '/ ' + second.value + '/' + gocc.value + '/' + drop.textContent;
    Plotly.d3.json(initHeats, function (error, figure) {

        if (error) throw error;
        var data = [
            {
                x: figure.x,
                y: figure.y,
                z: figure.z,
                type: 'heatmap',
                colorscale: [
    ['0.0', 'rgb(5,48,97)'],
    ['0.111111111111', 'rgb(33,102,172)'],
    ['0.222222222222', 'rgb(67,147,195)'],
    ['0.333333333333', 'rgb(209,229,240)'],
    ['0.444444444444', 'rgb(247,247,247)'],
    ['0.555555555556', 'rgb(253,219,199)'],
    ['0.666666666667', 'rgb(244,165,130)'],
    ['0.777777777778', 'rgb(214,86,77)'],
    ['0.888888888889', 'rgb(178,24,43)'],
    ['1.0', 'rgb(103,0,31)']
  ]
            }
        ];
        var layout = {
            showlegend: true,
            xaxis: {
                dtick: 1
            },
			width: 750,
			height: 810,	
			yaxis: {
		    	automargin: true
		    },			
        };

        Plotly.newPlot('myDiv', data, layout, { showLink: true, plotlyServerURL: "https://chart-studio.plotly.com" });
    });
});

$('#initSearch').on('click', function () {
    var initLines = '/YeastPloidy/goSearch/' + third.value + '/' + drop.textContent + '/' + gobp.value + '/' + gocc.value + '/ ' + first.value + '/ ' + second.value;
    Plotly.d3.json(initLines, function (error, figure) {
        if (error) throw error;

        var layout = {
            showlegend: true,
            hovermode: 'closest',

            xaxis: {
                dtick: 1,
                //tickvals: figure.map(function(a) {return a.y;}),
                //ticktext: figure.map(function (a) { return a.y; }),
                title: 'Ploidy'
            },
            yaxis: {
                title: 'Ratio (log2)'
            }
            //legend: {
            //    "orientation": "h",
            //},
        };

        Plotly.newPlot('myDiv', figure.data, layout, { showLink: true, plotlyServerURL: "https://chart-studio.plotly.com" });
    });
});

$('#mergeGo').on('click', function () {
    var initLines = '/YeastPloidy/mergeMe/' + third.value + '/' + drop.textContent + '/' + gobp.value + '/' + gocc.value + '/ ' + first.value + '/ ' + second.value;
    Plotly.d3.json(initLines, function (error, figure) {
        if (error) throw error;

        var layout = {
            showlegend: true,
            hovermode: 'closest',

            xaxis: {
                dtick: 1,
                //tickvals: figure.map(function(a) {return a.y;}),
                //ticktext: figure.map(function (a) { return a.y; }),
                title: 'Ploidy'
            },
            yaxis: {
                title: 'Ratio (log2)'
            }
            //legend: {
            //    "orientation": "h",
            //},
        };

        Plotly.newPlot('myDiv', figure.data, layout, { showLink: true, plotlyServerURL: "https://chart-studio.plotly.com" });
    });
});

$('#addLine').on('click', function () {
    var addLines = '/YeastPloidy/goSearch/' + third.value + '/' + drop.textContent + '/' + gobp.value + '/' + gocc.value + '/ ' + first.value + '/ ' + second.value;
    Plotly.d3.json(addLines, function (error, figure) {

        if (error) throw error;

        Plotly.plot('myDiv', figure.data);
    });
});

$('#specificHeat').on('click', function () {
    var initHeats = '/YeastPloidy/goSpecHeat/ ' + third.value + '/' + drop.textContent;
    Plotly.d3.json(initHeats, function (error, figure) {

        if (error) throw error;
        var data = [
            {
                x: figure.x,
                y: figure.y,
                z: figure.z,
                type: 'heatmap',
                colorscale: [
    ['0.0', 'rgb(5,48,97)'],
    ['0.111111111111', 'rgb(33,102,172)'],
    ['0.222222222222', 'rgb(67,147,195)'],
    ['0.333333333333', 'rgb(209,229,240)'],
    ['0.444444444444', 'rgb(247,247,247)'],
    ['0.555555555556', 'rgb(253,219,199)'],
    ['0.666666666667', 'rgb(244,165,130)'],
    ['0.777777777778', 'rgb(214,86,77)'],
    ['0.888888888889', 'rgb(178,24,43)'],
    ['1.0', 'rgb(103,0,31)']
  ]
            }
        ];

        var layout = {
            showlegend: true,
            xaxis: {
                dtick: 1
            },
			yaxis: {
		    	automargin: true
		    },
			width: 750,
			height: 810,
        };

        Plotly.newPlot('myDiv', data, layout, { showLink: true, plotlyServerURL: "https://chart-studio.plotly.com" });
    });
});

//// RNA!

//$('#initLineR').on('click', function () {
//    var initLines = '/RNAEData/ ' + (firstR.value) + '/ ' + (secondR.value);
//    Plotly.d3.json(initLines, function (error, figure) {
//        if (error) throw error;

//        var layout = {
//            xaxis: {
//                dtick: 1,
//                //tickvals: figure.map(function(a) {return a.y;}),
//                //ticktext: figure.map(function (a) { return a.y; }),
//                title: 'Ploidy'
//            },
//            yaxis: {
//                title: 'Ratio (log2)'
//            },
//            //legend: {
//            //    "orientation": "h",
//            //},
//        };

//        Plotly.newPlot('myDiv', figure.data, layout);
//    });
//});

//$('#searchGOBPR').on('click', function () {
//    var initLines = '/goBPR/ ' + (firstR.value) + '/ ' + (secondR.value) + '/' + (gobpR.value);
//    Plotly.d3.json(initLines, function (error, figure) {
//        if (error) throw error;

//        var layout = {
//            xaxis: {
//                dtick: 1,
//                //tickvals: figure.map(function(a) {return a.y;}),
//                //ticktext: figure.map(function (a) { return a.y; }),
//                title: 'Ploidy'
//            },
//            yaxis: {
//                title: 'Ratio (log2)'
//            },
//            //legend: {
//            //    "orientation": "h",
//            //},
//        };

//        Plotly.newPlot('myDiv', figure.data, layout);
//    });
//});

//$('#searchGOCCR').on('click', function () {
//    var initLines = '/goCCR/ ' + (firstR.value) + '/ ' + (secondR.value) + '/' + (goccR.value);
//    Plotly.d3.json(initLines, function (error, figure) {
//        if (error) throw error;

//        var layout = {
//            xaxis: {
//                dtick: 1,
//                //tickvals: figure.map(function(a) {return a.y;}),
//                //ticktext: figure.map(function (a) { return a.y; }),
//                title: 'Ploidy'
//            },
//            yaxis: {
//                title: 'Ratio (log2)'
//            },
//            //legend: {
//            //    "orientation": "h",
//            //},
//        };

//        Plotly.newPlot('myDiv', figure.data, layout);
//    });
//});

//$('#initSearchR').on('click', function () {
//    var initLines = '/goSearchR/ ' + (thirdR.value);
//    Plotly.d3.json(initLines, function (error, figure) {
//        if (error) throw error;

//        var layout = {
//            xaxis: {
//                dtick: 1,
//                //tickvals: figure.map(function(a) {return a.y;}),
//                //ticktext: figure.map(function (a) { return a.y; }),
//                title: 'Ploidy'
//            },
//            yaxis: {
//                title: 'Ratio (log2)'
//            },
//            //legend: {
//            //    "orientation": "h",
//            //},
//        };

//        Plotly.newPlot('myDiv', figure.data, layout);
//    });
//});

//$('#addLineR').on('click', function () {
//    var addLines = '/goSearchR/' + (thirdR.value);
//    Plotly.d3.json(addLines, function (error, figure) {

//        if (error) throw error;

//        Plotly.plot('myDiv', figure.data);
//    });
//});
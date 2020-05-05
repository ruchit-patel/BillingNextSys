"use strict";

if (document.readyState === 'loading') {  // Loading hasn't finished yet
    document.addEventListener('DOMContentLoaded', callUpdategraph);
} else {  // `DOMContentLoaded` has already fired
    callUpdategraph();
}

function callUpdategraph() {
    $.ajax({
        url: '/Dashboard/Admin?handler=UpdateGraph',
        type: 'post',
        data: {
            type: 'CashFlows'
        },
        headers: {
            "MY-XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        dataType: 'json',
        success: function (data) {
            console.info(data);
        }
    });
}

var connection = new signalR.HubConnectionBuilder().withUrl("/graphHub").build();


connection.on("sendCashFlowsData", (graphData) => {
    var graphJSONData = JSON.parse(graphData);
        getGraphBorderColorsAsync().then(borderdata =>
            getGraphColorsAsync()
                .then(data =>
                    generateGraph(data, graphJSONData, borderdata)));
});
connection.start().catch(function (err) {
     return console.error(err.toString());
});


async function getGraphColorsAsync() {
    let response = await fetch(location.origin + "/js/ColorCodeArray.json");
    let data = await response.json()
    return data;
}

async function getGraphBorderColorsAsync() {
    let response = await fetch(location.origin + "/js/ColorCodeBorderArray.json");
    let data = await response.json()
    return data;
}


function generateGraph(graphColors, graphJSONData, borderdata) {
 
    var FormatedDataMain = [];
    var tempLabels = [];
    var ctx = document.getElementById('myChart').getContext('2d');

    graphJSONData.forEach(element => {
        var tempData = [];
        element.GraphContent.forEach(graphElement => {
            if (tempLabels.length < element.GraphContent.length) {
                    tempLabels.push(graphElement.Label.split(' ')[0]);
            }
            tempData.push(graphElement.Data);
        });

        var FormatedData = {};

        FormatedData.label = element.Type;

        FormatedData.data = tempData;

        FormatedData.backgroundColor = [randomColorChooser(graphColors)];

        var bordercolor = [];
        for (var i = 0; i < tempData.length; i++) {
            bordercolor.push(randomColorChooser(borderdata));
        }
        FormatedData.borderColor = bordercolor;

        FormatedData.borderWidth = 1;

        FormatedDataMain.push(FormatedData);
    });

    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: tempLabels,
            datasets: FormatedDataMain
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}

function randomColorChooser(graphColors){
    return graphColors.ColorCodes[Math.floor(Math.random() * graphColors.ColorCodes.length)];
}
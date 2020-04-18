"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/graphHub").build();


connection.on("sendToUser", (graphData) => {
    var graphJSONData= JSON.parse(graphData);

    var colorcodes;

    var FormatedDataMain = [];
    var tempLabels = [];
    getUserAsync()
        .then(data =>
            colorcodes = data);
    
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
        console.log(tempData);
        FormatedData.label = element.Type;
        FormatedData.data = tempData;

        

        var randomIndex = Math.floor(Math.random() * colorcodes.ColorCodes[0].length);
        console.log(colorcodes.ColorCodes);
        var randomElement = colorcodes.ColorCodes[randomIndex];
        console.log(randomElement);

        FormatedData.backgroundColor=[
            'rgba(255, 99, 132, 0.2)'
        ];
        FormatedData.borderColor=[
            'rgba(255, 99, 132, 1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)',
            'rgba(153, 102, 255, 1)',
            'rgba(255, 159, 64, 1)'
        ];
        FormatedData.borderWidth = 1;
        FormatedDataMain.push(FormatedData);
    });
    console.log(FormatedDataMain);

   


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


    
});
connection.start().catch(function (err) {
     return console.error(err.toString());
});


async function getUserAsync() {
    let response = await fetch(location.origin + "/js/ColorCodeArray.json");
    let data = await response.json()
    return data;
}



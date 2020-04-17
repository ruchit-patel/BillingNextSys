"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/graphHub").build();


connection.on("sendToUser", (graphData) => {
    
    graphlabels.forEach(function(part, index, theArray) {
        theArray[index] = theArray[index].split('T')[0];
      });
    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: graphlabels, 
            datasets: [{
                label: 'Inward Cash Flow',
                data: graphData,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }
           ]
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






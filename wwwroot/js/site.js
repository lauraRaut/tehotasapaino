// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//const axios = require('axios').default;

//async function uploadFile (event) {
//    const file = event.target.files[0]
//    axios.post('upload_file', file, {
//        headers: {
//            'Content-Type': file.type
//        }
//    })
//}

//axios.








window.onload = function () {
    const topicData = JSON.parse(document.getElementById('appdata').dataset.obj);
    console.log(topicData);
    console.log(topicData[0].position);
   


    const priceList = topicData.map(sortPricesFromData);

    function sortPricesFromData(topicData) {
        return topicData.priceamount;
    }

    console.log(priceList);

    const hourPositionList = topicData.map(sortPositionsFromData);

    function sortPositionsFromData(topicData) {
        return topicData.position;
    }
    showChart(priceList, hourPositionList);

}


    function showChart(priceList, hourPositionList) {

        const ctx = document.getElementById("myChart");
        // Mychartista voi tehdä funktion, joka ottaa sisään x ja y koordinaatit sisään
        const myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: hourPositionList,
                datasets: [{
                    label: 'Sähkönhinta',
                    data: priceList,
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
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
                }]
            },
            options: {
                scales: {
                    y: {

                        beginAtZero: true
                    }
                }
            }

        });

    }


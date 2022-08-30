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
    const priceData = JSON.parse(document.getElementById('appdata').dataset.obj);
    console.log(priceData);

    const priceList = priceData.map(sortPricesFromData);

    function sortPricesFromData(priceData) {
        return priceData.Priceamount;
    }

    const hourPositionList = priceData.map(sortPositionsFromData);

    function sortPositionsFromData(priceData) {
        return priceData.Position;
    }

    showChart(priceList, hourPositionList);
}


    function showChart(priceList, hourPositionList) {

        const ctx = document.getElementById("priceConsumptionGraph");
        // Mychartista voi tehdä funktion, joka ottaa sisään x ja y koordinaatit sisään
        const myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: hourPositionList,
                datasets: [{
                    label: 'Sähkönhinta',
                    data: priceList,
                    stepped: true,
                    borderColor: "rgba(255,99,132,1)",
                    borderWidth: 2,
                    hoverBackgroundColor: "rgba(255,99,132,0.4)",
                    hoverBorderColor: "rgba(255,99,132,1)"
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                scales: {
                    y: {
                        ticks: {
                            color: "white"
                        },
                        grid: { color: "rgba(255,255,255,0.2"},
                        beginAtZero: true
                    },
                    x: {
                        ticks: {
                            color: "white"
                        },
                        grid: {
                            color: "rgba(255,255,255,0.2",
                            display: true
                        }
                    }
                }
            }

        });

}

document.addEventListener('click', function (event) {
    const id = event.target.dataset.id;
    if (!id) return;

    if (id === "uploadfile") {
        showUploadModal(event);
    }
    else {
        return;
    }
});


async function showUploadModal(event) {
    const modalID = "uploadFileModal";
    const myModal = new bootstrap.Modal(document.getElementById(modalID), {
        keyboard: false,
        backdrop: "static"
    });
    myModal.show();
}
var averagePrice = document.getElementById('averagePrice').innerText;

function CalculateMachine(id, priceid, kwh) {
    var result = document.getElementById(id).value;
    var calc = (result * kwh) * averagePrice / 100;

    document.getElementById(priceid).innerHTML = calc.toFixed(2);
    
}








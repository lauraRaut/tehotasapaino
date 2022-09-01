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
    const usageData = JSON.parse(document.getElementById('consumptiondata').dataset.obj);
    console.log(usageData);

    const priceList = priceData.map(sortPricesFromData);

    function sortPricesFromData(priceData) {
        return priceData.Priceamount;
    }

    const hourPositionList = priceData.map(sortPositionsFromData);

    function sortPositionsFromData(priceData) {
        return priceData.Position;
    }

    showChart(priceList, hourPositionList, usageData);
}//tähän mahdollisesti kutsun sisään usageData
// uudessa chartissa uusi datasetti, mutta label edelleen hourPositionisr


function showChart(priceList, hourPositionList, usageData) {

    const ctx = document.getElementById("priceConsumptionGraph");
    // Mychartista voi tehdä funktion, joka ottaa sisään x ja y koordinaatit sisään
    const myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: hourPositionList,
            datasets: [{
                label: 'Electricity price',
                data: priceList,
                stepped: true,
                borderColor: "rgba(255,99,132,1)",
                borderWidth: 2,
                hoverBackgroundColor: "rgba(255,99,132,0.4)",
                hoverBorderColor: "rgba(255,99,132,1)",
                yAxisID: 'y',
                pointRadius: 0,
                pointStyle: 'rectRounded',
                pointBorderWidth: 0,
                hitRadius: 10
            },
            {
                label: 'Your consumption',
                data: usageData,
                stepped: true,
                borderColor: "rgba(99, 255, 222,1)",
                borderWidth: 2,
                hoverBackgroundColor: "rgba(99, 255, 222,0.4)",
                hoverBorderColor: "rgba(99, 255, 222,1)",
                yAxisID: 'y1',
                pointRadius: 0
            }


            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    labels: {
                        color: "white"
                    }
                }
            
            },
            scales: {
                y: {
                    type: 'linear',
                    display: true,
                    position: 'left',
                    ticks: {
                        color: "rgba(255,99,132,1)",
                        callback: function (value, index, ticks) {
                            return value + ' c/kWh';
                        },
                        font: { size: 10 }
                    },
                    grid: { color: "rgba(255,255,255,0.2)" },
                    beginAtZero: true
                },

                y1: {
                    type: 'linear',
                    display: true,
                    position: 'right',
                    ticks: {
                        color: "rgba(99, 255, 222,1)",
                        callback: function (value, index, ticks) {
                            return value + ' kWh';
                        },
                        font: {size: 10}

                    },

                    x: {
                        ticks: {
                            color: "white",
                           
                        },
                        grid: {
                            color: "white",
                            display: true
                        }
                    }
                }
            }
        }
    });



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
}


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


function changeButtonToSpinner(buttonElem, spinnerBtnId) {
    event.target
    buttonElem.replaceWith(SpinnerButtonHTML(spinnerBtnId));
}

function SpinnerButtonHTML(uniqueID) {
    const btn_elem = document.createElement("button");
    btn_elem.disabled = true;
    btn_elem.id = uniqueID;
    btn_elem.className = "btn btn-outline-dark btn-sm text-dark";
    btn_elem.type = "button";
    btn_elem.textContent = "Saving...";

    const span_elem = document.createElement("span");
    span_elem.className = "spinner-grow spinner-grow-sm text-dark";
    span_elem.role = "status";

    btn_elem.appendChild(span_elem);

    return btn_elem;
}








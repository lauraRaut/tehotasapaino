// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//async function uploadFile (event) {
//    const file = event.target.files[0]
//    axios.post('upload_file', file, {
//        headers: {
//            'Content-Type': file.type
//        }
//    })
//}

//axios.


var isPriceHigh = false;

document.addEventListener('click', function (event) {
    
    console.log(event.target.dataset);

    if (isPriceHigh === true) {

    if (event.target.dataset.setLightColor === "E4E47A" || event.target.dataset.setLightColor === "FF6363" || event.target.dataset.setLightColor === "00FF21") {
        console.log(event.target.dataset);
        document.getElementById('userSelectedColor').id = "";
        event.target.id = "userSelectedColor";
        postLightStateToServer(event.target.dataset,isPriceHigh);
        }
    }

    if (event.target.dataset.setAction === "inc") {
        counterElem = document.getElementById('selectedPrice');
        const val = parseInt(document.getElementById('selectedPrice').innerText);
        const newVal = val + 10;
        counterElem.innerText = newVal.toString();

        
    }

    if (event.target.dataset.setAction === "dec") {
        counterElem = document.getElementById('selectedPrice');
        const val = parseInt(document.getElementById('selectedPrice').innerText);
        const newVal = val - 10;
        if (newVal < 0) {
        counterElem.innerText = 0;
        }
        else {
            counterElem.innerText = newVal;
        }

    }

});

function postLightStateToServer(dataset,isPriceHigh) {

    axios.interceptors.request.use(request => {
        console.log('Starting Request', JSON.stringify(request, null, 2))
        return request
    })

    axios({
        method: 'post',
        url: "/PriceLightAlert/lightstate",
        headers: { 'Content-Type': 'application/json'},
        data: {
            AlertLightHexColor: dataset.setLightColor,
            isPriceHight: String(isPriceHigh)
        }
    })
        .then((data) => console.log(data))
        .then((response) => {
            console.log(response);
        }, (error) => {
            console.log(error);
        });
}



window.onload = function () {
    const priceData = JSON.parse(document.getElementById('appdata').dataset.obj);

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
        const ctx = document.getElementById('priceConsumptionGraph');
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
                    pointRadius: 3,
                    pointStyle: 'rectRounded',
                    pointBorderWidth: 10,
                    hitRadius: 10
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                onClick: (evt, el, chart) => {

                    if (el.length > 0) {
                        tooltipData = chart.data.datasets[el[0].datasetIndex].data[el[0].index]
                        priceAlertHander(tooltipData)
                    }
                },
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


function priceAlertHander(selectedDayAheadPrice) {
    console.log(selectedDayAheadPrice)
    const priceLevelCtn = document.getElementById('selectedPrice').innerText

    if (priceLevelCtn <= selectedDayAheadPrice) {
        isPriceHigh = true
        const selectedColor = document.getElementById('userSelectedColor');
        console.log(selectedColor.dataset);
        postLightStateToServer(selectedColor.dataset,isPriceHigh)
        document.getElementById('alertIndicator').innerText = "Alert active!"
        document.getElementById('selectedPrice').style.backgroundColor = "rgb(255, 99, 99)"
        
    }

    else {
        isPriceHigh = false
        document.getElementById('alertIndicator').innerText = "Alert not active!"
        document.getElementById('selectedPrice').style.backgroundColor = "rgb(0, 0, 0)"
        const dummydata = { dataset: "" }
        postLightStateToServer(dummydata, isPriceHigh)
    }
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


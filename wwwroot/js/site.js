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

                        beginAtZero: true
                    },
                    x: {
                        grid: {
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

    if (id === "") {

    }

    let elem = document.getElementById(id);

    elem.hidden = !elem.hidden;
});


async function showUploadModal(event) {
    const topicID = event.target.dataset.id;
    const modalID = "removeModal-" + topicID;
    const myModal = new bootstrap.Modal(document.getElementById(modalID), {
        keyboard: false,
        backdrop: "static"
    });
    myModal.show();
}


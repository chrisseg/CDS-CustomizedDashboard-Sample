var speedLineChart;

var getCurrentTime = function () {

    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();

    return time;

};

var lineChartConfig = {
    type: 'line',
    data: {
        datasets: [
            {
                data: [0],
                borderColor: '#00bf63',
                fill: false,
                label:'Spindle Speed'
            },

            {
                data: [0],
                borderColor: '#ff7000',
                fill: false,
                label: 'Feed Rate'
            }

        ],
        labels: [getCurrentTime()]

    },
    options: {
        legend: {
            display: true
        },
        elements: {
            line: {
                tension: 0 // disables bezier curves
            },
            point: {
                radius: 0
            }
        },
        responsive: true,
        maintainAspectRatio: false

    }
};

function filterMsg(msg) {

    if (msg.equipmentId === 'FAMT001') {
        updateFamt001(msg);
    } else if (msg.equipmentId === 'GBox01'){
        updateGBox01(msg);
    }

}

function initFamt001() {
    var linectx = document.getElementById('spindle-speed-chart').getContext('2d');

    Chart.defaults.global.defaultFontColor = '#000';
    speedLineChart = new Chart(linectx, lineChartConfig); 
}



function updateFamt001(msg) {

    var updateSpeed = msg.spindleSpeed_actualSpeed;
    var updatefeedRate = msg.feedRate_actualFeedRate;


    if (lineChartConfig.data.datasets[0].data.length > 30) {

        lineChartConfig.data.labels.splice(0, 1);
        lineChartConfig.data.datasets[0].data.splice(0, 1);
        lineChartConfig.data.datasets[1].data.splice(0, 1);

    }

    lineChartConfig.data.labels.push(getCurrentTime());
    lineChartConfig.data.datasets[0].data.push(updateSpeed);
    lineChartConfig.data.datasets[1].data.push(updatefeedRate);
    speedLineChart.update();  

}

function updateGBox01(msg) {
    $('#widget-card-humidity-value').text(msg.Humidity);
    $('#widget-card-temperature-value').text(msg.TemperatureC);
}

function initSignalR() {

    //msfapiservice
    $.connection.hub.url = 'https://admin.iot-cds.net/signalr';

    var hub = $.connection.RTMessageHub;

    hub.client.onReceivedMessage = function (message) {
       filterMsg($.parseJSON(message));
    };

    $.connection.hub.start({ withCredentials: false }).done(function () {
        hub.server.register(_COMPANYID);
    });
}

$(document).ready(function () {
    initSignalR();
    initFamt001();
});
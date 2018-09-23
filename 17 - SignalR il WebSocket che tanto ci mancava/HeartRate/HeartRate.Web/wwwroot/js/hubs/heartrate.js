/** 
 * Initialize signalR
 */
function InitializeRealTime() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/heartratehub")
        .build();

    connection.on("ReceiveHeartRate", (heartRateData) => {
        UpdateCharts(heartRateData);
    });
    
    connection.start()
        .catch(err => console.error(err.toString()));
}

var heartRateDataPoints = [];
var heartRateDataPointsTemp = [];
var chart;
/**
 * Initialize Chart
 */
function InitializeCharts() {
    chart = new CanvasJS.Chart("heartRateContainer", {
        title: { text: "Heart Rate Real-Time" },
        animationEnabled: true,
        animationDuration: 500, 
        zoomEnabled: true,
        axisY: { includeZero: false },
        data: [{
            type: "spline",
            xValueType: "dateTime",
            dataPoints: heartRateDataPoints
        }]
    });
    chart.render();
}
function compareDataPointXAscend(dataPoint1, dataPoint2) {
    return dataPoint1.x - dataPoint2.x;
}
/**
 * Aggiorna il grafico
 * @param {any} lastPoint
 */
function UpdateCharts(lastPoint) {
    heartRateDataPoints.push({ x: new Date(lastPoint.x), y: lastPoint.y });
    // Ordinamento temporale
    chart.options.data[0].dataPoints.sort(compareDataPointXAscend);    
    // Rimozione punti vecchi
    if (heartRateDataPoints.length > 50) {
        heartRateDataPoints.shift();
    }
    chart.render();
}

function clearGraph() {
    heartRateDataPoints = [];
    chart.render();
}

// Run
InitializeRealTime();
InitializeCharts();
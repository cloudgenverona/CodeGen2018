function InitializeRealTime() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/heartratehub")
        .build();

    connection.on("ReceiveAlert", (heartRateData) => {
        showAlert(heartRateData);
    });

    connection.start()
        .catch(err => console.error(err.toString()));
}

function showAlert(heartRateData) {
    $('.modal-body').html('\<h4 class="alert-heading"> Attenzione, rilevato valore:' + heartRateData.y + '!</h4>');
    $('#alertModal').modal('show');
}

InitializeRealTime();
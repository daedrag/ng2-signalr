(function () {
    var todoHub = $.connection.todoHub;
    todoHub.client.hi = function(s) {
        console.log("Client received: " + s);
    };

    var stockHub = $.connection.stockHub;
    stockHub.client.onNewStock = function(stockInfo) {
        console.log(stockInfo);
    };

    $.connection.hub.url = "http://localhost:5000/signalr";
    // start the connection
    $.connection.hub.start()
        .done(response => {
            console.log("SignalR connection established!");
            todoHub.server.hello("world");
            stockHub.server.subscribe("AAPL");
        })
        .fail(error => {
            console.log("SignalR connection error!");
            console.error(error);
        });
}());
using System;
using backend.Services;
using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs
{
    public class StockHub : Hub
    {
        private readonly IStockLiveUpdateFactory _stockLiveUpdateFactory;
        public StockHub(IStockLiveUpdateFactory stockLiveUpdateFactory)
        {
            _stockLiveUpdateFactory = stockLiveUpdateFactory;
        }
        public void Subscribe(string symbol)
        {
            var connectionId = Context.ConnectionId;
            try
            {
                var stockLiveUpdate = _stockLiveUpdateFactory.Get(symbol);
                var callback = new StockLiveUpdate.StockLiveUpdateEventHandler((sender, args) =>
                {
                    var stockArgs = args as StockLiveUpdateEventArgs;
                    if (stockArgs == null) return;
                    Clients.Client(connectionId).onNewStock(stockArgs.StockInfo);
                    Console.WriteLine("OnNewStock() called");
                });
                stockLiveUpdate.Subscribe(connectionId, callback);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
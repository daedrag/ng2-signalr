using System;
using System.Threading.Tasks;
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

        public override Task OnDisconnected(bool stopCalled)
        {
            var clientId = Context.ConnectionId;
            var subscriptions = _stockLiveUpdateFactory.GetByClientId(clientId);
            return Task.Run(() => 
            {
                Console.WriteLine($"Client [{clientId}] disconnected. Unsubscribe from {subscriptions.Count} subscriptions");
                subscriptions.ForEach(stockLiveUpdate => stockLiveUpdate.Unsubscribe(clientId));
            });
        }
    }
}
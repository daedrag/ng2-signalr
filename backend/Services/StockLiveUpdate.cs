using System;
using System.Collections.Generic;
using System.Threading;
using backend.Models;

namespace backend.Services
{
    public class StockLiveUpdate
    {
        public delegate void StockLiveUpdateEventHandler(object sender, StockLiveUpdateEventArgs args);
        public event StockLiveUpdateEventHandler NewStockUpdated;
        protected readonly Dictionary<string, StockLiveUpdateEventHandler> ClientSubscriptions;
        protected readonly string Symbol;
        private readonly Timer _timer;

        public StockLiveUpdate(string symbol)
        {
            ClientSubscriptions = new Dictionary<string, StockLiveUpdateEventHandler>();
            Symbol = symbol;

            var updatePeriod = (new Random(DateTime.Now.Millisecond).NextDouble() + 0.1) * 3;
            _timer = new Timer(new TimerCallback(this.BroadcastStockInfo), null,
                TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(updatePeriod));
        }

        public bool HasClientId(string clientId)
        {
            lock (ClientSubscriptions)
            {
                return ClientSubscriptions.ContainsKey(clientId);
            }
        }

        public void Subscribe(string clientId, StockLiveUpdateEventHandler handler)
        {
            lock (ClientSubscriptions)
            {
                if (ClientSubscriptions.ContainsKey(clientId))
                {
                    Console.WriteLine($"Existing client [{clientId}] subscribes to symbol [{Symbol}]");
                    return;
                }
                Console.WriteLine($"New client [{clientId}] subscribes to symbol [{Symbol}]");
                ClientSubscriptions.Add(clientId, handler);
                NewStockUpdated += handler;
            }
        }

        public void Unsubscribe(string clientId)
        {
            lock (ClientSubscriptions)
            {
                if (!ClientSubscriptions.ContainsKey(clientId))
                    return;
                
                Console.WriteLine($"Client [{clientId}] unsubscribes from symbol [{Symbol}]");
                var handler = ClientSubscriptions[clientId];
                NewStockUpdated -= handler;
            }
        }

        protected void BroadcastStockInfo(Object stateInfo)
        {
            lock (this)
            {
                if (ClientSubscriptions.Count == 0) return;
                var newPrice = (new Random(DateTime.Now.Millisecond).Next(1, 10000) / 100.0);
                var newStockInfo = new StockInfo(Symbol, newPrice, DateTime.Now);
                OnNewStockUpdated(newStockInfo);
            }
        }

        protected virtual void OnNewStockUpdated(StockInfo stockInfo)
        {
            StockLiveUpdateEventHandler handler = NewStockUpdated;
            if (handler == null) return;
            Console.WriteLine($"New stock: {stockInfo.Symbol}, {stockInfo.Price}");
            handler(this, new StockLiveUpdateEventArgs { StockInfo = stockInfo });
        }
    }
}
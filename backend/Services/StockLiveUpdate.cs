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
        protected readonly HashSet<string> ClientSubscriptions;
        protected readonly string Symbol;
        private readonly Timer _timer;
        private readonly Random _randomizer;

        public StockLiveUpdate(string symbol)
        {
            ClientSubscriptions = new HashSet<string>();
            Symbol = symbol;

            _timer = new Timer(new TimerCallback(this.BroadcastStockInfo), null,
                TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3));
            _randomizer = new Random();
        }

        public void Subscribe(string clientId, StockLiveUpdateEventHandler handler)
        {
            lock (ClientSubscriptions)
            {
                if (ClientSubscriptions.Contains(clientId))
                {
                    Console.WriteLine($"Exisitng client [{clientId}] subscribes to symbol [{Symbol}]");
                    return;
                }
                Console.WriteLine($"New client [{clientId}] subscribes to symbol [{Symbol}]");
                ClientSubscriptions.Add(clientId);
                NewStockUpdated += handler;
            }
        }

        protected void BroadcastStockInfo(Object stateInfo)
        {
            lock (this)
            {
                if (ClientSubscriptions.Count == 0) return;
                var newPrice = (_randomizer.NextDouble() + 1) * 100;
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
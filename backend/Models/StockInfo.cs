using System;

namespace backend.Models
{
    public class StockInfo
    {
        public string Symbol { get; }
        public double Price { get; }
        public DateTime Time { get; }
        public StockInfo(string symbol, double price, DateTime time)
        {
            Symbol = symbol;
            Price = price;
            Time = time;
        }
    }
}
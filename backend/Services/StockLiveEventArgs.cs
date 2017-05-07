using System;
using backend.Models;

namespace backend.Services
{
    public class StockLiveUpdateEventArgs : EventArgs
    {
        public StockInfo StockInfo { get; set; }
    }
}
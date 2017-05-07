using System.Collections.Generic;

namespace backend.Services
{
    public class StockLiveUpdateFactory : IStockLiveUpdateFactory
    {
        protected Dictionary<string, StockLiveUpdate> ObjMap = new Dictionary<string, StockLiveUpdate>();
        public StockLiveUpdate Get(string symbol)
        {
            lock (ObjMap)
            {
                if (ObjMap.ContainsKey(symbol))
                    return ObjMap[symbol];
                var newObj = new StockLiveUpdate(symbol);
                ObjMap.Add(symbol, newObj);
                return newObj;
            }            
        }
    }
}
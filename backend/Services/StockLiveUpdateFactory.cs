using System.Collections.Generic;
using System.Linq;

namespace backend.Services
{
    public class StockLiveUpdateFactory : IStockLiveUpdateFactory
    {
        protected readonly Dictionary<string, StockLiveUpdate> ObjMap = new Dictionary<string, StockLiveUpdate>();
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

        public List<StockLiveUpdate> GetByClientId(string clientId)
        {
            lock (ObjMap)
            {
                return ObjMap.Values.Where(obj => obj.HasClientId(clientId)).ToList();
            }
        }
    }
}
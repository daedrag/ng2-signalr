using System.Collections.Generic;

namespace backend.Services
{
    public interface IStockLiveUpdateFactory
    {
        StockLiveUpdate Get(string symbol);
        List<StockLiveUpdate> GetByClientId(string clientId);
    }
}
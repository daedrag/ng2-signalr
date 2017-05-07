namespace backend.Services
{
    public interface IStockLiveUpdateFactory
    {
        StockLiveUpdate Get(string symbol);
    }
}
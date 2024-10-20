namespace Dashboard.Api.Services
{
    public interface IGrowthAnalytics
    {
        Task<double> GetUserGrowthComparedToYesterdayAsync();
        Task<double> GetProductGrowthComparedToYesterdayAsync();
        Task<double> GetOrderDeliveredGrowthComparedToYesterdayAsync();
        Task<double> GetOrderBeingDeliveredGrowthComparedToYesterdayAsync();
    }
}

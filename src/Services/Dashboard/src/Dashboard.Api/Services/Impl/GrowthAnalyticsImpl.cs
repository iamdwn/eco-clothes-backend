using DataAccess.Base;

namespace Dashboard.Api.Services.Impl
{
    public class GrowthAnalyticsImpl : IGrowthAnalytics
    {
        private readonly IUnitOfWork _unitOfWork;

        public GrowthAnalyticsImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<double> GetOrderBeingDeliveredGrowthComparedToYesterdayAsync()
        {
            throw new NotImplementedException();
        }

        public Task<double> GetOrderDeliveredGrowthComparedToYesterdayAsync()
        {
            throw new NotImplementedException();
        }

        public Task<double> GetProductGrowthComparedToYesterdayAsync()
        {
            throw new NotImplementedException();
        }

        public Task<double> GetUserGrowthComparedToYesterdayAsync()
        {
            throw new NotImplementedException();
        }
    }
}

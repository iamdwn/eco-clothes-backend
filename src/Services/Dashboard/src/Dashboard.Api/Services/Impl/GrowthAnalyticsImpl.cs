using DataAccess.Base;

namespace Dashboard.Api.Services.Impl
{
    public class GrowthAnalyticsImpl : IGrowthAnalytics
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderAnalytics _orderAnalytics;
        private readonly IProductAnalytics _productAnalytics;
        private readonly IUserAnalytics _userAnalytics;
        private readonly double _percentage = 0.01;
        private readonly double _x0Growth = 0.0;
        private readonly double _x1Growth = 1.0;
        private readonly double _x2Growth = 2.0;

        public GrowthAnalyticsImpl(IUnitOfWork unitOfWork, IProductAnalytics productAnalytics, IUserAnalytics userAnalytics, IOrderAnalytics orderAnalytics)
        {
            _unitOfWork = unitOfWork;
            _productAnalytics = productAnalytics;
            _userAnalytics = userAnalytics;
            _orderAnalytics = orderAnalytics;
        }

        public Task<double> GetOrderBeingDeliveredGrowthComparedToYesterdayAsync()
        {
            var today = _orderAnalytics.CountDailyOrdersBeingDeliveredAsync(DateTime.Now);
            var yesterday = _orderAnalytics.CountDailyOrdersBeingDeliveredAsync(DateTime.Now.AddDays(-1));
            return CalGrowthOfFigures((double)today.Result, (double)yesterday.Result);
        }

        public Task<double> GetOrderDeliveredGrowthComparedToYesterdayAsync()
        {
            var today = _orderAnalytics.CountDailyOrdersDeliveredAsync(DateTime.Now);
            var yesterday = _orderAnalytics.CountDailyOrdersDeliveredAsync(DateTime.Now.AddDays(-1));
            return CalGrowthOfFigures((double)today.Result, (double)yesterday.Result);
        }

        public Task<double> GetProductGrowthComparedToYesterdayAsync()
        {
            var today = _productAnalytics.CountDailyProductsAsync(DateTime.Now);
            var yesterday = _productAnalytics.CountDailyProductsAsync(DateTime.Now.AddDays(-1));
            return CalGrowthOfFigures((double)today.Result, (double)yesterday.Result);
        }

        public Task<double> GetUserGrowthComparedToYesterdayAsync()
        {
            var today = _userAnalytics.CountDailyUsersAsync(DateTime.Now);
            var yesterday = _userAnalytics.CountDailyUsersAsync(DateTime.Now.AddDays(-1));
            return CalGrowthOfFigures((double)today.Result, (double)yesterday.Result);
        }

        public Task<double> CalGrowthOfFigures(double num1, double num2)
        {
            if (num1 == 0 && num2 == 0) return Task.FromResult(_x0Growth);
            if (num1 == 0) return Task.FromResult(_x1Growth);
            if (num1 + num2 == 0) return Task.FromResult(_x2Growth);
            return Task.FromResult(((num1 - num2) / (num1 + num2)) / _percentage);
        }
    }
}

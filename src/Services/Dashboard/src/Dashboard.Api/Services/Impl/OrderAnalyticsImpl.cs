﻿using DataAccess.Base;
using DataAccess.Models;

namespace Dashboard.Api.Services.Impl
{
    public class OrderAnalyticsImpl : IOrderAnalytics
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderAnalyticsImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CountDailyOrdersBeingDeliveredAsync(DateTime dateTime)
        {
            return _unitOfWork.OrderRepository.Get(
                    filter: u => u.Status.Equals("Đang Giao")
                                && u.StartDate.Equals(DateOnly.FromDateTime(dateTime))
                    ).Count();
        }

        public async Task<int> CountDailyOrdersDeliveredAsync(DateTime dateTime)
        {
            return _unitOfWork.OrderRepository.Get(
                    filter: u => u.Status.Equals("Đã Giao")
                                && u.StartDate.Equals(DateOnly.FromDateTime(dateTime))
                    ).Count();
        }

        public async Task<int> CountOrdersBeingDeliveredAsync()
        {
            return _unitOfWork.OrderRepository.Get(
                    filter: u => u.Status.Equals("Đang Giao")
                    ).Count();
        }

        public async Task<int> CountOrdersDeliveredAsync()
        {
            return _unitOfWork.OrderRepository.Get(
                    filter: u => u.Status.Equals("Đã Giao")
                    ).Count();
        }

        public async Task<int> CountTotalOrdersAsync()
        {
            return _unitOfWork.OrderRepository.Get().Count();
        }

        public async Task<IEnumerable<Order>> GetOrdersBeingDeliveredAsync()
        {
            return _unitOfWork.OrderRepository.Get(
                    filter: u => u.Status.Equals("Đang Giao")
                    ).ToList();
        }

        public async Task<IEnumerable<Order>> GetOrdersDeliveredAsync()
        {
            return _unitOfWork.OrderRepository.Get(
                    filter: u => u.Status.Equals("Đã Giao")
                    ).ToList();
        }

        public async Task<IEnumerable<Order>> GetTotalOrdersAsync()
        {
            return _unitOfWork.OrderRepository.Get().ToList();
        }
    }
}

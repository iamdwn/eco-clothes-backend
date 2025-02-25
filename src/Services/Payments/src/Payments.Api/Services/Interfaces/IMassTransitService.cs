﻿namespace Payments.Api.Services.Interfaces
{
    public interface IMassTransitService
    {
        Task Send<T>(T message, string queueName) where T : class;
        Task Publish<T>(T message) where T : class;
    }
}

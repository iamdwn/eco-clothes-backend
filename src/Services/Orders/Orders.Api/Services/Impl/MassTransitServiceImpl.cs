﻿using MassTransit;

namespace Orders.Api.Services.Impl
{
    public class MassTransitServiceImpl : IMassTransitService
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitServiceImpl(ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Send<T>(T message, string queueName) where T : class
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));

            await sendEndpoint.Send<T>(message);
        }

        public async Task Publish<T>(T message) where T : class
        {
            await _publishEndpoint.Publish<T>(message);
        }
    }
}

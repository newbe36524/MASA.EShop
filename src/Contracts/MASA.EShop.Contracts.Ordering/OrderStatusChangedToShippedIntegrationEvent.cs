﻿using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;

namespace MASA.EShop.Contracts.Ordering
{
    public record OrderStatusChangedToShippedIntegrationEvent(Guid OrderId, string OrderStatus,
            string Description, string BuyerName) : IntegrationEvent
    {
        public override string Topic { get; set; } = nameof(OrderStatusChangedToShippedIntegrationEvent);
    }
}

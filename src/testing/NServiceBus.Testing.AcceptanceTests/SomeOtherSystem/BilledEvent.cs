namespace TestNServiceBus.Testing.IntegrationTests.SomeOtherSystem
{
    using System;

    public interface BilledEvent
    {
        Guid OrderId { get; set; }
        string Input { get; set; }
    }
}
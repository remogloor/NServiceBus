namespace TestNServiceBus.Testing.IntegrationTests.SystemUnderTest
{
    using System;

    public interface OrderResponse
    {
        Guid OrderId { get; set; }
        string Input { get; set; }
    }
}
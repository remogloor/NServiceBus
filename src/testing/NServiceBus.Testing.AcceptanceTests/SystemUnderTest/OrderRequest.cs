namespace TestNServiceBus.Testing.IntegrationTests.SystemUnderTest
{
    using System;

    public interface OrderRequest
    {
        Guid OrderId { get; set; }
        string Input { get; set; }
    }
}
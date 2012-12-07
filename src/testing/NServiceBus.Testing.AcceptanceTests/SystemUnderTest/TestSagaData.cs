namespace TestNServiceBus.Testing.IntegrationTests.SystemUnderTest
{
    using System;

    using NServiceBus.Saga;

    public class TestSagaData : ISagaEntity
    {
        public Guid Id { get; set; }

        public string Originator { get; set; }

        public string OriginalMessageId { get; set; }

        public string Data { get; set; }
    }
}
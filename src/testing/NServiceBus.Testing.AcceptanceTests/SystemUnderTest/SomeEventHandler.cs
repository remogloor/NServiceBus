namespace TestNServiceBus.Testing.IntegrationTests.SystemUnderTest
{
    using NServiceBus;

    using TestNServiceBus.Testing.IntegrationTests.SomeOtherSystem;

    public class SomeEventHandler : IHandleMessages<SomeEvent>
    {
        private readonly IBus bus;

        public SomeEventHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(SomeEvent message)
        {
            this.bus.Publish<SomeOtherEvent>(m => { m.Input = message.Input; });
        }
    }
}
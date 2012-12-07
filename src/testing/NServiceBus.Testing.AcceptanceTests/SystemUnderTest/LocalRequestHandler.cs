namespace TestNServiceBus.Testing.IntegrationTests.SystemUnderTest
{
    using NServiceBus;

    public class LocalRequestHandler : IHandleMessages<LocalRequest>
    {
        private readonly IBus bus;

        public LocalRequestHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(LocalRequest message)
        {
            this.bus.Publish<SomeOtherEvent>(m => { m.Input = message.Input; });
        }
    }
}
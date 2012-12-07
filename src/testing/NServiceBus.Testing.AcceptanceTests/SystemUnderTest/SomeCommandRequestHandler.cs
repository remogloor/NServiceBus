namespace TestNServiceBus.Testing.IntegrationTests.SystemUnderTest
{
    using NServiceBus;

    public class SomeCommandRequestHandler : IHandleMessages<SomeCommandRequest>
    {
        private readonly IBus bus;

        public SomeCommandRequestHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(SomeCommandRequest message)
        {
            this.bus.Reply<SomeCommandResponse>(m => { m.Input = message.Input; });
        }
    }
}
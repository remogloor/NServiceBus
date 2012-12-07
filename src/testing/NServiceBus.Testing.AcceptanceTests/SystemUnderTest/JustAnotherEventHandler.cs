namespace TestNServiceBus.Testing.IntegrationTests.SystemUnderTest
{
    using NServiceBus;

    using TestNServiceBus.Testing.IntegrationTests.SomeOtherSystem;

    public class JustAnotherEventHandler : IHandleMessages<JustAnotherEvent>
    {
        private readonly IBus bus;

        public JustAnotherEventHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(JustAnotherEvent message)
        {
            this.bus.Send<LocalRequest>(m => { m.Input = message.Input; });
        }
    }
}
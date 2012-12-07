namespace TestNServiceBus.Testing.IntegrationTests.SystemUnderTest
{
    using NServiceBus;
    using NServiceBus.Saga;

    using TestNServiceBus.Testing.IntegrationTests.SomeOtherSystem;

    public class TestSaga : Saga<TestSagaData>, 
                            IAmStartedByMessages<OrderRequest>,
                            IHandleMessages<BilledEvent>
    {
        public override void ConfigureHowToFindSaga()
        {
            this.ConfigureMapping<BilledEvent>(s => s.Id, m => m.OrderId);
        }

        public void Handle(OrderRequest message)
        {
            this.Data.Id = message.OrderId;
            this.Data.Data = message.Input;
        }

        public void Handle(BilledEvent message)
        {
            this.ReplyToOriginator<OrderResponse>(
                m =>
                    {
                        m.Input = this.Data.Data + " " + message.Input;
                        m.OrderId = this.Data.Id;
                    });
            this.MarkAsComplete();
        }
    }
}
namespace TestNServiceBus.Testing.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using NServiceBus;
    using NServiceBus.Testing.IntegrationTesting;

    using NUnit.Framework;

    using TestNServiceBus.Testing.IntegrationTests.SomeOtherSystem;
    using TestNServiceBus.Testing.IntegrationTests.SystemUnderTest;

    [TestFixture]
    public class SampleIntegrationTest
    {

        [TestFixtureSetUp]
        public void SetUp()
        {
            TestBus.Configure(
                () =>
                Configure.With(GetAllAssembliesButOtherTestsAndCompatibility())
                         .DefineEndpointName("SystemUnderTest")
                         .DefaultBuilder()
                         .XmlSerializer(),
                cfg =>
                    cfg.DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("TestNServiceBus.Testing.IntegrationTests") && t.Name.EndsWith("Event"))
                       .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("TestNServiceBus.Testing.IntegrationTests") && t.Name.EndsWith("Request"))
                       .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("TestNServiceBus.Testing.IntegrationTests") && t.Name.EndsWith("Response")));
        }

        [Test]
        public void RequestResponse()
        {
           var bus = new TestBus();

           bus.Bus.Send<SomeCommandRequest>(m => { m.Input = "SomeData"; });

           bus.Verify().Send<SomeCommandResponse>(m => { m.Input = "SomeData"; });
        }

        [Test]
        public void PublishSubscribe()
        {
            var bus = new TestBus();

            bus.Bus.Publish<SomeEvent>(m => { m.Input = "SomeData"; });

            bus.Verify().Publish<SomeOtherEvent>(m => { m.Input = "SomeData"; });
        }

        [Test]
        public void SendLocal()
        {
            var bus = new TestBus();

            bus.Bus.Publish<JustAnotherEvent>(m => { m.Input = "SomeData"; });

            bus.Verify().Publish<SomeOtherEvent>(m => { m.Input = "SomeData"; });
        }
        
        [Test]
        public void Sagas()
        {
            var bus = new TestBus();

            var orderId = Guid.NewGuid();
            bus.Bus.Send<OrderRequest>(m => { m.Input = "Hello"; m.OrderId = orderId; });
            bus.Bus.Publish<BilledEvent>(m => { m.Input = "World"; m.OrderId = orderId; });

            bus.Verify().Send<OrderResponse>(
                m =>
                    {
                        m.Input = "Hello World";
                        m.OrderId = orderId;
                    });
        }

        private static IEnumerable<Assembly> GetAllAssembliesButOtherTestsAndCompatibility()
        {
            var allAssemblies = AllAssemblies.Except("NServiceBus.Compatibility");

            // Exclude all other Tests and Compatibility because they comtain interfering message mutators
            return allAssemblies.Where(a => !a.GetName().Name.StartsWith("NServiceBus") || !a.GetName().Name.EndsWith("Tests") || a == Assembly.GetExecutingAssembly());
        }
    }
}

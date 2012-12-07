namespace NServiceBus.Testing.IntegrationTesting
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using NServiceBus.Logging;
    using NServiceBus.Logging.Loggers;
    using NServiceBus.Unicast;

    public class TestBus
    {
        private static TestBusImplementation testBusImplementation;

        public TestBus()
        {
            testBusImplementation.receivedMessageStorage.Clear();
        }

        public IBus Bus
        {
            get
            {
                return testBusImplementation.Bus;
            }
        }

        public IBus Verify()
        {
            return testBusImplementation.Verify();
        }

        public static void Configure(Func<Configure> configure, Func<Configure, Configure> configureConventions)
        {
            testBusImplementation = new TestBusImplementation(configure, configureConventions);
        }
    }

    public class TestBusImplementation
    {
        public const string MessageSinkName = "Sink";
        private readonly TestConfigurationSource testConfigurationSource = new TestConfigurationSource();
        internal readonly ReceivedMessageStorage receivedMessageStorage = new ReceivedMessageStorage();

        private readonly CommunicationVerifyer communicationVerifyer;
        private readonly AppDomain testSandboxAppDomain;
        private readonly Sandbox testSandbox;

        public IBus Bus { get; private set; }

        public TestBusImplementation(Func<Configure> configure, Func<Configure, Configure> configureConventions)
        {
            var stopwatch = new Stopwatch();
            this.testSandboxAppDomain = AppDomain.CreateDomain(
                "TestingSandbox", 
                AppDomain.CurrentDomain.Evidence.Clone(),
                AppDomain.CurrentDomain.SetupInformation);
            this.testSandbox = (Sandbox)this.testSandboxAppDomain.CreateInstanceAndUnwrap(typeof(Sandbox).Assembly.FullName, typeof(Sandbox).FullName);
            this.testSandbox.Init(configure, configureConventions);

            stopwatch.Restart();
            LogManager.LoggerFactory = new NullLoggerFactory(); 
            Configure.DefineEndpointVersionRetriever = () => "1.0.0.0";

            var types = this.testSandbox.Types.Where(t => !this.IsMessageHandler(t))
                .Union(Assembly.GetExecutingAssembly().GetTypes()).ToList();
            var startablebus =
                Configure.With(types)
                .DefineEndpointName("Sink")
                .WithTestingConventions(configureConventions)
                .CustomConfigurationSource(testConfigurationSource)
                .DefaultBuilder()
                .XmlSerializer()
                .InMemorySubscriptionStorage()
                .InMemoryFaultManagement()
                .InMemorySagaPersister()
                .UseInMemoryGatewayPersister()
                .UseInMemoryTimeoutPersister()
                .WithTestingTransport(this.testSandbox.ReceiverList, receivedMessageStorage)
                .UnicastBus().DoNotAutoSubscribe()
                .CreateBus();

            this.Bus = startablebus.Start();
            this.testSandbox.Start();

            foreach (var type in types.Where(MessageConventionExtensions.IsEventType))
            {
                ((UnicastBus)this.Bus).RegisterMessageType(type, this.testSandbox.Address);
                this.Bus.Subscribe(type);
            }

            this.communicationVerifyer = new CommunicationVerifyer(this.receivedMessageStorage, ((UnicastBus)this.Bus).MessageMapper);
        }

        private bool IsMessageHandler(Type type)
        {
            return
                (type.Namespace == null || !type.Namespace.StartsWith("NServiceBus")) &&
                type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMessageHandler<>));
        }

        public IBus Verify()
        {
            return communicationVerifyer;
        }
    }

    public class Sandbox : MarshalByRefObject
    {
        private readonly TestConfigurationSource testConfigurationSource = new TestConfigurationSource();
        private IBus bus;
        private IStartableBus startableBus;

        public Sandbox()
        {
            this.ReceiverList = new ReceiverList();
        }

        public ReceiverList ReceiverList { get; private set; }

        public void Init(Func<Configure> configure, Func<Configure, Configure> configureConventions)
        {
            Configure.DefineEndpointVersionRetriever = () => "1.0.0.0";
            this.startableBus = configure()
                .WithTestingConventions(configureConventions)
                .CustomConfigurationSource(testConfigurationSource)
                .InMemorySubscriptionStorage()
                .InMemoryFaultManagement()
                .InMemorySagaPersister().Sagas()
                .UseInMemoryGatewayPersister()
                .UseInMemoryTimeoutPersister()
                .WithTestingTransport(this.ReceiverList)
                .UnicastBus()
                .CreateBus();
        }

        public void Start()
        {
            this.bus = this.startableBus.Start();
        }

        public Address Address
        {
            get
            {
                return Address.Local;
            }
        }

        public Type[] Types
        {
            get
            {
                return Configure.TypesToScan.ToArray();
            }
        }
    }
}
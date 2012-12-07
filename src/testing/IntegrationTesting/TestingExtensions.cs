namespace NServiceBus.Testing.IntegrationTesting
{
    using System;

    public static class TestingExtensions
    {
        public static Configure WithTestingTransport(this Configure configurer, ReceiverList receiverList)
        {
            configurer.Configurer.ConfigureComponent<Receiver>(DependencyLifecycle.InstancePerCall);
            configurer.Configurer.ConfigureComponent<Sender>(DependencyLifecycle.InstancePerCall);
            configurer.Configurer.ConfigureComponent(() => receiverList, DependencyLifecycle.SingleInstance);
            return configurer;
        }

        public static Configure WithTestingTransport(this Configure configurer, ReceiverList receiverList, IReceivedMessageStorage receivedMessageStorage)
        {
            configurer.WithTestingTransport(receiverList);
            configurer.Configurer.ConfigureComponent(() => receivedMessageStorage, DependencyLifecycle.SingleInstance);
            return configurer;
        }

        public static Configure WithTestingConventions(this Configure configurer, Func<Configure, Configure> configureConvention)
        {
            configureConvention(configurer);
            return configurer;
        }
    }
}
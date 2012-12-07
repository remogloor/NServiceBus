namespace NServiceBus.Testing.IntegrationTesting
{
    using System.Collections.Generic;

    public interface IReceiverList
    {
        Receiver Sink { get; }

        void SetSink(Receiver receiver);

        bool TryGetValue(Address address, out Receiver receiver);

        void Add(Address address, Receiver receiver);
    }
}
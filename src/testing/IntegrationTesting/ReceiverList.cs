namespace NServiceBus.Testing.IntegrationTesting
{
    using System;
    using System.Collections.Generic;

    public class ReceiverList : MarshalByRefObject, IReceiverList
    {
        private IDictionary<Address, Receiver> receiverList = new Dictionary<Address, Receiver>();

        public void SetSink(Receiver receiver)
        {
            this.Sink = receiver;
        }

        public bool TryGetValue(Address address, out Receiver receiver)
        {
            return this.receiverList.TryGetValue(address, out receiver);
        }

        public void Add(Address address, Receiver receiver)
        {
            this.receiverList.Add(address, receiver);
        }

        public Receiver Sink { get; private set; }
    }
}
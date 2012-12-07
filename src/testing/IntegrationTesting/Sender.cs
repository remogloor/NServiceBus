namespace NServiceBus.Testing.IntegrationTesting
{
    using System;

    using NServiceBus.Unicast.Queuing;

    public class Sender : ISendMessages
    {
        private readonly IReceiverList receiverList;

        public Sender(IReceiverList receiverList)
        {
            this.receiverList = receiverList;
        }

        public void Send(TransportMessage message, Address address)
        {
            message.Id = Guid.NewGuid().ToString();

            Receiver receiver;
            if (this.receiverList.TryGetValue(address, out receiver))
            {            
                receiver.Send(message);
            }
            else
            {
                this.receiverList.Sink.Send(message);
            }
        }
    }
}
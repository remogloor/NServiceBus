namespace NServiceBus.Testing.IntegrationTesting
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using NServiceBus.Unicast.Transport.Transactional;

    public class Receiver : MarshalByRefObject, IDequeueMessages
    {
        private readonly IReceiverList receiverList;
        private string name;

        public Receiver(IReceiverList receiverList)
        {
            this.receiverList = receiverList;
            this.name = Configure.GetEndpointNameAction();
        }

        public void Init(Address address, TransactionSettings transactionSettings)
        {
            if (this.name != TestBusImplementation.MessageSinkName)
            {
                this.receiverList.Add(address, this);
            }
            else {
                if (address.Queue == "sink")
                {
                    this.receiverList.SetSink(this);
                }}
        }

        public void Start(int maxDegreeOfParallelism)
        {
        }

        public void ChangeMaxDegreeOfParallelism(int value)
        {
        }

        public void Stop()
        {
        }

        public event EventHandler<TransportMessageAvailableEventArgs> MessageDequeued;

        public void Send(TransportMessage message)
        {
            var waitForCompletion = new ManualResetEvent(false);
            Task.Factory.StartNew(
                () =>
                    {
                        this.MessageDequeued(this, new TransportMessageAvailableEventArgs(message));
                        waitForCompletion.Set();
                    });
            waitForCompletion.WaitOne();
        }
    }
}
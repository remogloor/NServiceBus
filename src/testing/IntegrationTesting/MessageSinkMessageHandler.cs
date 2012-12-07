namespace NServiceBus.Testing.IntegrationTesting
{
    public class MessageSinkMessageHandler : IHandleMessages<object>
    {
        private readonly IReceivedMessageStorage receivedMessageStorage;

        public MessageSinkMessageHandler()
        {
        }

        public MessageSinkMessageHandler(IReceivedMessageStorage receivedMessageStorage)
        {
            this.receivedMessageStorage = receivedMessageStorage;
        }

        public void Handle(object message)
        {
            if (this.receivedMessageStorage != null)
                this.receivedMessageStorage.Add(message);
        }
    }
}
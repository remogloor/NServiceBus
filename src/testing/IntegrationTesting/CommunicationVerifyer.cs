namespace NServiceBus.Testing.IntegrationTesting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using NServiceBus.MessageInterfaces;

    public class CommunicationVerifyer : IBus
    {
        private readonly ReceivedMessageStorage receivedMessageStorage;
        private readonly IMessageMapper messageMapper;

        public CommunicationVerifyer(ReceivedMessageStorage receivedMessageStorage, IMessageMapper messageMapper)
        {
            this.receivedMessageStorage = receivedMessageStorage;
            this.messageMapper = messageMapper;
        }

        public T CreateInstance<T>()
        {
            throw new NotImplementedException();
        }

        public T CreateInstance<T>(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public object CreateInstance(Type messageType)
        {
            throw new NotImplementedException();
        }

        public void Publish<T>(params T[] messages)
        {
            throw new NotImplementedException();
        }

        public void Publish<T>(Action<T> messageConstructor)
        {
            var message = messageMapper.CreateInstance<T>();
            messageConstructor(message);

            this.receivedMessageStorage.Should().Contain(m => this.IsEqualMessage(message, m));
        }

        public void Subscribe(Type messageType)
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T>()
        {
            throw new NotImplementedException();
        }

        public void Subscribe(Type messageType, Predicate<object> condition)
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T>(Predicate<T> condition)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(Type messageType)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<T>()
        {
            throw new NotImplementedException();
        }

        public ICallback SendLocal(params object[] messages)
        {
            throw new NotImplementedException();
        }

        public ICallback SendLocal<T>(Action<T> messageConstructor)
        {
            throw new NotImplementedException();
        }

        public ICallback Send(params object[] messages)
        {
            throw new NotImplementedException();
        }

        public ICallback Send<T>(Action<T> messageConstructor)
        {
            var message = messageMapper.CreateInstance<T>();
            messageConstructor(message);

            this.receivedMessageStorage.Should().Contain(m => this.IsEqualMessage(message, m));
            return null;
        }

        public ICallback Send(string destination, params object[] messages)
        {
            throw new NotImplementedException();
        }

        public ICallback Send(Address address, params object[] messages)
        {
            throw new NotImplementedException();
        }

        public ICallback Send<T>(string destination, Action<T> messageConstructor)
        {
            throw new NotImplementedException();
        }

        public ICallback Send<T>(Address address, Action<T> messageConstructor)
        {
            throw new NotImplementedException();
        }

        public ICallback Send(string destination, string correlationId, params object[] messages)
        {
            throw new NotImplementedException();
        }

        public ICallback Send(Address address, string correlationId, params object[] messages)
        {
            throw new NotImplementedException();
        }

        public ICallback Send<T>(string destination, string correlationId, Action<T> messageConstructor)
        {
            throw new NotImplementedException();
        }

        public ICallback Send<T>(Address address, string correlationId, Action<T> messageConstructor)
        {
            throw new NotImplementedException();
        }

        public ICallback SendToSites(IEnumerable<string> siteKeys, params object[] messages)
        {
            throw new NotImplementedException();
        }

        public ICallback Defer(TimeSpan delay, params object[] messages)
        {
            throw new NotImplementedException();
        }

        public ICallback Defer(DateTime processAt, params object[] messages)
        {
            throw new NotImplementedException();
        }

        public void Reply(params object[] messages)
        {
            throw new NotImplementedException();
        }

        public void Reply<T>(Action<T> messageConstructor)
        {
            throw new NotImplementedException();
        }

        public void Return<T>(T errorEnum)
        {
            throw new NotImplementedException();
        }

        public void HandleCurrentMessageLater()
        {
            throw new NotImplementedException();
        }

        public void ForwardCurrentMessageTo(string destination)
        {
            throw new NotImplementedException();
        }

        public void DoNotContinueDispatchingCurrentMessageToHandlers()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> OutgoingHeaders { get; private set; }

        public IMessageContext CurrentMessageContext { get; private set; }

        public IInMemoryOperations InMemory { get; private set; }

        private bool IsEqualMessage<T>(T message, object m)
        {
            if (!m.GetType().Implements<T>())
            {
                return false;
            }

            try
            {
                m.ShouldHave().AllRuntimeProperties().EqualTo(message);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
﻿using JustBehave;
using JustSaying.Messaging.MessageSerialisation;
using JustSaying.Models;
using JustSaying.TestingFramework;
using NUnit.Framework;

namespace JustSaying.Messaging.UnitTests.Serialisation.Newtonsoft
{
    public class WhenSerialisingAndDeserialising : BehaviourTest<NewtonsoftSerialiser<MessageWithEnum>>
    {
        private MessageWithEnum _messageOut;
        private MessageWithEnum _messageIn;
        private string _jsonMessage;

        protected override void Given()
        {
            _messageOut = new MessageWithEnum(Values.Two);
        }

        protected override void When()
        {
            _jsonMessage = SystemUnderTest.Serialise(_messageOut);
            _messageIn = SystemUnderTest.Deserialise(_jsonMessage) as MessageWithEnum;
        }

        [Then]
        public void MessageHasBeenCreated()
        {
            Assert.NotNull(_messageOut);
        }

        [Then]
        public void MessagesContainSameDetails()
        {
            Assert.AreEqual(_messageIn.RaisingComponent, _messageOut.RaisingComponent);
            //Assert.AreEqual(_messageIn.TimeStamp, _messageOut.TimeStamp);
            // ToDo: Sort timestamp issue!
        }
        
        [Then]
        public void EnumsAreRepresentedAsStrings()
        {
            Assert.That(_jsonMessage.Contains("EnumVal"));
            Assert.That(_jsonMessage.Contains("Two"));
        }
    }

    public class WhenAskingForAnewSerialiser : BehaviourTest<NewtonsoftSerialisationFactory>
    {
        private IMessageSerialiser<Message> _result;

        protected override void Given()
        {
            
        }

        protected override void When()
        {
            _result = SystemUnderTest.GetSerialiser<GenericMessage>();
        }

        [Then]
        public void OneIsProvided()
        {
            Assert.NotNull(_result);
        }
    }
}

using Amazon;
using JustEat.AwsTools;
using SimplesNotificationStack.AwsTools;
using SimplesNotificationStack.Messaging;
using SimplesNotificationStack.Messaging.MessageHandling;
using SimplesNotificationStack.Messaging.MessageSerialisation;
using SimplesNotificationStack.Messaging.Messages;

namespace SimplesNotificationStack.Stack
{
    /// <summary>
    /// This is not the perfect shining example of a fluent API YET!
    /// Intended usage:
    /// 1. Call Register()
    /// 2. Set subscribers - WithSqsTopicSubscriber() / WithSnsTopicSubscriber() etc
    /// 3. Set Handlers - WithTopicMessageHandler()
    /// </summary>
    public class FluentNotificationStack
    {
        private static NotificationStack _instance;
        private static readonly IMessageSerialisationRegister SerialisationRegister = new ReflectedMessageSerialisationRegister();

        private FluentNotificationStack(NotificationStack notificationStack)
        {
            _instance = notificationStack;
        }

        /// <summary>
        /// Create a new notification stack registration.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static FluentNotificationStack Register(Component component)
        {
            return new FluentNotificationStack(new NotificationStack(component));
        }

        /// <summary>
        /// Subscribe to a topic using SQS.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public FluentNotificationStack WithSqsTopicSubscriber(NotificationTopic topic)
        {
            var endpoint = new Messaging.Lookups.SqsSubscribtionEndpointProvider().GetLocationEndpoint(_instance.Component, topic);
            var queue = new SqsQueueByUrl(endpoint, AWSClientFactory.CreateAmazonSQSClient(RegionEndpoint.EUWest1));
            var sqsSub = new SqsNotificationListener(queue, SerialisationRegister);
            _instance.AddNotificationTopicSubscriber(topic, sqsSub);
            return this;
        }

        /// <summary>
        /// Set message handlers for the given topic
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public FluentNotificationStack WithTopicMessageHandler(NotificationTopic topic, IHandler<Message> handler)
        {
            _instance.AddMessageHandler(topic, handler);
            return this;
        }

        /// <summary>
        /// I'm done setting up. Fire this baby up...
        /// </summary>
        public void StartListening()
        {
            _instance.Start();
        }
    }
}
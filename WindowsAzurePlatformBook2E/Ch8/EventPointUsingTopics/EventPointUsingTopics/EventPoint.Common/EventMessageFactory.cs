using System;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace EventPoint.Common
{
    public class EventMessageFactory
    {
        TopicDescription topicDescription;
        TopicClient eventTopic;
        NamespaceManager nsManager;
        MessagingFactory messageFactory;
        string topicName = "eventpoint";

        public EventMessageFactory()
        {
            // Create Namespace Manager and messaging factory
            Uri serviceAddress = ServiceBusEnvironment.CreateServiceUri("sb", Config.serviceNamespace, string.Empty);
            nsManager = new NamespaceManager(serviceAddress, TokenProvider.CreateSharedSecretTokenProvider(Config.issuerName, Config.issuerSecret));
            messageFactory = MessagingFactory.Create(serviceAddress, TokenProvider.CreateSharedSecretTokenProvider(Config.issuerName, Config.issuerSecret));

            // set up the topic with batched operations, and time to live of 10 minutes. Subscriptions will not delete the message, since multiple clients are accessing the message
            // it will expire on its own after 10 minutes.
            topicDescription = new TopicDescription(topicName) { DefaultMessageTimeToLive = TimeSpan.FromMinutes(10), Path = topicName, EnableBatchedOperations = true };
            if (!nsManager.TopicExists(topicDescription.Path))
                nsManager.CreateTopic(topicDescription);

            // create client
            eventTopic = messageFactory.CreateTopicClient(topicName);
        }

        public void SendEvent(EventMessage message)
        {
            BrokeredMessage brokeredMessage = new BrokeredMessage(message);
            eventTopic.Send(brokeredMessage);
        }

        public SubscriptionClient CreateSubscriptionClient(string subscriptionName)
        {
            // Use default ReceiveMode - PeekLock - we don't want to delete messages off of the queue until they expire
            return messageFactory.CreateSubscriptionClient(topicName, subscriptionName);
        }

        public SubscriptionDescription CreateSubscription(string subscriptionName)
        {
            // create the subscription if it doesn't exist
            if (!nsManager.SubscriptionExists(topicName, subscriptionName))
            {
                SubscriptionDescription description = new SubscriptionDescription(topicName, subscriptionName) { EnableBatchedOperations = true, Name = subscriptionName };
                return nsManager.CreateSubscription(description);
            }

            // Use default ReceiveMode - PeekLock - we don't want to delete messages off of the queue until they expire
            return nsManager.GetSubscription(topicName, subscriptionName);
        }

        /// <summary>
        /// Creates a subscription for a specific priority
        /// </summary>
        /// <param name="subscriptionName"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public SubscriptionDescription CreateSubscription(string subscriptionName, string priority)
        {
            return CreateSubscription(subscriptionName, new CorrelationFilter(priority));
        }

        /// <summary>
        /// Creates a subscription with a custom filter
        /// </summary>
        /// <param name="subscriptionName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public SubscriptionDescription CreateSubscription(string subscriptionName, Filter filter)
        {
            // create the subscription if it doesn't exist
            if (!nsManager.SubscriptionExists(topicName, subscriptionName))
            {
                SubscriptionDescription description = new SubscriptionDescription(topicName, subscriptionName) { EnableBatchedOperations = true, Name = subscriptionName };
                return nsManager.CreateSubscription(description, filter);
            }

            // Use default ReceiveMode - PeekLock - we don't want to delete messages off of the queue until they expire
            return nsManager.GetSubscription(topicName, subscriptionName);
        }
    }
}

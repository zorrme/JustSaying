using System;
using Amazon.SimpleNotificationService;

namespace JustSaying.AwsTools.MessageHandling
{
    public class LocalTopicArnProvider : ITopicArnProvider
    {
        private readonly IAmazonSimpleNotificationService _client;
        private readonly Lazy<string> _lazyGetArn;
        private bool _exists;

        public LocalTopicArnProvider(IAmazonSimpleNotificationService client, string topicName)
        {
            _client = client;

            _lazyGetArn = new Lazy<string>(() => GetArnInternal(topicName));
        }

        private string GetArnInternal(string topicName)
        {
            try
            {
                var arn = _client.FindTopic(topicName).TopicArn;
                _exists = true;
                return arn;
            }
            catch
            {
                // ignored
            }
            return null;
        }

        public string GetArn()
        {
            return _lazyGetArn.Value;
        }

        public bool ArnExists()
        {
            // ReSharper disable once UnusedVariable
            var ignored = _lazyGetArn.Value;
            return _exists;
        }
    }
}

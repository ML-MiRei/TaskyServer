using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProjectService.Application.Dtos;
using System.Net;

namespace ProjectService.Infrastructure.Kafka
{
    public class KafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private static ProducerConfig _config;
        private Queue<(string, MessageModel)> _messagesQueue { get; set; } = new Queue<(string, MessageModel)>();

        public KafkaProducer(ILogger<KafkaProducer> logger, IOptions<KafkaSettings> options)
        {
            _logger = logger;
            _config = new ProducerConfig
            {
                BootstrapServers = options.Value.BootstrapServers,
                EnableDeliveryReports = true,
                ClientId = Dns.GetHostName(),
                Debug = "msg",
                Acks = Acks.Leader,
                MessageSendMaxRetries = 3,
                RetryBackoffMs = 1000,
                EnableIdempotence = false
            };

            StartSenderAsync("projects-notifications");
        }

        public void SendMessage(string key, MessageModel value)
        {
            _messagesQueue.Enqueue((key, value));
        }

        public async void StartSenderAsync(string topic)
        {
            //using var producer = new ProducerBuilder<string, MessageModel>(_config)
            //    .SetKeySerializer(Serializers.Utf8)
            //    .SetLogHandler((_, message) =>
            //        _logger.LogDebug($"Facility: {message.Facility}-{message.Level} Message: {message.Message}"))
            //    .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}. Is Fatal: {e.IsFatal}"))
            //    .Build();

            //try
            //{
            //    _logger.LogInformation("\nProducer loop started...\n\n");
            //    while (true)
            //    {
            //        var result = _messagesQueue.TryDequeue(out var message);
            //        if (!result)                        
            //            continue;

            //        var deliveryReport = await producer.ProduceAsync(topic,
            //            new Message<string, MessageModel>
            //            {
            //                Key = message.Item1,
            //                Value = message.Item2
            //            });

            //        if (deliveryReport.Status != PersistenceStatus.Persisted)
            //        {
            //           _logger.LogError(
            //                $"ERROR: Message not ack'd by all brokers (key: '{message.Item1}'). Delivery status: {deliveryReport.Status}");
            //        }
            //    }
            //}
            //catch (ProduceException<string, MessageModel> e)
            //{
            //    _logger.LogError($"Permanent error: {e.Message} for message (value: '{e.DeliveryResult.Value}')");
            //    _logger.LogInformation("Exiting producer...");
            //}
        }
    }
}

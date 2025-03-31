using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Abstractions.Services;
using NotificationService.Application.NotificationFactories;

namespace NotificationService.Infrastructure.Kafka
{
    public class Consumer
    {
        private static ILogger<Consumer> _logger;
        private static NotificationsService _notificationsService;
        private readonly ConsumerConfig _config;

        public Consumer(ILogger<Consumer> logger, NotificationsService notificationsService ,string bootstrapServer)
        {
            _notificationsService = notificationsService;
            _logger = logger;
            _config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServer,
                EnableAutoCommit = false,
                EnableAutoOffsetStore = false,
                GroupId = "default",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            Thread thread = new Thread(() => StartReceivingMessagesAsync("tasks"));
        }

        public async Task StartReceivingMessagesAsync(string topicName)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_config)
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build();
            try
            {
                consumer.Subscribe(topicName);
                _logger.LogInformation("\nConsumer loop started...\n\n");
                while (true)
                {
                    var result = consumer.Consume();
                    var message = result?.Message?.Value;
                    if (message == null)
                    {
                        continue;
                    }

                    var notificationFactory = new TaskNotificationFactory();
                    var notification = notificationFactory.Create(new Core.Models.MessageTemplate(result.Key?.ToString(), message));

                    _notificationsService.Create(notification);

                    consumer.Commit(result);
                    consumer.StoreOffset(result);
                }
            }
            catch (KafkaException e)
            {
                _logger.LogError($"Consume error: {e.Message}");
                _logger.LogInformation("Exiting producer...");
            }
        }
    }
}
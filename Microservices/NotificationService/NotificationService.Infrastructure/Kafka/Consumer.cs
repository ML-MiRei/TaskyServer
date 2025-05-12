using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Abstractions.Services;
using NotificationService.Application.Dtos;
using NotificationService.Application.NotificationFactories;
using NotificationService.Core.Models;

namespace NotificationService.Infrastructure.Kafka
{
    public class Consumer
    {
        private static ILogger<Consumer> _logger;
        private static NotificationsService _notificationsService;
        private readonly ConsumerConfig _config;

        public Consumer(ILogger<Consumer> logger, NotificationsService notificationsService, IOptions<KafkaSettings> options)
        {
            _notificationsService = notificationsService;
            _logger = logger;
            _config = new ConsumerConfig
            {
                BootstrapServers = options.Value.BootstrapServers,
                EnableAutoCommit = false,
                EnableAutoOffsetStore = false,
                GroupId = "default",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var factory1 = new TaskNotificationFactory();
            var factory2 = new ProjectNotificationFactory();

            Thread thread1 = new Thread(() => StartReceivingMessagesAsync("tasks-notifications", factory1));
            Thread thread2 = new Thread(() => StartReceivingMessagesAsync("tasks-notifications", factory2));
        }

        public async Task StartReceivingMessagesAsync(string topic, NotificationFactory factory)
        {
            using var consumer = new ConsumerBuilder<string, MessageModel>(_config)
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build();
            try
            {
                consumer.Subscribe(topic);
                _logger.LogInformation("\nConsumer loop started...\n\n");
                while (true)
                {
                    var result = consumer.Consume();
                    var message = result?.Message;
                    if (message == null)
                    {
                        continue;
                    }

                    var notification = factory.Create(new MessageTemplate(message.Key, message.Value.Message), message.Value.ObjectId);
                    _notificationsService.Create(message.Value.UserIds, notification);

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
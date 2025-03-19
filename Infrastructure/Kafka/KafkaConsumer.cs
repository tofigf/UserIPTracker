using Confluent.Kafka;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace Infrastructure.Kafka
{
 

    public class KafkaConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly AsyncRetryPolicy _retryPolicy;

        public KafkaConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration, ILogger<KafkaConsumer> logger)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _logger = logger;

            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt * 2),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Kafka retry {retryCount} after {timeSpan.TotalSeconds} sec, Error: {exception.Message}");
                    });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = "user-connection-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe("user-connections");

            while (!stoppingToken.IsCancellationRequested)
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    var connectionEvent = JsonConvert.DeserializeObject<ConnectUserRequest>(consumeResult.Value);

                    using var scope = _scopeFactory.CreateScope();
                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    await userRepository.AddUserConnectionAsync(connectionEvent.UserId, connectionEvent.IpAddress);

                    _logger.LogInformation($"[KafkaConsumer] Stored connection for User {connectionEvent.UserId}, IP: {connectionEvent.IpAddress}");
                });
            }
        }
    }

}

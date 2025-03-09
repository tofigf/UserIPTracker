using Confluent.Kafka;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Infrastructure.Kafka
{
        public class KafkaConsumer : BackgroundService
        {
            private readonly IServiceScopeFactory _scopeFactory;
            private readonly IConfiguration _configuration;

            public KafkaConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
            {
                _scopeFactory = scopeFactory;
                _configuration = configuration;
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
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);
                        var connectionEvent = JsonConvert.DeserializeObject<ConnectUserRequest>(consumeResult.Message.Value);

                        if (connectionEvent != null)
                        {
                            using var scope = _scopeFactory.CreateScope();
                            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                            await userRepository.AddUserConnectionAsync(connectionEvent.UserId, connectionEvent.IpAddress);

                            Console.WriteLine($"[KafkaConsumer] Stored connection for User {connectionEvent.UserId}, IP: {connectionEvent.IpAddress}");
                        }
                        else
                        {
                            Console.WriteLine("[ERROR] Kafka Consumer: Deserialized connection event is null.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Kafka Consumer: {ex.Message}");
                    }
                }
            }
        }

    }

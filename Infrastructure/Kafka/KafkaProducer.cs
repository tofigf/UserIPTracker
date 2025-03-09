using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Kafka
{
    namespace UserIPTracker.Infrastructure.Kafka
    {
        public class KafkaProducer
        {
            private readonly IProducer<string, string> _producer;

            public KafkaProducer(IConfiguration configuration)
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = configuration["Kafka:BootstrapServers"]
                };
                _producer = new ProducerBuilder<string, string>(config).Build();
            }

            public async Task PublishUserConnectionAsync(long userId, string ipAddress)
            {
                var message = JsonConvert.SerializeObject(new { UserId = userId, IpAddress = ipAddress });
                await _producer.ProduceAsync("user-connections", new Message<string, string>
                {
                    Key = userId.ToString(),
                    Value = message
                });
            }
        }
    }
}

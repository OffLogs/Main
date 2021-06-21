using System.Net;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Services.Communication.Serializers;

namespace OffLogs.Business.Services.Communication
{
    public class KafkaService: IKafkaService
    {
        private readonly IConfiguration _configuration;
        private ProducerConfig _producerConfig;
        private string _producerId;
        
        private IProducer<Null, object> _producer;
        private IProducer<Null, object> Producer
        {
            get
            {
                if (_producer == null)
                {
                    var builder = new ProducerBuilder<Null, object>(_producerConfig);
                    builder.SetValueSerializer(new JsonSerializer<object>());
                    _producer = builder.Build();
                }

                return _producer;
            }
        }

        public KafkaService(IConfiguration configuration)
        {
            _configuration = configuration;
            _producerId = configuration.GetValue<string>("Kafka:ProducerId");
            
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration.GetValue<string>("Kafka:Servers"),
                ClientId = Dns.GetHostName()
            };
        }

        ~KafkaService()
        {
            _producer?.Dispose();
            _producer = null;
        }

        public async Task ProduceLogMessageAsync(LogEntity logEntity)
        {
            _producerConfig.ClientId = _producerId;

            await Producer.ProduceAsync("", new Message<Null, object>
            {
                Value = logEntity
            });
        }
    }
}
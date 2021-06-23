using System;
using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OffLogs.Business.Services.Kafka.Deserializers
{
    public class KeyDeserializer: IDeserializer<string>
    {
        private readonly ILogger _logger;

        public KeyDeserializer() {}

        public KeyDeserializer(ILogger logger)
        {
            _logger = logger;
        }
        
        public string Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (context.Component == MessageComponentType.Key)
            {
                try
                {
                    return Encoding.UTF8.GetString(data);
                }
                catch (Exception e)
                {
                    _logger?.LogError($"Kafka Key deserializer error: {e.Message}", e);
                    return default;
                }
            }
            return default;
        }
    }
}
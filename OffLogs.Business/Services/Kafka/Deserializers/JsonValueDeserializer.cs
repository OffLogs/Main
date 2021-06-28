using System;
using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OffLogs.Business.Services.Kafka.Deserializers
{
    public class JsonValueDeserializer<T>: IDeserializer<T>
    {
        private readonly ILogger _logger;

        public JsonValueDeserializer() {}

        public JsonValueDeserializer(ILogger logger)
        {
            _logger = logger;
        }

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return default;
            }

            if (context.Component == MessageComponentType.Key)
            {
                return default;
            }

            try
            {
                var valueString = Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject<T>(valueString);
            }
            catch (Exception e)
            {
                _logger?.LogError($"Kafka Json deserializer error: {e.Message}", e);
                return default;
            }
        }
    }
}
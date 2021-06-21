using System;
using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace OffLogs.Business.Services.Communication.Deserializers
{
    public class JsonDeserializer<T>: IDeserializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            try
            {
                if (context.Component == MessageComponentType.Key)
                {
                    if (data.GetType().IsPrimitive)
                    {
                        var stringKey = $"{data}";
                        if (!string.IsNullOrEmpty(stringKey))
                        {
                            return Encoding.UTF8.GetBytes($"{data}");
                        }
                        return null;
                    }
                }
                return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            }
            catch (Exception e)
            {
                throw new Exception($"Json converter error: {e.Message}", e);
            }
        }

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return default;
            }

            if (context.Component == MessageComponentType.Key)
            {
                // TODO: refactor it?
                return default;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
            }
            catch (Exception e)
            {
                throw new Exception($"Kafka Json deserializer error: {e.Message}", e);
            }
        }
    }
}
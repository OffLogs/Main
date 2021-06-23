using System;
using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace OffLogs.Business.Services.Kafka.Serializers
{
    public class ValueSerializer<T>: ISerializer<T>
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
                throw new Exception($"Kafka Json converter error: {e.Message}", e);
            }
        }
    }
}
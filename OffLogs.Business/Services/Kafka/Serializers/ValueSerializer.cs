using System;
using System.Text;
using Confluent.Kafka;
using OffLogs.Business.Helpers;

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
                    if (data is string or int or long or decimal or float)
                    {
                        var stringKey = $"{data}";
                        if (!string.IsNullOrEmpty(stringKey))
                        {
                            return Encoding.UTF8.GetBytes($"{data}");
                        }
                        return null;
                    }
                }
                return JsonHelper.SerializeToBytes(data);
            }
            catch (Exception e)
            {
                throw new Exception($"Kafka Json converter error: {e.Message}", e);
            }
        }
    }
}

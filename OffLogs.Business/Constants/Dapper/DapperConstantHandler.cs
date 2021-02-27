using System.Data;
using Dapper;

namespace OffLogs.Business.Constants.Dapper
{
    public class DapperConstantHandler<T> : SqlMapper.TypeHandler<T> where T : AConstant<T>, new()
    {
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = value.GetValue();
        }

        public override T Parse(object value)
        {
            return new T().FromString(value as string);
        }
    }
}
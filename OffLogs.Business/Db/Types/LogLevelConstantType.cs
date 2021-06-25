using OffLogs.Business.Constants;

namespace OffLogs.Business.Db.Types
{
    public class LogLevelConstantType : BaseConstantType<LogLevel>
    {
        public override LogLevel FromString(string value)
        {
            return new LogLevel().FromString(value);
        }
    }
}
using NHibernate;
using NHibernate.SqlCommand;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Orm.Connection.Interceptors
{
    public class SqlQueryInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Log.Debug($"NHibernate: {sql}");
            return base.OnPrepareStatement(sql);
        }
    }
}

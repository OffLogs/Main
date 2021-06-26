using System;
using System.Data;
using System.Data.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using OffLogs.Business.Constants;

namespace OffLogs.Business.Db.Types
{
    public abstract class BaseConstantType<T>: IUserType
    {
        public object DeepCopy(object value) => value;

        public object Replace(object original, object target, object owner) => original;

        public object Assemble(object cached, object owner) => cached;

        public object Disassemble(object value) => value;

        public SqlType[] SqlTypes
        {
            get => new[] { NHibernateUtil.String.SqlType }
        }

        public Type ReturnedType => typeof(AConstant<T>);

        public bool IsMutable => false;

        new public bool Equals(object x, object y) => object.Equals(x, y);

        public int GetHashCode(object x) => x.GetHashCode();

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var value = (string) rs[names[0]];

            if (string.IsNullOrEmpty(value))
                throw new Exception("Invalid value from DB");

            return FromString(value);
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var parameter = (IDataParameter) cmd.Parameters[index];
            parameter.Value = (value as AConstant<T>)?.GetValue();
        }

        public abstract T FromString(string value);
    }
}
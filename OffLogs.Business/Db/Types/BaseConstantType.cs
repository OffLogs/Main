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
        new public bool Equals(object x, object y)
        {
            return object.Equals(x, y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            object r = rs[names[0]];
            var value = (string) r;

            if (string.IsNullOrEmpty(value))
                throw new Exception("Invalid value from DB");

            return FromString(value);
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var parameter = (IDataParameter) cmd.Parameters[index];
            parameter.Value = (value as AConstant<T>)?.GetValue();
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public SqlType[] SqlTypes
        {
            get
            {
                return new SqlType[]
                {
                    SqlTypeFactory.GetString(10)
                };
            }
        }

        public Type ReturnedType
        {
            get { return typeof(CityCode); }
        }

        public bool IsMutable
        {
            get { return false; }
        }
        
        public abstract T FromString(string Value);
    }
}
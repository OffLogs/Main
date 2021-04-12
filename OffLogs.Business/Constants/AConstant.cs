using System;
using System.Data;
using OffLogs.Business.Common.Mvc.Attribute.Constant;
using ServiceStack.OrmLite.Converters;

namespace OffLogs.Business.Constants
{
    public abstract class AConstant<T> : StringConverter, IValidationAttribute
    {
        protected readonly string _Name;
        protected readonly string _Value;

        public AConstant() { }

        protected AConstant(string value, string name)
        {
            _Name = name;
            _Value = value;
        }

        public override string ToString()
        {
            return _Name;
        }

        public string GetValue()
        {
            return _Value;
        }

        public override bool Equals(object Value)
        {
            var status = Value as AConstant<T>;
            return status?.GetValue().Equals(_Value) ?? false;
        }

        public override int GetHashCode()
        {
            return _Value.GetHashCode();
        }

        #region OrmLite Converter

        public override void InitDbParam(IDbDataParameter p, Type fieldType)
        {
            p.DbType = DbType.String;
        }

        public override object ToDbValue(Type fieldType, object value)
        {
            var constantValue = (LogLevel)value;
            var stringValue = constantValue?.GetValue();
            return stringValue;
        }
        
        public override object FromDbValue(Type fieldType, object value)
        {
            var strValue = value as string; 
            return strValue != null
                ? new LogLevel().FromString($"{value}")
                : base.FromDbValue(fieldType, value);
        }

        #endregion
        
        public abstract bool IsValid(string Value);
        public abstract T FromString(string Value);
    }
}

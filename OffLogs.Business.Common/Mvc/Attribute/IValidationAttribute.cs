namespace OffLogs.Business.Common.Mvc.Attribute
{
    public interface IValidationAttribute
    {
        bool IsValid(string Value);
    }
}

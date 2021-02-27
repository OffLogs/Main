namespace OffLogs.Business.Mvc.Attribute.Constant
{
    public interface IValidationAttribute
    {
        bool IsValid(string Value);
    }
}

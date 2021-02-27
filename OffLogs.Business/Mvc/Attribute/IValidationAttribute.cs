namespace OffLogs.Business.Mvc.Attribute
{
    public interface IValidationAttribute
    {
        bool IsValid(string Value);
    }
}

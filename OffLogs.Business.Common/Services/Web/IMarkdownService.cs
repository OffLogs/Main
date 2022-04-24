using Domain.Abstractions;

namespace OffLogs.Business.Common.Services.Web;

public interface IMarkdownService: IDomainService
{
    string ToHtml(string markdown);
}

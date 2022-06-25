using System.Collections.Generic;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.UserEmails;

public record struct FetchListAction();

public record struct FetchListResultAction(
    ICollection<UserEmailDto> Items
);

using System.Collections.Generic;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.UserEmails;

public record struct FetchListAction(bool IsLoadIfEmpty = true);

public record struct SetIsLoadingAction(bool IsLoading);

public record struct FetchListResultAction(
    ICollection<UserEmailDto> Items
);

public record struct AddEmailToAddListItemAction();

public record struct RemoveEmailFromFromListAction(long Id = 0);

public record struct UpdateApplicationAction(UserEmailDto Application);

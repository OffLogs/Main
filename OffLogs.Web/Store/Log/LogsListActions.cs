using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Store.Log.Models;

namespace OffLogs.Web.Store.Log;

public record struct AddLogDetailsAction(LogDto Log);

public record struct FetchListResultAction(PaginatedListDto<LogListItemDto> Data);

public record struct FetchLogAction(long LogId);

public record struct FetchNextListPageAction();

public record struct ResetListAction();

public record struct ResetLogDetailsAction();

public record struct SelectLogAction(long? Id);

public record struct SetIsLogFavoriteAction(long LogId, bool IsFavorite);

public record struct SetApplication(long ApplicationId);

public record struct SetListFilterAction(LogFilterModel Filter);

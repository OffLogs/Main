﻿using System.Collections.Generic;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Web.Store.Application;

public record struct AddApplicationAction(ApplicationListItemDto Application);
public record struct UpdateApplicationAction(ApplicationDto Application);

public record struct DeleteApplicationAction(long Id);

public record struct FetchListResultAction(
    ICollection<ApplicationListItemDto> Items,
    int TotalPages,
    int PageSize,
    int TotalCount,
    bool IsHasMore
);

public record struct SetPaginationInfoAction(int SkipItems);

public record struct RemoveApplicationFromListAction(long Id);

public record struct ResetListAction;

public record struct SelectApplicationAction(long? ApplicationId);

public record struct FetchNextListPageAction(bool IsLoadIfEmpty);

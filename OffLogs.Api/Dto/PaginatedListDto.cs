using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OffLogs.Api.Dto
{
    public class PaginatedListDto<TItem>: IResponse
    {
        [JsonPropertyName("items")]
        public ICollection<TItem> Items { get; set; }

        [JsonPropertyName("totalPages")]
        public long TotalPages { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalCount")]
        public long TotalCount { get; set; }

        [JsonPropertyName("isHasMore")]
        public bool IsHasMore
        {
            get => PageSize <= Items.Count;
        }

        public PaginatedListDto()
        {
        }

        public PaginatedListDto(
            ICollection<TItem> responseList, 
            long totalItems = 0, 
            int pageSize = GlobalConstants.ListPageSize
        )
        {
            TotalCount = totalItems;
            Items = responseList;
            PageSize = pageSize;
            decimal totalPages = (decimal)totalItems / pageSize;
            TotalPages = (int)Math.Ceiling(totalPages);
        }
    }
}

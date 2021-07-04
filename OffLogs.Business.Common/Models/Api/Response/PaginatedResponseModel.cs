using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Business.Common.Models.Api.Response
{
    public record PaginatedResponseModel<T>
    {
        [JsonPropertyName("items")]
        public ICollection<T> Items { get; set; }
        
        [JsonPropertyName("totalPages")]
        public long TotalPages { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("isHasMore")]
        public bool IsHasMore
        {
            get => PageSize <= Items.Count;
        }

        public PaginatedResponseModel()
        {
        }

        public PaginatedResponseModel(ICollection<T> responseList, long totalItems, int pageSize = GlobalConstants.ListPageSize)
        {
            Items = responseList;
            PageSize = pageSize;
            decimal totalPages = (decimal)totalItems / pageSize;
            TotalPages = (int)Math.Ceiling(totalPages);
        }
    }
}
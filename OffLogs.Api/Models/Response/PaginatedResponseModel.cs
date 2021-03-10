using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using OffLogs.Business.Constants;

namespace OffLogs.Api.Models.Response
{
    public record PaginatedResponseModel<T>
    {
        [JsonPropertyName("items")]
        public ICollection<T> Items { get; set; }
        
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        public PaginatedResponseModel()
        {
        }

        public PaginatedResponseModel(ICollection<T> responseList, int totalItems, int pageSize = GlobalConstants.ListPageSize)
        {
            Items = responseList;
            PageSize = pageSize;
            decimal totalPages = (decimal)totalItems / pageSize;
            TotalPages = (int)Math.Ceiling(totalPages);
        }
    }
}

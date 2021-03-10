using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        
        public PaginatedResponseModel(ICollection<T> responseList, int totalItems, int pageSize = 20)
        {
            Items = responseList;
            PageSize = pageSize;
            decimal totalPages = totalItems / pageSize;
            TotalPages = (int)Math.Floor(totalPages);
        }
    }
}

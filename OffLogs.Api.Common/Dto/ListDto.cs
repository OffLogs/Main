using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Api.Common.Dto
{
    public class ListDto<TItem> : OffLogs.Business.Orm.Dto.ListDto<TItem>, IResponse
    {
        [JsonPropertyName("items")]
        public override ICollection<TItem> Items { get; set; }

        [JsonPropertyName("totalCount")]
        public override long TotalCount { get; set; }

        public ListDto()
        {
        }
        
        public ListDto(ICollection<TItem> items, long count) : base(items, count)
        {
        }
    }
}

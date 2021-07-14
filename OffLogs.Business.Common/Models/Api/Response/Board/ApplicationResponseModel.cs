using System;

namespace OffLogs.Business.Common.Models.Api.Response.Board
{
    public record ApplicationResponseModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        
        public string Name { get; set; }
        public string ApiToken { get; set; }
        public DateTime CreateTime { get; set; }

        public ApplicationResponseModel()
        {
        }
    }
}
using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses
{
    public class IdRequest : IRequest
    {
        [Required]
        [IsPositive(AllowZero = true)]
        public long Id { get; set; }

        public IdRequest()
        {
        }
        
        public IdRequest(long id)
        {
            Id = id;
        }
    }
}

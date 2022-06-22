using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Public.UserEmail
{
    public class VerificationRequest : IRequest
    {
        [Required]
        [StringLength(512)]
        public string Token { get; set; }
    }
}

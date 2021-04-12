using System.ComponentModel.DataAnnotations;

namespace OffLogs.Business.Common.Models.Api.Request.Board
{
    public record ApplicationUpdateModel: ApplicationAddModel
    {
        [Required]
        public long Id { get; set; }

        public ApplicationUpdateModel()
        {
        }
    }
}
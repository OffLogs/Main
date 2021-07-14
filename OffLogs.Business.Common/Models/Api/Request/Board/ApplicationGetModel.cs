using System.ComponentModel.DataAnnotations;

namespace OffLogs.Business.Common.Models.Api.Request.Board
{
    public record ApplicationGetModel
    {
        [Required]
        public long Id { get; set; }

        public ApplicationGetModel()
        {
        }
    }
}
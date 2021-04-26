using System.ComponentModel.DataAnnotations;

namespace OffLogs.Business.Common.Models.Api.Request.Board
{
    public record ApplicationAddModel
    {
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        public ApplicationAddModel()
        {
        }
    }
}
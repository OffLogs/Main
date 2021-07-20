using Api.Requests.Abstractions;
using OffLogs.Api.Dto.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Application.Actions
{
    public class AddRequest: IRequest<ApplicationDto>
    {
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }
    }
}

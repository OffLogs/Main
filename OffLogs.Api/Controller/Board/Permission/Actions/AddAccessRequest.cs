using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants.Permissions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Permission.Actions
{
    public class AddAccessRequest: IRequest
    {
        [Required]
        [EnumDataType(typeof(PermissionAccessType))]
        public PermissionAccessType AccessType { get; set; }

        public int RecepientId { get; set; }

        public int ItemId { get; set; }
    }
}

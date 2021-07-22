using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants.Permissions;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace Offlogs.Business.Api.Controller.Board.Permission.Actions
{
    public class AddAccessRequest: IRequest
    {
        /// <summary>
        /// Type of the access permission which will be assigned
        /// </summary>
        [Required]
        [EnumDataType(typeof(PermissionAccessType))]
        public PermissionAccessType AccessType { get; set; }

        /// <summary>
        /// Who will receive these permissions
        /// </summary>
        [IsPositive(AllowZero = true)]
        public long RecepientId { get; set; }

        /// <summary>
        /// Item to be granted permissions 
        /// </summary>
        [IsPositive(AllowZero = true)]
        public long ItemId { get; set; }
    }
}

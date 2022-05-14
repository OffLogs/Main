using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Permission;
using OffLogs.Business.Common.Constants.Permissions;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService
    {
        public async Task<bool> PermissionAddAccess(
            PermissionAccessType accessType, 
            long recipientId,
            long itemId
        )
        {
            await PostAuthorizedAsync(
                MainApiUrl.PermissionAddAccess, 
                new AddAccessRequest { 
                    AccessType = accessType,
                    RecipientId = recipientId,
                    ItemId = itemId
                }
            );
            return true;
        }
        
        public async Task<bool> PermissionRemoveAccess(
            PermissionAccessType accessType, 
            long recipientId,
            long itemId
        )
        {
            await PostAuthorizedAsync(
                MainApiUrl.PermissionRemoveAccess, 
                new RemoveAccessRequest { 
                    AccessType = accessType,
                    RecipientId = recipientId,
                    ItemId = itemId
                }
            );
            return true;
        }
    }
}

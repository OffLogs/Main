﻿using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;
using OffLogs.Api.Common.Requests.Board.Log;
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
            var response = await PostAuthorizedAsync<object>(
                MainApiUrl.PermissionAddAccess, 
                new AddAccessRequest { 
                    AccessType = accessType,
                    RecipientId = recipientId,
                    ItemId = itemId
                }
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return true;
        }
        
        public async Task<bool> PermissionRemoveAccess(
            PermissionAccessType accessType, 
            long recipientId,
            long itemId
        )
        {
            var response = await PostAuthorizedAsync<object>(
                MainApiUrl.PermissionRemoveAccess, 
                new RemoveAccessRequest { 
                    AccessType = accessType,
                    RecipientId = recipientId,
                    ItemId = itemId
                }
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return true;
        }
    }
}

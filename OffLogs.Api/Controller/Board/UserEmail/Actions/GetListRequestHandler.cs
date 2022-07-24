using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.UserEmail;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.User;
using OffLogs.Business.Services.Api;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Board.UserEmail.Actions
{
    public class GetListRequestHandler : IAsyncRequestHandler<GetListRequest, UserEmailsListDto>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IRequestService _requestService;

        public GetListRequestHandler(
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IRequestService requestService
        )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
        }

        public async Task<UserEmailsListDto> ExecuteAsync(GetListRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var user = await _queryBuilder.FindByIdAsync<UserEntity>(userId);
            if (user == null)
            {
                throw new RecordNotFoundException();
            }

            return _mapper.Map<UserEmailsListDto>(user.Emails);
        }
    }
}

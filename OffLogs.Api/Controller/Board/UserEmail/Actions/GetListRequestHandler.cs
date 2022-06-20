using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.User;
using OffLogs.Business.Services.Api;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Board.UserEmail.Actions
{
    public class GetListRequestHandler : IAsyncRequestHandler<SearchRequest, UsersListDto>
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

        public async Task<UsersListDto> ExecuteAsync(SearchRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var list = await _queryBuilder.For<ICollection<UserEntity>>()
                .WithAsync(new UserSearchCriteria(request.Search, new long[] { userId }));

            return _mapper.Map<UsersListDto>(list);
        }
    }
}

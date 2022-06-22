using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using Commands.Abstractions;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.UserEmail;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.UserEmail;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Board.UserEmail.Actions
{
    public class AddRequestHandler : IAsyncRequestHandler<AddRequest, UserEmailDto>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IRequestService _requestService;
        private readonly IDbSessionProvider _commitPerformer;
        private readonly IUserEmailService _userEmailService;

        public AddRequestHandler(
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder,
            IAsyncCommandBuilder commandBuilder,
            IRequestService requestService,
            IDbSessionProvider commitPerformer,
            IUserEmailService userEmailService
        )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _commandBuilder = commandBuilder;
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _commitPerformer = commitPerformer;
            _userEmailService = userEmailService;
        }

        public async Task<UserEmailDto> ExecuteAsync(AddRequest request)
        {
            var userId = _requestService.GetUserIdFromJwt();
            var user = await _queryBuilder.FindByIdAsync<UserEntity>(userId);
            if (user == null)
            {
                throw new RecordNotFoundException();
            }

            var userEmail = await _userEmailService.Add(user, request.Email);
            await _commitPerformer.PerformCommitAsync();
            
            return _mapper.Map<UserEmailDto>(userEmail);
        }
    }
}

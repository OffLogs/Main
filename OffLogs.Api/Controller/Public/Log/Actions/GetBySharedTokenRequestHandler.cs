using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AutoMapper;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.Log;
using OffLogs.Business.Exceptions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Public.Log.Actions
{
    public class GetBySharedTokenRequestHandler : IAsyncRequestHandler<GetBySharedTokenRequest, LogSharedDto>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncQueryBuilder _queryBuilder;

        public GetBySharedTokenRequestHandler(
            IMapper mapper,
            IAsyncQueryBuilder queryBuilder
        )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
        }

        public async Task<LogSharedDto> ExecuteAsync(GetBySharedTokenRequest request)
        {
            var log = await _queryBuilder.For<LogEntity>()
                .WithAsync(new GetByTokenCriteria(request.Token));
            if (log == null)
            {
                throw new ItemNotFoundException(nameof(log));
            }

            return _mapper.Map<LogSharedDto>(log);
        }
    }
}

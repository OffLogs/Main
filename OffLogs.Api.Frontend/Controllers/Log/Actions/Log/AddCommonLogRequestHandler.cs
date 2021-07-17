using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka;
using Queries.Abstractions;

namespace OffLogs.Api.Frontend.Controllers.Log.Actions.Log
{
    public class AddCommonLogRequestHandler : IAsyncRequestHandler<AddCommonLogsRequest>
    {
        private readonly IAsyncQueryBuilder _asyncQueryBuilder;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IJwtApplicationService _jwtApplicationService;

        public AddCommonLogRequestHandler(
            IAsyncQueryBuilder asyncQueryBuilder,
            IKafkaProducerService kafkaProducerService,
            IJwtApplicationService jwtApplicationService
        )
        {
            _asyncQueryBuilder = asyncQueryBuilder ?? throw new ArgumentNullException(nameof(asyncQueryBuilder));
            _kafkaProducerService = kafkaProducerService;
            _jwtApplicationService = jwtApplicationService;
        }

        public async Task ExecuteAsync(AddCommonLogsRequest request)
        {
            await Task.CompletedTask;
            
            // var animal = await _asyncQueryBuilder.FindByIdAsync<Animal>(request.Logs);
            //
            // var food = await _asyncQueryBuilder.FindByIdAsync<Food>(request.FoodId);
            //
            // await _feedingService.FeedAsync(animal, food, request.Count);
        }
    }
}
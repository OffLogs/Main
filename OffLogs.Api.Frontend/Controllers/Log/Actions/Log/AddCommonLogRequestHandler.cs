using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Queries.Abstractions;

namespace OffLogs.Api.Frontend.Controllers.Log.Actions.Log
{
    public class AddCommonLogRequestHandler : IAsyncRequestHandler<AddCommonLogsRequest>
    {
        private readonly IAsyncQueryBuilder _asyncQueryBuilder;

        public AddCommonLogRequestHandler(IAsyncQueryBuilder asyncQueryBuilder)
        {
            _asyncQueryBuilder = asyncQueryBuilder ?? throw new ArgumentNullException(nameof(asyncQueryBuilder));
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
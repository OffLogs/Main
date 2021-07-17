using Persistence.Transactions.Behaviors;

namespace AspNetCore.ApiControllers.Abstractions
{
    public interface IShouldPerformCommit
    {
        IExpectCommit CommitPerformer { get; }
    }
}
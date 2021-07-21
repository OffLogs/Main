using System;
using Autofac;

namespace Queries.Abstractions
{
    public class ScopedAsyncQueryFactory : IAsyncQueryFactory
    {
        private readonly ILifetimeScope _scope;

        public ScopedAsyncQueryFactory(ILifetimeScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));   
        }


        public IAsyncQuery<TCriterion, TResult> Create<TCriterion, TResult>() where TCriterion : ICriterion
        {
            return _scope.Resolve<IAsyncQuery<TCriterion, TResult>>();
        }
    }
}
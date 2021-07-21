using System;
using Autofac;

namespace Queries.Abstractions
{
    public class ScopedAsyncQueryBuilder : IAsyncQueryBuilder
    {
        private readonly ILifetimeScope _scope;

        public ScopedAsyncQueryBuilder(ILifetimeScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }


        public IAsyncQueryFor<TResult> For<TResult>()
        {
            return _scope.Resolve<IAsyncQueryFor<TResult>>();
        }
    }
}
using System;
using Autofac;

namespace Commands.Abstractions
{
    public class ScopedAsyncCommandFactory : IAsyncCommandFactory
    {
        private readonly ILifetimeScope _scope;

        public ScopedAsyncCommandFactory(ILifetimeScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public IAsyncCommand<TCommandContext> Create<TCommandContext>() where TCommandContext : ICommandContext
        {
            return _scope.Resolve<IAsyncCommand<TCommandContext>>();
        }
    }
}
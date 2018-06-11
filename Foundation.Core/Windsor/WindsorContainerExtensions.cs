using System;
using System.Transactions;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;

namespace Foundation.Windsor
{
    internal class WindsorTransactionScope : IDisposable
    {
        private readonly IDisposable _windsorScope;
        private readonly TransactionScope _trasactionScope;


        public WindsorTransactionScope(IWindsorContainer container)
        {
            _windsorScope = container.BeginScope();
            _trasactionScope = new TransactionScope();
        }


        public void Dispose()
        {
            _trasactionScope.Complete();
            _trasactionScope.Dispose();
            _windsorScope.Dispose();
        }
    }

    public static class WindsorContainerExtensions
    {
        public static IDisposable BeginTransactionScope(this IWindsorContainer container)
        {
            return new WindsorTransactionScope(container);
        }
    }
}

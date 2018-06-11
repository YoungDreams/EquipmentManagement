using Foundation.Core;
using NHibernate;
using TransactionException = System.Transactions.TransactionException;

namespace Foundation.Data.UnitOfWork
{
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        private ITransaction _transaction;
        private bool _isCommitted;

        public NHibernateUnitOfWork()
        {
            _transaction = ServiceLocator.Current.Resolve<ISession>().BeginTransaction();
        }

        public void Dispose()
        {
            if (!_isCommitted)
                throw new TransactionException("Did you forget commit the transaction?");
            _transaction.Dispose();
            _transaction = null;
        }

        public void Commit()
        {
            if (_isCommitted)
                throw new TransactionException("A transaction only can be submitted once!");
            _transaction.Commit();
            _isCommitted = true;
        }
    }
}

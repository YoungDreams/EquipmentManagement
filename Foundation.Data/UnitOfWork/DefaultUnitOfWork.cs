using System.Transactions;
using Foundation.Core;

namespace Foundation.Data.UnitOfWork
{
    public class DefaultUnitOfWork : IUnitOfWork
    {
        private TransactionScope _scope;
        private bool _isCommitted;

        public DefaultUnitOfWork()
        {
            _scope = new TransactionScope();
        }

        public void Dispose()
        {
            if (!_isCommitted)
                throw new TransactionException("Did you forget commit the transaction?");
            _scope.Dispose();
            _scope = null;
        }

        public void Commit()
        {
            if (_isCommitted)
                throw new TransactionException("A transaction only can be submitted once!");
            _scope.Complete();
            _isCommitted = true;
        }
    }
}

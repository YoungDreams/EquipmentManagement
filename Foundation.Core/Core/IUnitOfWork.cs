using System;

namespace Foundation.Core
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}

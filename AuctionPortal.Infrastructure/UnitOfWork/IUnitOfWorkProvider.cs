using System;

namespace AuctionPortal.Infrastructure.UnitOfWork
{
    public interface IUnitOfWorkProvider : IDisposable
    {
        IUnitOfWork Create();

        IUnitOfWork GetUnitOfWorkInstance();
    }
}

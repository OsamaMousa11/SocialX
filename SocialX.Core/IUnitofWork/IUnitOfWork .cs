using Microsoft.EntityFrameworkCore.Storage;
using SocialX.infrastraction.IRepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.IUnitofWork
{
    public interface IUnitOfWork: IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}

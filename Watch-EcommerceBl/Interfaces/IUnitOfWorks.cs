using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_EcommerceBl.Interfaces
{
    public interface IUnitOfWorks : IAsyncDisposable
    {
        //IGenericRepository<TEntity> Repository<TEntity, Tkey>() where TEntity : class;
        Task<int> CompleteAsync();

    }
}

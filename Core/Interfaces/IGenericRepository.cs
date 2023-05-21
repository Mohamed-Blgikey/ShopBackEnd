using Core.Entities;
using Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetWithSpecAsync(ISpecification<T> specification);
        Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> specification);
        Task<int> Count();
    }
}

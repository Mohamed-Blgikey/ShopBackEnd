using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUniteOdWork:IDisposable
    {
        IGenericRepository<T> repository<T>() where T : BaseEntity;
        Task<int> Complete();
    }
}

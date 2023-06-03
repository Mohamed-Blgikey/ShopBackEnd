using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Data
{
    public class UniteOdWork : IUniteOdWork
    {
        private readonly StoreContext context;
        private Hashtable Repositories;

        public UniteOdWork(StoreContext context)
        {
            this.context = context;
        }
        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await context.DisposeAsync();
        }

        public IGenericRepository<T> repository<T>() where T : BaseEntity
        {
            if (Repositories == null)
                Repositories = new Hashtable();
            var type = typeof(T).Name;
            if (!Repositories.ContainsKey(type))
            {
                var repType = typeof(GenericRepository<>);
                var repoinstance = Activator.CreateInstance(repType.MakeGenericType(typeof(T)),context);
                Repositories.Add(type,repoinstance);
               
            }
        
            return (IGenericRepository<T>)Repositories[type];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        T Add(T entity);
        T Update(T entity);
        void Remove(int id);
        IEnumerable<T> GetByName(string name);
        IEnumerable<T> GetRecent();
       
    }
}

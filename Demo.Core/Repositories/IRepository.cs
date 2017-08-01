using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> FindAll();
        IEnumerable<T> Find(Func<T, bool> exp);
        T FirstOrDefault(Func<T, bool> exp);

        void Add(T entity);
        void AddAll(ICollection<T> entities);

        void Delete(T entity);
        void DeleteAll();

        void Save();
    }
}

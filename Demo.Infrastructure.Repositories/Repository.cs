using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Core.Repositories;

namespace Demo.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private List<T> _data = new List<T>();

        public IQueryable<T> FindAll()
        {
            return _data.AsQueryable();
        }

        public IEnumerable<T> Find(Func<T, bool> exp)
        {
            return _data.AsQueryable().Where(exp);
        }

        public T FirstOrDefault(Func<T, bool> exp)
        {
            return _data.AsQueryable().FirstOrDefault(exp);
        }

        public void Add(T entity)
        {
            _data.Add(entity);
        }

        public void AddAll(ICollection<T> entities)
        {
            _data.AddRange(entities);
        }

        public void Delete(T entity)
        {
            _data.Remove(entity);
        }

        public void DeleteAll()
        {
            _data.Clear();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}

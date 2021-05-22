using Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Implementation of this EFcore project so that Domain project do not have dependencies rather other layers  tend depend on doamin layers interface
        /// (This is a simple explanation of Dependency Inversion Principle)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        T GetById(Guid id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}

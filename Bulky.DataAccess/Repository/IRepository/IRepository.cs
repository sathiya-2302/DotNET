using System;
using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    //T-  imagine category write operations needs to be performed on T
    IEnumerable<T> GetAll(string? includeProperties = null);
    T Get(Expression<Func<T,bool>> filter,string? includeProperties = null);
    void Add(T Entity);
    //void Update(T Entity);
    void Remove(T Entity);
    void RemoveRange(IEnumerable<T> Entity);

}

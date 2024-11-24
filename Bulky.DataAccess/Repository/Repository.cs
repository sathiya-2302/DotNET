using System;
using System.Linq.Expressions;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{

    private readonly ApplicationDbContext _db;
    private readonly DbSet<T> DbSet;

    public Repository(ApplicationDbContext db){
        _db = db;
        this.DbSet = _db.Set<T>();
        db.Products.Include(u=>u.Category).Include(u=>u.CategoryId);

    }
    public void Add(T Entity)
    {
       DbSet.Add(Entity);
    }

    public T Get(Expression<Func<T, bool>> filter,string? includeProperties = null)
    {
        
        IQueryable<T> Query=DbSet;
         if(!string.IsNullOrEmpty(includeProperties)){
            foreach(var includeprop in includeProperties
            .Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries)){
                Query=Query.Include(includeprop);
            }
        }
        Query=Query.Where(filter);
        return Query.FirstOrDefault();

    }

    public IEnumerable<T> GetAll(string? includeProperties = null)
    {
        IQueryable<T> Query=DbSet;
        if(!string.IsNullOrEmpty(includeProperties)){
            foreach(var includeprop in includeProperties
            .Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries)){
                Query=Query.Include(includeprop);
            }
        }
        return Query.ToList();
    }

    public void Remove(T Entity)
    {
        DbSet.Remove(Entity);
    }

    public void RemoveRange(IEnumerable<T> Entity)
    {
        DbSet.RemoveRange(Entity);
    }
}

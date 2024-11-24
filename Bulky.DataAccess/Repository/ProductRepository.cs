using System;
using System.Linq.Expressions;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{

    ApplicationDbContext _db;
    public ProductRepository(ApplicationDbContext db) : base(db) 
    {
        _db = db;
    }

    public void Update(Product obj)
    {
       var objFromDb=_db.Products.FirstOrDefault(u=>u.Id==obj.Id);
       if(objFromDb!= null){
        objFromDb.Title = obj.Title;
        objFromDb.ISBN = obj.ISBN;
        objFromDb.Price = obj.Price;
        objFromDb.Price100 = obj.Price100;
        objFromDb.Price50 = obj.Price50;
        objFromDb.ListPrice=obj.ListPrice;
        objFromDb.Author=obj.Author;
        objFromDb.CategoryId=obj.CategoryId;
        objFromDb.Description=obj.Description;
        
        objFromDb.ImageUrl = obj.ImageUrl;

       }
    }
}

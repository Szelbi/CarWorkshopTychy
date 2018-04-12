using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWorkshop.Database
{
    public class ProductRepository
    {
        private DatabaseContext _db;

        public ProductRepository(DatabaseContext db)
        {
            this._db = db;
        }

        public List<Product> List(bool withDeleted = false)
        {
            return this._db.Products.Where(p => p.Deleted.Equals(withDeleted)).ToList();
        }

        public List<Product> ListCategory(int id, bool withDeleted = false)
        {
            return List(withDeleted).Where(c => c.CategoryId.Equals(id)).ToList();
        }

        public Product Get(int id)
        {
            return _db.Products.FirstOrDefault(p => p.ProductId.Equals(id) && !p.Deleted);
        }

        public List<Product> Search(string q)
        {
            return _db.Products.Where(p => !p.Deleted && p.ProductName.Contains(q)).ToList();
        }
    }
}
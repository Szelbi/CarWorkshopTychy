using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWorkshop.Database
{
    public class CategoryRepository
    {
        private DatabaseContext _db;

        public CategoryRepository(DatabaseContext db)
        {
            this._db = db;
        }

        public List<Category> List(bool withDeleted = false)
        {
            return this._db.Categories.Where(c => c.Deleted.Equals(withDeleted)).ToList();
        }

        public bool Exists(int id)
        {
            return this._db.Categories.FirstOrDefault(c => c.CategoryId.Equals(id) && !c.Deleted) != null;
        }
    }
}
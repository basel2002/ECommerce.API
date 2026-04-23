using DAL.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CategoryRepository:GenericRepository<Categorie>,ICategoryRepository
    {
        public CategoryRepository(AppDbContext context):base(context)
        {
            
        }



    }
}

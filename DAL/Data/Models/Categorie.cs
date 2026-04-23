using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Categorie: IAuditableEntity

    {

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }




        // relation with product
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}

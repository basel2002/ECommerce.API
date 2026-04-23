using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL

{
    public class CartItem: IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }

        //relation with cart 

        public int CartId { get; set; }
        public Cart cart    { get; set; }

        //relation with Product
        public int ProductId { get; set; }
        public Product product { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class OrderItem: IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }  // snapshot of price at purchase time
        public decimal TotalPrice { get; set; }

        //relation with order 
        public int OrderId { get; set; }
        public Order Order { get; set; }


        //relation with product

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;


namespace DAL
{
    public class ApplicationUser:IdentityUser, IAuditableEntity
    {




        public string FirstName { get; set; }
        public string LastName { get; set; }

        //relation to cart
        public Cart Cart { get; set; }
        //relation with order 
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        public DateTime CreatedAt { get; set ; }
        public DateTime? UpdatedAt { get; set; }
    }
}

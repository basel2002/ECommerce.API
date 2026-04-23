namespace DAL
{
    public class Cart : IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Id { get; set; }



        //relation with user
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        //relation with cartitwm
        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}

namespace DAL
{
    public class Product: IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }



        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }



        //relation with categorie
        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }



        //relation with cartitwm
        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();

        // relation with orderItem
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}

namespace BLL
{
    public  class ProductEditDto
    {
       
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int StockQty { get; set; }
        //public int CategoryId { get; set; }
    }
}

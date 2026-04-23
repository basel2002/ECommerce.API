namespace BLL
{
    public class CategoryEditDto
    {
        public string? Description { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}

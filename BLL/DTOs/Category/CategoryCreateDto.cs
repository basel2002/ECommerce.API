namespace BLL
{
    public class CategoryCreateDto
    {
        public required string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}

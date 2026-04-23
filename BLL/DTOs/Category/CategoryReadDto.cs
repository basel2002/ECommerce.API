namespace BLL
{
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }

    }
}

namespace Common
{
    public class PageResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public PaginationMetaData Metadata { get; set; } = new();
    }
}

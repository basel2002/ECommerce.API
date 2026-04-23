using DAL;

namespace BLL
{
    public class OrderReadDto
    {
        public string UserId;
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}

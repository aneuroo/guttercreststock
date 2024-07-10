namespace WebApplication2.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public List<string>? Parts { get; set; }
        public List<int>? Required { get; set; }
        public string? OrderId { get; set; }

    }
}

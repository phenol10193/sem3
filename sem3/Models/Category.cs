namespace sem3.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public int ParentId { get; set; }
        public bool Flag { get; set; }
    }
}

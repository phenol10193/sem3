namespace sem3.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public string? UrlImage { get; set; }
        public List<Room>? Rooms { get; set; }
    }
}

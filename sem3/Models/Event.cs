namespace sem3.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public float CostRoom { get; set; }
        public string? UrlImage { get; set;}
        public string? Description { get; set;}
        public int RoomId { get; set; }
        public bool Flag { get; set; }


    }
}

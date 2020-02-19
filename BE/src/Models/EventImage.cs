namespace BE.Models
{
    public class EventImage
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int ImageId { get; set; }

        public virtual Event Event { get; set; }
        public virtual Image Image { get; set; }
    }
}
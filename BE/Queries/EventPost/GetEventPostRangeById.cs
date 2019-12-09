namespace BE.Queries.EventPost
{
    public class GetEventPostRangeById
    {
        public int EventId { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public int UserId { get; set; }
    }
}
namespace BE.Features.FriendshipRecommendation
{
    public class Output
    {
        public int IssuerId { get; set; }
        public int UserId { get; set; }
        public double SimilarityScore { get; set; }
    }
}
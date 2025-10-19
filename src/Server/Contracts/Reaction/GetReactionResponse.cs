namespace PepeProject.Contracts.Reaction
{
    public class GetReactionResponse
    {
        public int ReactionId { get; set; }

        public int MemeId { get; set; }

        public int UserId { get; set; }

        public string ReactionType { get; set; } = null!;

        public DateTime? ReactionDate { get; set; }
    }
}
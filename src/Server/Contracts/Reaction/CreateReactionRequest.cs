namespace PepeProject.Contracts.Reaction
{
    public class CreateReactionRequest
    {
        public int MemeId { get; set; }

        public int UserId { get; set; }

        public string ReactionType { get; set; } = null!;
    }
}
namespace PepeProject.Contracts.UserRole
{
    public class GetUserRoleResponse
    {
        public int UserRoleId { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

        public DateTime? AssignedAt { get; set; }
    }
}
namespace PepeProject.Contracts.Role
{
    public class GetRoleResponse
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
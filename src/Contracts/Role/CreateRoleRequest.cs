namespace PepeProject.Contracts.Role
{
    public class CreateRoleRequest
    {
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
namespace APIVault.API.DTOs.User
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Guid RoleId { get; set; }
        public Guid GroupId { get; set; }
    }

}

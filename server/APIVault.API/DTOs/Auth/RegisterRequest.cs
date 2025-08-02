namespace APIVault.API.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; }
        public int GroupId { get; set; }
    }
}

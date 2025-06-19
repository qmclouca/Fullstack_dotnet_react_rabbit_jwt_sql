namespace Domain.DTOs.User
{
    public class UpdateUserDTO
    {
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? ObservedShares { get; set; }
    }
}

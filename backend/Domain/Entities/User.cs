namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
           
        }
        
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}

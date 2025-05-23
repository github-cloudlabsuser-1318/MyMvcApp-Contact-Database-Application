namespace MyMvcApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;   // Initialized to avoid CS8618 warning
        public string Email { get; set; } = string.Empty;  // Initialized to avoid CS8618 warning
    }
}

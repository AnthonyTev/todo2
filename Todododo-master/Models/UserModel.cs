namespace TODO.Models
{
    public class UserModel
    {
        public bool Status { get; set; }
        public UserData Data { get; set; } = new();
        public string? Message { get; set; }
    }

    public class UserData
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? FirstName { get; set; } 
        public string? Token { get; set; }
    }

}

namespace TODO.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public int Order { get; set; }
        public int UserId { get; set; }
    }
}

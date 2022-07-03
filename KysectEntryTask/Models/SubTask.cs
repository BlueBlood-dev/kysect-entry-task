namespace KysectEntryTask.Models
{
    public class SubTask
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; } = false;
    }
}
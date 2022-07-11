using System.Collections.Generic;

namespace KysectEntryTask.Models
{
    public class Group
    {
        public string Name { get; set; } = "";
        public int Id { get; set; } = -1;
        public List<int> Tasks { get; set; } = new List<int>();
    }
}
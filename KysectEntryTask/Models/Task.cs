using System;
using System.Collections.Generic;

namespace KysectEntryTask.Models
{
    public class Task
    {
        public int Id { get; set; } = -1;
        public bool IsDone { get; set; } = false;
        public string Description { get; set; } = "";
        public DateTime Deadline { get; set; } = DateTime.MaxValue;
        public List<int> SubTasks { get; set; } = new List<int>();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using KysectEntryTask.Models;

namespace KysectEntryTask
{
    public class DataStorage
    {
        public Dictionary<int, Task> Dtask { get; set; } = new Dictionary<int, Task>();

        public Dictionary<int, SubTask> Dsub { get; set; } = new Dictionary<int, SubTask>();

        public Dictionary<int, Group> Dgroup { get; set; } = new Dictionary<int, Group>();

        //maybe EntityFramework would be better. 
        public void AddTask()
        {
            Task task = new Task();
            Console.WriteLine("Enter the description");
            string description = Console.ReadLine();
            if (string.IsNullOrEmpty(description))
            {
                throw new Exception("Empty description");
            }

            int id = Dtask.Any() ? (Dtask.Keys.Max() + 1) : 1;
            task.Id = id;
            task.Description = description;
            Dtask.Add(id, task);
            Console.WriteLine("task was added with id {0}", id);
        }

        public void AddDeadlineTask()
        {
            Console.WriteLine("Enter the id of the task, afterwards enter the deadline in format day/month/year");
            try
            {
                string id = Console.ReadLine();
                string date = Console.ReadLine();
                var dateTime = Convert.ToDateTime(date);
                Dtask[Convert.ToInt32(id)].Deadline = dateTime;
                Console.WriteLine("The deadline to the task with id{0} was added", id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void DisplayTasks()
        {
            foreach (var variableTask in Dtask.Values)
            {
                Console.WriteLine("id: {0}\n{1}\n description: n{2}\ndeadline: {3}\n", variableTask.Id,
                    variableTask.Description,
                    variableTask.Deadline, variableTask.IsDone);
                Console.WriteLine("SubTasks are....");
                foreach (var subTask in variableTask.SubTasks)
                    Console.WriteLine("id: {0}\n description: {1}", Dsub[subTask].Id, Dsub[subTask].Description);
            }
        }

        public void MarkAsCompleted()
        {
            try
            {
                Console.WriteLine("Enter the id");
                string id = Console.ReadLine();
                Dtask[Convert.ToInt32(id)].IsDone = true;
                Console.WriteLine("the task with id " + id + " was marked as done");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void DeleteTask()
        {
            Console.WriteLine("Enter the id");
            int id = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            try
            {
                foreach (var variable in Dtask[id].SubTasks)
                    Dsub.Remove(variable);

                foreach (var group in Dgroup.Values)
                    if (group.Tasks.Contains(id))
                        group.Tasks.Remove(id);

                Dtask.Remove(id);
                Console.WriteLine("Task was deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void TaskForToday()
        {
            DateTime today = DateTime.Today;
            foreach (var task in Dtask.Values)
            {
                if (task.Deadline == today)
                    Console.WriteLine("The task for today is:" + task.Description);
            }
        }

        public void AddSubTask()
        {
            SubTask task = new SubTask();
            Console.WriteLine("enter parent id and afterwards the description");
            string id = Console.ReadLine();
            string description = Console.ReadLine();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(description))
            {
                throw new Exception("Wrong args were passed");
            }

            try
            {
                if (Dtask.ContainsKey(Int32.Parse(id)))
                {
                    task.Id = Dsub.Any() ? (Dsub.Keys.Max() + 1) : 1;
                    task.Description = description;
                    task.ParentId = Convert.ToInt32(id);
                    Dtask[task.ParentId].SubTasks.Add(task.Id);
                    Dsub.Add(task.Id, task);
                    Console.WriteLine("The subtask was added to task {0} and now has the id {1}", id, task.Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public void MarkSubAsCompleted()
        {
            try
            {
                Console.WriteLine("Enter the id of the task");
                string id = Console.ReadLine();
                Dsub[Convert.ToInt32(id)].IsDone = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public void AddGroup()
        {
            Group group = new Group();
            Console.WriteLine("Enter the group name");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("empty name");
            }

            group.Name = name;
            group.Id = Dgroup.Any() ? (Dgroup.Keys.Max() + 1) : 1;
            Console.WriteLine("Enter the id of tasks you want to group, end by 0");
            string input = null;
            while (input != "0")
            {
                input = Console.ReadLine();
                if (input != null && Dtask.ContainsKey(int.Parse(input)))
                    group.Tasks.Add(int.Parse(input));
            }

            Dgroup.Add(group.Id, group);
        }

        public void AddTaskToGroup()
        {
            Console.WriteLine("Enter the task id");
            int taskId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            Console.WriteLine("Enter the group id");
            int groupId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            if (Dtask.ContainsKey(taskId) && Dgroup.ContainsKey(groupId))
                Dgroup[groupId].Tasks.Add(taskId);
            else
                throw new Exception("There are no such id");
        }

        public void DisplayGroups()
        {
            foreach (var group in Dgroup.Values)
            {
                Console.WriteLine("id: {0}\nname: {1}\nTasks are:", group.Id, group.Name);
                foreach (var task in group.Tasks)
                    Console.Write("{0}\n", Dtask[task].Description);
            }
        }

        public void DeleteTaskFromGroup()
        {
            Console.WriteLine("Enter the task id and then the group id");
            int taskId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            int groupId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            if (Dtask.ContainsKey(taskId) && Dgroup.ContainsKey(groupId))
                Dgroup[groupId].Tasks.Remove(taskId);
            else
                throw new Exception("Id is wrong");
        }

        public void DeleteGroup()
        {
            Console.WriteLine("Enter the group id");
            int id = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            if (Dgroup.ContainsKey(id))
                Dgroup.Remove(id);
            else
                Console.WriteLine("there is no such group");
        }
    }
}
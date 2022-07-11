using System;
using System.Data;
using System.IO;
using KysectEntryTask;
using System.Text.Json;
using KysectEntryTask.Models;

namespace App
{
    internal class App
    {
        private DataStorage _data;

        public void Run()
        {
            string path = null;
            Console.WriteLine("Did you have any Tasks saved before? yes/no");
            string userAnswer = Console.ReadLine();

            if (!string.IsNullOrEmpty(userAnswer) && userAnswer.ToLower() == "yes")
            {
                Console.WriteLine("Please enter the absolute path of data storage");
                path = Console.ReadLine();
                if (string.IsNullOrEmpty(path))
                {
                    throw new Exception("Wrong input, comrade");
                }
                else if (!File.Exists(path))
                {
                    throw new Exception("Couldn't find the file");
                }
                else
                {
                    _data = JsonSerializer.Deserialize<DataStorage>(File.ReadAllText(path));
                }
            }
            else if (!string.IsNullOrEmpty(userAnswer) && userAnswer.ToLower() == "no")
            {
                Console.WriteLine("Please enter the absolute path of data storage");
                path = Console.ReadLine();
                if (string.IsNullOrEmpty(path))
                {
                    throw new Exception("Empty path passed");
                }
                else
                {
                    var file = System.IO.File.Create(path);
                    file.Close();
                    if (!File.Exists(path))
                    {
                        throw new Exception("File wasn't created");
                    }

                    _data = new DataStorage();
                }
            }
            else if (string.IsNullOrEmpty(userAnswer) ||
                     (userAnswer.ToLower() != "no" && userAnswer.ToLower() != "yes"))
            {
                Console.WriteLine("Wrong answer, comrade, bye");
                Environment.Exit(0);
            }

            HandleCommands(path);
        }


        private void HandleCommands(string path)
        {
            string command = null;
            while (command != "/exit")
            {
                command = Console.ReadLine();
                string id;
                switch (command)
                {
                    case "/task-add":
                        _data.AddTask();
                        break;
                    case "/task-all":
                        _data.DisplayTasks();
                        break;
                    case "/task-deadline":
                        _data.AddDeadlineTask();
                        break;
                    case "/task-completed":
                        _data.MarkAsCompleted();
                        break;
                    case "/task-today":
                        _data.TaskForToday();
                        break;
                    case "/task-delete":
                        _data.DeleteTask();
                        break;
                    case "/sub-add":
                        _data.AddSubTask();
                        break;
                    case "/sub-completed":
                        _data.MarkSubAsCompleted();
                        break;
                    case "/group-add":
                        _data.AddGroup();
                        break;
                    case "/group-all":
                        _data.DisplayGroups();
                        break;
                    case "/group-delete":
                        _data.DeleteGroup();
                        break;
                    case "/group-task":
                        _data.AddTaskToGroup();
                        break;
                    case "/group-delete-task":
                        _data.DeleteTaskFromGroup();
                        break;
                }
            }

            File.WriteAllText(path, JsonSerializer.Serialize(_data));
        }
    }
}
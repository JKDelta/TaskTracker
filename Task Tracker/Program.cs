using System;
using System.Threading;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
class Program
{
    static void Main()
    {
        Console.WriteLine("Please log in to your account.");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

        Console.WriteLine("Logging in...");
        Thread.Sleep(2000);
        Console.WriteLine($"Login successful! Hello {username}, welcome to your task tracker.");

        Tasks sharedTasks = new Tasks(); // Create a shared instance of Tasks
        Functions functions = new Functions(sharedTasks); // Create a single Functions instance

        Menu.ChooseOption(functions); // Call the menu
    }
}

class Menu
{
    public static void ChooseOption(Functions functions)
    {
        Console.WriteLine("\nPlease select an option from the menu below:");
        Thread.Sleep(1000);
        Console.WriteLine("1. Add a new task");
        Thread.Sleep(300);
        Console.WriteLine("2. View all tasks");


        int choice = Console.ReadLine() switch
        {
            "1" => 0,
            "2" => 1,
            "3" => 2,

            _ => -1 // -1 for invalid input
        };

        if (choice == -1)
        {
            Console.WriteLine("Invalid choice. Please try again.");
            Thread.Sleep(1000);
            Retry.TryAgain(functions); // Loop back
            return;
        }

        Console.WriteLine("You selected option " + (choice + 1) + ".");

        switch (choice)
        {
            case 0:
                Console.WriteLine("Adding a new task...");
                functions.taskAdd();
                break;
            case 1:
                Console.WriteLine("Fetching all your tasks...");
                functions.taskView();

                break;

            default:
                Console.WriteLine("Unexpected issue occurred.");
                break;
        }
    }
}

class Retry
{
    public static void TryAgain(Functions functions)
    {
        Menu.ChooseOption(functions); // Retry menu selection
    }
}
class taskInfo
{

    public string taskName { get; set; }
    public string taskDescription { get; set; }
    public DateTime taskDueDate { get; set; }
    public bool taskStatus { get; set; }
}

class Tasks
{
    public List<taskInfo> allTasks { get; set; } = new List<taskInfo>();
}
class Functions
{
    private Tasks tasks;
    private int selectedTaskIndex = -1;

    public Functions(Tasks tasks)
    {
        this.tasks = tasks; // Assign the shared Tasks object
    }
    static taskInfo task = new taskInfo(); // Create a new task object

    public void taskAdd()
    {

        Console.WriteLine("Please enter the task name:");
        string newTaskName = Console.ReadLine(); // Get task name from user

        Console.WriteLine("Please enter the task description:");
        string newTaskDescription = Console.ReadLine(); // Get task description from user

        Console.WriteLine("Please enter the task due date (yyyy-mm-dd):");
        DateTime newTaskDueDate = DateTime.Parse(Console.ReadLine()); // Get task due date from user

        bool newTaskStatus = false; // Default task status is incomplete

        taskInfo newTask = new taskInfo
        {
            taskName = newTaskName,
            taskDescription = newTaskDescription,
            taskDueDate = newTaskDueDate,
            taskStatus = newTaskStatus

        };
        tasks.allTasks.Add(newTask);
        addReturn();

    }

    public void addReturn()
    {//create a function that changes the text so that this function can be universal


        Console.WriteLine("Task added successfully!");
        //implement JSON serialization to save the task to a file when you learn about it
        Console.WriteLine("Would you like to add another task? (y/n)");
        string addAnother = Console.ReadLine();

        if (addAnother.ToLower() == "y")
        {
            taskAdd(); // Call the method again to add another task
        }
        else if (addAnother.ToLower() == "n")
        {
            Console.WriteLine("Returning to menu...");
            Thread.Sleep(1000);
            Menu.ChooseOption(this); // Return to menu  
        }
        else
        {
            Console.WriteLine("invalid input");
            addReturn();
        }
    }
    public void taskView()
    {

        if (tasks.allTasks.Count == 0)
        {
            Console.WriteLine("No tasks were found.");
            Console.WriteLine("Make sure to add tasks before looking for them.");
        }

        Console.WriteLine("Here are your tasks:");
        for (int i = 0; i < tasks.allTasks.Count; i++)
        {

            Console.WriteLine($"{i + 1}. {tasks.allTasks[i].taskName}");

        }
        Console.WriteLine("Plese select the task you want to view:");
        string select = Console.ReadLine();
        selectedTaskIndex = int.Parse(select);
        //add logic that checks if the input is valid

        if (selectedTaskIndex == tasks.allTasks.Count - 1)
        {
            taskInfo selectedTask = tasks.allTasks[selectedTaskIndex - 1];
            Console.WriteLine($"Name: {selectedTask.taskName}");
            Console.WriteLine($"Description: {selectedTask.taskDescription}");
            Console.WriteLine($"Due Date: {selectedTask.taskDueDate:yyyy-MM-dd}");
            Console.WriteLine($"Status: {(selectedTask.taskStatus ? "Complete" : "Incomplete")}");
        }
        Thread.Sleep(300);
        taskCont();

    }

    public void taskCont()
    {
        Console.WriteLine("What would like to do now:");
        Console.WriteLine("1. Visit another task");
        Console.WriteLine("2. Edit this task");
        Console.WriteLine("3. Set this task as completed");
        string response = Console.ReadLine();

        if (!int.TryParse(response, out int newResponse) || newResponse > 3 || newResponse < 1)
        {
            Console.WriteLine("Invalid input please try again");
        }
        else
        {

            if (newResponse == 1)
            {
                taskView();
            }
            else if (newResponse == 2)
            {
                taskEdit();
            }
            else
            {
                taskInfo selectedTask = tasks.allTasks[selectedTaskIndex - 1];
                selectedTask.taskStatus = !selectedTask.taskStatus;
                Console.WriteLine("This task is now " + selectedTask.taskStatus);
                Thread.Sleep(300);
                taskCont();
            }

        }
    }



    public void taskEdit()
    {
        taskInfo selectedTask = tasks.allTasks[selectedTaskIndex - 1];
        Console.WriteLine("you're now editing task" + selectedTask.taskName);
        Console.WriteLine("Do you want to edit:");
        Console.WriteLine("1. the Task Name");
        Console.WriteLine("2. the Task Description");
        Console.WriteLine("3. the Task Due Date");
        string result = Console.ReadLine();
        if (!int.TryParse(result, out int newresult) || newresult > 3 || newresult < 1)
        {
            Console.WriteLine("Invalid input, please try again.");
            taskEdit();
        }

        if (newresult == 1)
        {
            Console.WriteLine(selectedTask.taskName);
            Console.WriteLine("Write the new name of this task");
            selectedTask.taskName = Console.ReadLine();
        }
        else if (newresult == 2)
        {
            Console.WriteLine(selectedTask.taskDescription);
            Console.WriteLine("Write the new description of this task");
            selectedTask.taskDescription = Console.ReadLine();
        }
        else
        {
            Console.WriteLine(selectedTask.taskDueDate);
            Console.WriteLine("Write the new due date of this task");
            string input = Console.ReadLine();
            if (DateTime.TryParse(input, out DateTime newDueDate))
            {
                selectedTask.taskDueDate = newDueDate;
                Console.WriteLine("Task due date updated successfully!");
            }
            else
            {
                Console.WriteLine("Invalid date format. Please try again.");
                taskEdit(); // Retry editing the task
            }
        }
         Console.WriteLine("Would you like to edit anything else? y/n");
         string edit = Console.ReadLine();

         if (edit == "y")
         {
            taskEdit();
         }
         else
         {
            Console.WriteLine("Would you like to:");
            Console.WriteLine("1. Go back to the previous task");
            Console.WriteLine("2. Go back to your tasks");
            Console.WriteLine("3. Go back to menu");
            taskLoop();
         }

    }
    public void taskLoop()
    {
        string goingto = Console.ReadLine();
        int goinginto = int.Parse(goingto);
        if (goinginto == 1)
        {
         taskCont();
        }
        else if (goinginto == 2)
        {
         taskView();
        }
        else
        {
         Menu.ChooseOption(this);
        }
    }

}
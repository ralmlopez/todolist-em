namespace Tasks;

public class UI
{
    public static void ShowMenu()
    {
        Console.WriteLine("1. Create Task");
        Console.WriteLine("2. Update Task");
        Console.WriteLine("3. Complete Task");
        Console.WriteLine("4. Remove Task");
        Console.WriteLine("q. Quit");
        Console.WriteLine();
        Console.Write("Enter choice: ");
    }

    public static void ShowMessage(string message)
    {
        if (string.IsNullOrEmpty(message)) return;

        ConsoleColor originalBackground = Console.BackgroundColor;
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.BackgroundColor = originalBackground;
        Console.WriteLine();
    }

    public static void ShowTasks(DataStore dataStore)
    {
        const int spaces = 10;
        const int listWidth = 60;
        var blankSpaces = new string(' ', spaces);
        var listHorizontalBorder = new string('-', listWidth);
        var headerFormat = $" {{0, -{listWidth}}}{blankSpaces}  {{1, -{listWidth}}}";

        Console.Clear();
        Console.WriteLine(string.Format(headerFormat, "TASKS", "DONE"));
        Console.WriteLine($" {listHorizontalBorder}{blankSpaces}  {listHorizontalBorder}");

        var todo = Projections.GetTasksPending(dataStore)
            .Select(x => $"{x.RowNumber} | {x.Id} | {x.Task}").ToList();
        var completed = Projections.GetTasksCompleted(dataStore)
            .Select(x => $"{x.Id} | {x.Task}").ToList();

        int maxCount = Math.Max(todo.Count, completed.Count);
        for (int i = 0; i < maxCount; i++)
        {
            var taskDone = i < todo.Count ? todo[i] : string.Empty;
            var taskNotDone = i < completed.Count ? completed[i] : string.Empty;
            Console.WriteLine($"|{FormatForDisplay(taskDone)}|{blankSpaces}|{FormatForDisplay(taskNotDone)}|");
        }

        Console.WriteLine($" {listHorizontalBorder}{blankSpaces}  {listHorizontalBorder}");
        Console.WriteLine();
    }

    public static string GetTaskName()
    {
        Console.Write("Enter task name: ");
        return Console.ReadLine() ?? string.Empty;
    }

    public static (Guid, string) GetIdAndNewTaskName(DataStore dataStore)
    {
        Console.Write("Enter task id to update: ");
        var input = Console.ReadLine() ?? string.Empty;
        var id = Projections.GetTasksPending(dataStore).First(x => x.RowNumber == int.Parse(input)).Id;

        Console.Write("Enter updated task name: ");
        var newTaskName = Console.ReadLine() ?? string.Empty;

        return (id, newTaskName);
    }

    public static (Guid, string) GetTaskIdToComplete(DataStore dataStore)
    {
        Console.Write("Enter task id to complete: ");
        var input = Console.ReadLine() ?? string.Empty;
        var pendingTask = Projections.GetTasksPending(dataStore).First(x => x.RowNumber == int.Parse(input));
        return (pendingTask.Id, pendingTask.Task);
    }

    public static (Guid, string) GetTaskIdToRemove(DataStore dataStore)
    {
        Console.Write("Enter task id to remove: ");
        var input = Console.ReadLine() ?? string.Empty;
        var pendingTask = Projections.GetTasksPending(dataStore).First(x => x.RowNumber == int.Parse(input));
        return (pendingTask.Id, pendingTask.Task);
    }

    static string FormatForDisplay(string task)
    {
        const int maxLength = 60;
        return task.Length > maxLength ? task.Substring(0, maxLength) : task.PadRight(maxLength);
    }

}

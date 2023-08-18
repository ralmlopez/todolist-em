namespace Tasks;

public class UI
{
    public static void ShowMenu()
    {
        Console.WriteLine("1. Create Task");
        Console.WriteLine("2. Update Task");
        Console.WriteLine("3. Complete Task");
        Console.WriteLine("q. Quit");
        Console.WriteLine();
        Console.Write("Enter choice: ");
    }

    public static void ShowTasks(DataStore dataStore)
    {
        const int spaces = 10;
        const int listWidth = 20;
        var blankSpaces = new string(' ', spaces);
        var listHorizontalBorder = new string('-', listWidth);
        var headerFormat = $" {{0, -{listWidth}}}{blankSpaces}  {{1, -{listWidth}}}";

        Console.Clear();
        Console.WriteLine(string.Format(headerFormat, "TASKS", "DONE"));
        Console.WriteLine($" {listHorizontalBorder}{blankSpaces}  {listHorizontalBorder}");

        var todo = Projections.GetTasksPending(dataStore).ToList();
        var completed = Projections.GetTasksCompleted(dataStore).ToList();

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

    public static (int, string) GetIdAndNewTaskName()
    {
        Console.Write("Enter task id to update: ");
        var input = Console.ReadLine() ?? string.Empty;
        var id = int.Parse(input == string.Empty ? "0" : input);

        Console.Write("Enter updated task name: ");
        var newTaskName = Console.ReadLine() ?? string.Empty;

        return (id, newTaskName);
    }

    public static int GetCompletedTaskId()
    {
        Console.Write("Enter completed task id: ");
        var input = Console.ReadLine() ?? string.Empty;
        return int.Parse(input == string.Empty ? "0" : input);
    }

    static string FormatForDisplay(string task)
    {
        const int maxLength = 20;
        return task.Length > maxLength ? task.Substring(0, maxLength) : task.PadRight(maxLength);
    }

}

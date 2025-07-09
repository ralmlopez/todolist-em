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

    public static void ShowTasks(IEnumerable<TaskEvent> allTasks)
    {
        const int spaces = 10;
        const int listWidth = 60;
        var blankSpaces = new string(' ', spaces);
        var listHorizontalBorder = new string('-', listWidth);
        var headerFormat = $" {{0, -{listWidth}}}{blankSpaces}  {{1, -{listWidth}}}";

        Console.Clear();
        Console.WriteLine(string.Format(headerFormat, "TASKS", "DONE"));
        Console.WriteLine($" {listHorizontalBorder}{blankSpaces}  {listHorizontalBorder}");

        var todo = TaskList(allTasks)
            .Select(x => $"{x.RowNumber} | {x.Id} | {x.Task}").ToList();
        var completed = TaskCompletedList(allTasks)
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


    public static IEnumerable<PendingTask> TaskList(IEnumerable<TaskEvent> allTasks)
    {
        var exclusions = new HashSet<Guid>(allTasks.Where(x => x.EventType == EventType.TaskCompleted
                    || x.EventType == EventType.TaskRemoved).Select(x => x.Id));

        return allTasks
            .Where(x => (x.EventType == EventType.TaskCreated || x.EventType == EventType.TaskUpdated)
                    && !exclusions.Contains(x.Id))
            .GroupBy(x => x.Id)
            .Select(g => g.OrderBy(c => c.Id).ThenByDescending(c => c.Created).First())
            .Select((x, index) => new PendingTask { RowNumber= index + 1, Id = x.Id, Task = x.Task});
    }

    public static IEnumerable<CompletedTask> TaskCompletedList(IEnumerable<TaskEvent> allTasks)
    {
        var exclude = new HashSet<Guid>(allTasks.Where(x => x.EventType == EventType.TaskRemoved).Select(x => x.Id));

        return allTasks
            .Where(task => task.EventType == EventType.TaskCompleted
                    && !exclude.Contains(task.Id))
            .Select(x => new CompletedTask { Id = x.Id, Task = x.Task});
    }

    public static string GetTaskName()
    {
        Console.Write("Enter task name: ");
        return Console.ReadLine() ?? string.Empty;
    }

    public static (Guid, string) GetIdAndNewTaskName(IEnumerable<TaskEvent> allTasks)
    {
        Console.Write("Enter task id to update: ");
        var input = Console.ReadLine() ?? string.Empty;
        var id = TaskList(allTasks).First(x => x.RowNumber == int.Parse(input)).Id;

        Console.Write("Enter updated task name: ");
        var newTaskName = Console.ReadLine() ?? string.Empty;

        return (id, newTaskName);
    }

    public static (Guid, string) GetTaskIdToComplete(IEnumerable<TaskEvent> allTasks)
    {
        Console.Write("Enter task id to complete: ");
        var input = Console.ReadLine() ?? string.Empty;
        var pendingTask = TaskList(allTasks).First(x => x.RowNumber == int.Parse(input));
        return (pendingTask.Id, pendingTask.Task);
    }

    public static (Guid, string) GetTaskIdToRemove(IEnumerable<TaskEvent> allTasks)
    {
        Console.Write("Enter task id to remove: ");
        var input = Console.ReadLine() ?? string.Empty;
        var pendingTask = TaskList(allTasks).First(x => x.RowNumber == int.Parse(input));
        return (pendingTask.Id, pendingTask.Task);
    }

    static string FormatForDisplay(string task)
    {
        const int maxLength = 60;
        return task.Length > maxLength ? task.Substring(0, maxLength) : task.PadRight(maxLength);
    }

}

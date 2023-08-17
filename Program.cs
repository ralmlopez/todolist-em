const string FileName = "Tasks.txt";

RunProgram();

static void RunProgram()
{
    Console.BackgroundColor = ConsoleColor.DarkBlue;
    var quit = false;
    while (!quit)
    {
        ShowTasks();
        ShowMenu();
        quit = HandleChoice();
    }
}

static bool HandleChoice()
{
    var quit = false;
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.Write("Enter task name: ");
            AppendEvent(Console.ReadLine() ?? string.Empty, EventType.TaskCreated);
            break;

        case "2":
            Console.Write("Enter completed task name: ");
            AppendEvent(Console.ReadLine() ?? string.Empty, EventType.TaskCompleted);
            break;

        case "3":
            quit = true;
            break;
        default:
            break;
    }

    return quit;
}

static void AppendEvent(string task, string eventType)
{
    var record = $"{task},{eventType},{DateTime.Now}";
    File.AppendAllLines(FileName, new[] { record });
}

static void ShowMenu()
{
    Console.WriteLine("1. Create Task");
    Console.WriteLine("2. Finish Task");
    Console.WriteLine("3. Quit");
    Console.WriteLine();
    Console.Write("Enter choice: ");
}

static void ShowTasks()
{
    const int spaces = 10;
    const int listWidth = 20;
    var blankSpaces = new string(' ', spaces);
    var listHorizontalBorder = new string('-', listWidth);
    var headerFormat = $" {{0, -{listWidth}}}{blankSpaces}  {{1, -{listWidth}}}";

    Console.Clear();
    Console.WriteLine(string.Format(headerFormat, "TASKS", "DONE"));
    Console.WriteLine($" {listHorizontalBorder}{blankSpaces}  {listHorizontalBorder}");


    if (File.Exists(FileName))
    {
        var todo = GetTasksPending().ToList();
        var completed = GetTasksCompleted().ToList();

        int maxCount = Math.Max(todo.Count, completed.Count);
        for (int i = 0; i < maxCount; i++)
        {
            var taskDone = i < todo.Count ? todo[i] : string.Empty;
            var taskNotDone = i < completed.Count ? completed[i] : string.Empty;
            Console.WriteLine($"|{FormatForDisplay(taskDone)}|{blankSpaces}|{FormatForDisplay(taskNotDone)}|");
        }
    }

    Console.WriteLine($" {listHorizontalBorder}{blankSpaces}  {listHorizontalBorder}");
    Console.WriteLine();
}

static string FormatForDisplay(string task)
{
    const int maxLength = 20;
    return task.Length > maxLength ? task.Substring(0, maxLength) : task.PadRight(maxLength);
}

static IEnumerable<string> GetTasksPending()
{
    var allTasks = File
        .ReadLines(FileName)
        .Select(line => line.Split(','))
        .Select(items => new { Task = items[0], EventType = items[1] })
        .ToList();

    var allDone = new HashSet<string>(allTasks.Where(x => x.EventType == EventType.TaskCompleted).Select(x => x.Task));

    return allTasks
        .Where(x => x.EventType == EventType.TaskCreated && !allDone.Contains(x.Task))
        .Select(x => x.Task);
}

static IEnumerable<string> GetTasksCompleted()
{
    return File
        .ReadLines(FileName)
        .Select(line => line.Split(','))
        .Where(items => items[1] == EventType.TaskCompleted)
        .Select(items => items[0])
        .ToList();
}

static class EventType
{
    public const string TaskCreated = "TaskCreated";
    public const string TaskCompleted = "TaskCompleted";
}

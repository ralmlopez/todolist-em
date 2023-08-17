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
            CreateTask(Console.ReadLine() ?? string.Empty);
            break;

        case "2":
            FinishTask(Console.ReadLine() ?? string.Empty);
            break;

        case "3":
            quit = true;
            break;
        default:
            break;
    }

    return quit;
}

static void CreateTask(string item)
{
    var record = $"{item},{false.ToString()},{DateTime.Now.ToString()}";

    using (StreamWriter outputFile = new StreamWriter(FileName, true))
    {
        outputFile.WriteLine(record);
    }
}

static void FinishTask(string item)
{
    var record = $"{item},{true.ToString()},{DateTime.Now.ToString()}";

    using (StreamWriter outputFile = new StreamWriter(FileName, true))
    {
        outputFile.WriteLine(record);
    }
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

    Console.Clear();

    Console.WriteLine($" {FormatForDisplay("NOT DONE")}{blankSpaces}  {FormatForDisplay("DONE")}");
    Console.WriteLine($" {listHorizontalBorder}{blankSpaces}  {listHorizontalBorder}");

    var todo = GetTaskListNotDone();
    var completed = GetTaskListDone();

    var joined = todo.Zip(completed, (done, notDone) => new { Done = done, NotDone = notDone })
        .Concat(todo.Skip(completed.Count()).Select(done => new { Done = done, NotDone = string.Empty }))
        .Concat(completed.Skip(todo.Count()).Select(notDone => new { Done = string.Empty, NotDone = notDone }))
        .ToList();

    foreach (var task in joined)
    {
        Console.WriteLine($"|{FormatForDisplay(task.Done)}|{new string(' ', spaces)}|{FormatForDisplay(task.NotDone)}|");
    }

    Console.WriteLine($" {listHorizontalBorder}{blankSpaces}  {listHorizontalBorder}");
    Console.WriteLine();
}

static string FormatForDisplay(string item)
{
    const int maxLength = 20;
    return item.Length > maxLength ? item.Substring(0, maxLength) : $"{item}{new string(' ', maxLength - item.Length)}";
}

static IEnumerable<string> GetTaskListNotDone()
{
    var allTasks = File
        .ReadLines(FileName)
        .Select(line => line.Split(','))
        .Select(items => new { Task = items[0], Done = bool.Parse(items[1]) })
        .ToList();

    var allDone = allTasks
        .Where(x => x.Done)
        .Select(x => x.Task)
        .ToList();

    return allTasks
        .Where(x => !x.Done && !allDone.Contains(x.Task))
        .Select(x => x.Task)
        .ToArray();
}

static IEnumerable<string> GetTaskListDone()
{
    return File
        .ReadLines(FileName)
        .Select(line => line.Split(','))
        .Where(items => items[1] == true.ToString())
        .Select(items => items[0])
        .ToList();
}

using Tasks;

const string FileName = "Tasks.txt";

Run();

static void Run()
{
    Console.BackgroundColor = ConsoleColor.DarkBlue;
    var quit = false;
    var dataStore = new DataStore(FileName);

    while (!quit)
    {
        UI.ShowTasks(dataStore);
        UI.ShowMenu();
        quit = ProcessInput(dataStore);
    }
}

static bool ProcessInput(DataStore dataStore)
{
    var quit = false;
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Commands.CreateTask(dataStore, UI.GetTaskName());
            break;

        case "2":
            var input = UI.GetIdAndNewTaskName();
            Commands.UpdateTask(dataStore, input.Item1, input.Item2);
            break;

        case "3":
            Commands.CompleteTask(dataStore, UI.GetTaskIdToComplete());
            break;

        case "4":
            Commands.RemoveTask(dataStore, UI.GetTaskIdToRemove());
            break;

        case "q":
            quit = true;
            break;
        default:
            break;
    }

    return quit;
}

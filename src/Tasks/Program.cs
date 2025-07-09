using Tasks;

const string FileName = "Tasks.txt";

Run();

static void Run()
{
    Console.BackgroundColor = ConsoleColor.DarkGreen;
    var quit = false;
    var message = string.Empty;
    var dataStore = new DataStore(FileName);
    var allTasks = dataStore.GetAllEvents().ToList();

    while (!quit)
    {
        UI.ShowTasks(allTasks);
        UI.ShowMessage(message);
        UI.ShowMenu();
        message = string.Empty;

        (quit, message) = ProcessInput(dataStore, allTasks);
        allTasks = dataStore.GetAllEvents().ToList();
    }
}

static (bool, string) ProcessInput(DataStore dataStore, IEnumerable<TaskEvent> allTasks)
{
    var quit = false;
    var message = string.Empty;
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            var (createdEvent, error) = new CreateTaskCommand().Execute(UI.GetTaskName(), allTasks);
            if (!string.IsNullOrEmpty(error))
            {
                message = error;
            }
            else
            {
                dataStore.AppendEvent(createdEvent);
            }
            break;

        case "2":
            var updateInfo = UI.GetIdAndNewTaskName(allTasks);
            var updatedEvent = new UpdateTaskCommand().Execute(updateInfo.Item1, updateInfo.Item2);
            dataStore.AppendEvent(updatedEvent);
            break;

        case "3":
            var completeInfo = UI.GetTaskIdToComplete(allTasks);
            var completedEvent = new CompleteTaskCommand().Execute(completeInfo.Item1, completeInfo.Item2);
            dataStore.AppendEvent(completedEvent);
            break;

        case "4":
            var removeInfo = UI.GetTaskIdToRemove(allTasks);
            var removedEvent = new RemoveTaskCommand().Execute(removeInfo.Item1, removeInfo.Item2);
            dataStore.AppendEvent(removedEvent);
            break;

        case "q":
            quit = true;
            break;
        default:
            break;
    }

    return (quit, message);
}

using Tasks;

const string FileName = "Tasks.txt";

Run();

static void Run()
{
    Console.BackgroundColor = ConsoleColor.DarkBlue;
    var quit = false;
    var message = string.Empty;
    var dataStore = new DataStore(FileName);

    while (!quit)
    {
        UI.ShowTasks(dataStore);
        UI.ShowMessage(message);
        UI.ShowMenu();
        message = string.Empty;

        (quit, message) = ProcessInput(dataStore);
    }
}

static (bool, string) ProcessInput(DataStore dataStore)
{
    var quit = false;
    var message = string.Empty;
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            var (createdEvent, error) = Commands.CreateTask(UI.GetTaskName());
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
            var updateInfo = UI.GetIdAndNewTaskName(dataStore);
            var updatedEvent = Commands.UpdateTask(updateInfo.Item1, updateInfo.Item2);
            dataStore.AppendEvent(updatedEvent);
            break;

        case "3":
            var completeInfo = UI.GetTaskIdToComplete(dataStore);
            var completedEvent = Commands.CompleteTask(completeInfo.Item1, completeInfo.Item2);
            dataStore.AppendEvent(completedEvent);
            break;

        case "4":
            var removeInfo = UI.GetTaskIdToComplete(dataStore);
            var removedEvent = Commands.RemoveTask(removeInfo.Item1, removeInfo.Item2);
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

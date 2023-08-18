namespace Tasks;

public class Commands
{
    public static void CreateTask(DataStore dataStore, string task)
    {
        if (task == null) return;
        dataStore.AppendEvent(task, EventType.TaskCreated);
    }

    public static void UpdateTask(DataStore dataStore, int id, string task)
    {
        if (Projections.GetTaskPending(dataStore, id) == null) return;

        dataStore.AppendEvent(id, task, EventType.TaskUpdated);
    }

    public static void CompleteTask(DataStore dataStore, int id)
    {
        var task = Projections.GetTaskPending(dataStore, id);
        if (task == null) return;

        dataStore.AppendEvent(id, task, EventType.TaskCompleted);
    }

    public static void RemoveTask(DataStore dataStore, int id)
    {
        var task = Projections.GetTasksNotRemoved(dataStore, id);
        if (task == null) return;

        dataStore.AppendEvent(id, task, EventType.TaskRemoved);
    }
}

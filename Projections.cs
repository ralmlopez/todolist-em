namespace Tasks;

public class Projections
{
    public static IEnumerable<string> GetTasksPending(DataStore dataStore)
    {
        var allTasks = dataStore.GetAllEvents().ToList();

        var allDone = new HashSet<int>(allTasks.Where(x => x.EventType == EventType.TaskCompleted).Select(x => x.Id));

        return allTasks
            .Where(x => (x.EventType == EventType.TaskCreated || x.EventType == EventType.TaskUpdated)
                    && !allDone.Contains(x.Id))
            .GroupBy(x => x.Id)
            .Select(g => g.OrderBy(c => c.Id).ThenByDescending(c => c.Created).First())
            .Select(x => $"{x.Id}:{x.Task}");
    }

    public static string? GetTaskPending(DataStore dataStore, int id)
    {
        var allTasks = dataStore.GetAllEvents().ToList();

        var allDone = new HashSet<int>(allTasks.Where(x => x.EventType == EventType.TaskCompleted).Select(x => x.Id));

        return allTasks
            .Where(x => (x.EventType == EventType.TaskCreated || x.EventType == EventType.TaskUpdated)
                    && !allDone.Contains(x.Id)
                    && x.Id == id)
            .GroupBy(x => x.Id)
            .Select(g => g.OrderBy(c => c.Id).ThenByDescending(c => c.Created).First())
            .Select(x => x.Task)
            .FirstOrDefault();
    }

    public static IEnumerable<string> GetTasksCompleted(DataStore dataStore)
    {
        return dataStore
            .GetAllEvents()
            .Where(task => task.EventType == EventType.TaskCompleted)
            .Select(task => task.Task);
    }
}

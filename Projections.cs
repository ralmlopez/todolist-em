namespace Tasks;

public class Projections
{
    public static IEnumerable<string> GetTasksPending(DataStore dataStore)
    {
        var allTasks = dataStore.GetAllEvents().ToList();

        var exclusions = new HashSet<int>(allTasks.Where(x => x.EventType == EventType.TaskCompleted
                    || x.EventType == EventType.TaskRemoved).Select(x => x.Id));

        return allTasks
            .Where(x => (x.EventType == EventType.TaskCreated || x.EventType == EventType.TaskUpdated)
                    && !exclusions.Contains(x.Id))
            .GroupBy(x => x.Id)
            .Select(g => g.OrderBy(c => c.Id).ThenByDescending(c => c.Created).First())
            .Select(x => $"{x.Id}:{x.Task}");
    }

    public static string? GetTaskPending(DataStore dataStore, int id)
    {
        var allTasks = dataStore.GetAllEvents().ToList();

        var exclusions = new HashSet<int>(allTasks.Where(x => x.EventType == EventType.TaskCompleted
                    || x.EventType == EventType.TaskRemoved).Select(x => x.Id));

        return allTasks
            .Where(x => (x.EventType == EventType.TaskCreated || x.EventType == EventType.TaskUpdated)
                    && !exclusions.Contains(x.Id)
                    && x.Id == id)
            .GroupBy(x => x.Id)
            .Select(g => g.OrderBy(c => c.Id).ThenByDescending(c => c.Created).First())
            .Select(x => x.Task)
            .FirstOrDefault();
    }

    public static string? GetTasksNotRemoved(DataStore dataStore, int id)
    {
        var allTasks = dataStore.GetAllEvents().ToList();

        return allTasks
            .Where(x => x.EventType != EventType.TaskRemoved
                    && x.Id == id)
            .GroupBy(x => x.Id)
            .Select(g => g.OrderBy(c => c.Id).ThenByDescending(c => c.Created).First())
            .Select(x => x.Task)
            .FirstOrDefault();
    }

    public static IEnumerable<string> GetTasksCompleted(DataStore dataStore)
    {
        var allTasks = dataStore.GetAllEvents().ToList();
        var exclude = new HashSet<int>(allTasks.Where(x => x.EventType == EventType.TaskRemoved).Select(x => x.Id));

        return dataStore
            .GetAllEvents()
            .Where(task => task.EventType == EventType.TaskCompleted
                    && !exclude.Contains(task.Id))
            .Select(x => $"{x.Id}:{x.Task}");
    }
}

namespace Tasks;

public class PendingTask
{
    public int RowNumber { get; set; }
    public Guid Id { get; set; }
    public string Task { get; set; }
}


public class CompletedTask
{
    public Guid Id { get; set; }
    public string Task { get; set; }
}

public class Projections
{


    public static IEnumerable<PendingTask> GetTasksPending(DataStore dataStore)
    {
        var allTasks = dataStore.GetAllEvents().ToList();

        var exclusions = new HashSet<Guid>(allTasks.Where(x => x.EventType == EventType.TaskCompleted
                    || x.EventType == EventType.TaskRemoved).Select(x => x.Id));

        return allTasks
            .Where(x => (x.EventType == EventType.TaskCreated || x.EventType == EventType.TaskUpdated)
                    && !exclusions.Contains(x.Id))
            .GroupBy(x => x.Id)
            .Select(g => g.OrderBy(c => c.Id).ThenByDescending(c => c.Created).First())
            .Select((x, index) => new PendingTask { RowNumber= index + 1, Id = x.Id, Task = x.Task});
    }

    public static string GetTaskPending(DataStore dataStore, Guid id)
    {
        var allTasks = dataStore.GetAllEvents().ToList();

        var exclusions = new HashSet<Guid>(allTasks.Where(x => x.EventType == EventType.TaskCompleted
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

    public static string GetTasksNotRemoved(DataStore dataStore, Guid id)
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

    public static IEnumerable<CompletedTask> GetTasksCompleted(DataStore dataStore)
    {
        var allTasks = dataStore.GetAllEvents().ToList();
        var exclude = new HashSet<Guid>(allTasks.Where(x => x.EventType == EventType.TaskRemoved).Select(x => x.Id));

        return dataStore
            .GetAllEvents()
            .Where(task => task.EventType == EventType.TaskCompleted
                    && !exclude.Contains(task.Id))
            .Select(x => new CompletedTask { Id = x.Id, Task = x.Task});
    }
}

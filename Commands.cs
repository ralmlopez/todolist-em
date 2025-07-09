namespace Tasks;

public class Commands
{
    public static (TaskEvent, string) CreateTask(string task, IEnumerable<TaskEvent> allTasks)
    {
        if (string.IsNullOrEmpty(task))
        {
            return (null, "Task cannot be empty");
        }


        var exclusions = new HashSet<Guid>(allTasks.Where(x => x.EventType == EventType.TaskCompleted
                    || x.EventType == EventType.TaskRemoved).Select(x => x.Id));

        var duplicate = allTasks
            .Where(x => (x.EventType == EventType.TaskCreated || x.EventType == EventType.TaskUpdated)
                    && !exclusions.Contains(x.Id))
            .GroupBy(x => x.Id)
            .Select(g => g.OrderBy(c => c.Id).ThenByDescending(c => c.Created).First())
            .Any(x => x.Task == task);

        if (duplicate)
        {
            return (null, "Task already exists");
        }

        return (new TaskEvent(Guid.NewGuid(), task, EventType.TaskCreated, DateTime.Now), null);
    }

    public static TaskEvent UpdateTask(Guid id, string task)
    {
        return new TaskEvent(id, task, EventType.TaskUpdated, DateTime.Now);
    }

    public static TaskEvent CompleteTask(Guid id, string task)
    {
        return new TaskEvent(id, task, EventType.TaskCompleted, DateTime.Now);
    }

    public static TaskEvent RemoveTask(Guid id, string task)
    {
        return new TaskEvent(id, task, EventType.TaskRemoved, DateTime.Now);
    }
}

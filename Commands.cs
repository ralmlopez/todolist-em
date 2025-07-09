namespace Tasks;

public class Commands
{
    public static (TaskEvent?, string?) CreateTask(string task)
    {
        if (string.IsNullOrEmpty(task))
        {
            return (null, "Task cannot be empty");
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

namespace Tasks;

public class TaskEvent
{
    public Guid Id { get; private set; }
    public string Task { get; private set; }
    public string EventType { get; private set; }
    public DateTime Created { get; private set; }

    public TaskEvent(Guid id, string task, string eventType, DateTime created)
    {
        Id = id;
        Task = task;
        EventType = eventType;
        Created = created;
    }

    public override string ToString()
    {
        return $"{Id}:{Task}:{EventType}:{Created}";
    }
}

public class EventType
{
    public const string TaskCreated = "TaskCreated";
    public const string TaskUpdated = "TaskUpdated";
    public const string TaskCompleted = "TaskCompleted";
    public const string TaskRemoved = "TaskRemoved";
}

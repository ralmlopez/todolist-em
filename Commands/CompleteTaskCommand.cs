namespace Tasks;

public class CompleteTaskCommand
{
    public TaskEvent Execute(Guid id, string task)
    {
        return new TaskEvent(id, task, EventType.TaskCompleted, DateTime.Now);
    }
}

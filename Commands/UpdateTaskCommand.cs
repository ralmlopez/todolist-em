namespace Tasks;

public class UpdateTaskCommand
{
    public TaskEvent Execute(Guid id, string task)
    {
        return new TaskEvent(id, task, EventType.TaskUpdated, DateTime.Now);
    }
}

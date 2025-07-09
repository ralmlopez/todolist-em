namespace Tasks;

public class RemoveTaskCommand
{
    public TaskEvent Execute(Guid id, string task)
    {
        return new TaskEvent(id, task, EventType.TaskRemoved, DateTime.Now);
    }
}

namespace Tasks;

public class DataStore
{
    readonly string fileName;

    public DataStore(string fileName)
    {
        this.fileName = fileName;
    }

    public void AppendEvent(TaskEvent taskEvent)
    {
        var record = $"{taskEvent.Id},{taskEvent.Task},{taskEvent.EventType},{taskEvent.Created}";
        File.AppendAllLines(fileName, new[] { record });
    }

    public IEnumerable<TaskEvent> GetAllEvents()
    {
        if (File.Exists(fileName))
        {
            return File
                .ReadLines(fileName)
                .Select(line => line.Split(','))
                .Select(item =>
                        new TaskEvent(
                            id: System.Guid.Parse(item[0])
                            , task: item[1]
                            , eventType: item[2]
                            , created: DateTime.Parse(item[3])));
        }

        return new List<TaskEvent>();
    }
}

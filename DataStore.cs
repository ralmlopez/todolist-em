namespace Tasks;

public class DataStore
{
    readonly string fileName;
    int nextId = 0;

    public DataStore(string fileName)
    {
        this.fileName = fileName;
        var events = GetAllEvents().ToList();
        nextId = events.Count == 0 ? 1 : events.Max(e => e.Id);
    }

    public void AppendEvent(string task, string eventType)
    {
        var record = $"{nextId},{task},{eventType},{DateTime.Now}";
        File.AppendAllLines(fileName, new[] { record });
        nextId++;
    }

    public void AppendEvent(int id, string task, string eventType)
    {
        var record = $"{id},{task},{eventType},{DateTime.Now}";
        File.AppendAllLines(fileName, new[] { record });
        nextId++;
    }

    public IEnumerable<Event> GetAllEvents()
    {
        if (File.Exists(fileName))
        {
            return File
                .ReadLines(fileName)
                .Select(line => line.Split(','))
                .Select(item =>
                        new Event(
                            id: int.Parse(item[0])
                            , task: item[1]
                            , eventType: item[2]
                            , created: DateTime.Parse(item[3])));
        }

        return new List<Event>();
    }
}

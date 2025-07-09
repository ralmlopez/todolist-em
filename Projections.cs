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

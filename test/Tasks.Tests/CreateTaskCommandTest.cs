using FluentAssertions.Extensions;

namespace Tasks.Tests;

public class CreateTaskCommandTest
{
    const string DEFAULT_TASK = "Task1";

    [Theory]
    [MemberData(nameof(CreateData))]
    public void ShouldReturnCreateTaskEvent(IEnumerable<TaskEvent> events)
    {
        var command = new CreateTaskCommand();

        var result = command.Execute(DEFAULT_TASK, events);

        var resultEvent = result.Item1;
        var resultError = result.Item2;

        resultEvent.Should().NotBeNull();
        resultEvent.Id.Should().NotBeEmpty();
        resultEvent.Task.Should().Be(DEFAULT_TASK);
        resultEvent.EventType.Should().Be(EventType.TaskCreated);
        resultEvent.Created.Should().BeWithin(10.Seconds()).Before(DateTime.Now);

        resultError.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldNotAllowAnEmptyTask(string task)
    {
        var events = new List<TaskEvent>();
        var command = new CreateTaskCommand();

        var result = command.Execute(task, events);

        var resultEvent = result.Item1;
        var resultError = result.Item2;

        resultEvent.Should().BeNull();

        resultError.Should().NotBeNull();
        resultError.Should().Contain("empty");
    }

    [Theory]
    [MemberData(nameof(DuplicateData))]
    public void ShouldNotAllowDuplicateTask(IEnumerable<TaskEvent> events)
    {
        var command = new CreateTaskCommand();

        var result = command.Execute(DEFAULT_TASK, events);

        var resultEvent = result.Item1;
        var resultError = result.Item2;

        resultEvent.Should().BeNull();

        resultError.Should().NotBeNull();
        resultError.Should().Contain("exists");
    }

    public static IEnumerable<object[]> CreateData()
    {
        var guid = Guid.NewGuid();

        return
        new List<object[]>
        {
            new object[]
            {
                new List<TaskEvent> { }
            },

            new object[]
            {
                new List<TaskEvent>
                {
                    new TaskEvent(guid, DEFAULT_TASK, EventType.TaskCreated, DateTime.Parse("1/1/2025")),
                    new TaskEvent(guid, "Any Task", EventType.TaskUpdated, DateTime.Parse("1/2/2025"))
                }
            },

            new object[]
            {
                new List<TaskEvent>
                {
                    new TaskEvent(guid, DEFAULT_TASK, EventType.TaskCreated, DateTime.Parse("1/1/2025")),
                    new TaskEvent(guid, DEFAULT_TASK, EventType.TaskRemoved, DateTime.Parse("1/2/2025"))
                }
            },

            new object[]
            {
                new List<TaskEvent>
                {
                    new TaskEvent(guid, DEFAULT_TASK, EventType.TaskCreated, DateTime.Parse("1/1/2025")),
                    new TaskEvent(guid, DEFAULT_TASK, EventType.TaskCompleted, DateTime.Parse("1/2/2025"))
                }
            }
        };
    }

    public static IEnumerable<object[]> DuplicateData()
    {
        var guid = Guid.NewGuid();

        return
        new List<object[]>
        {
            new object[]
            {
                new List<TaskEvent>
                {
                    new TaskEvent(guid, DEFAULT_TASK, EventType.TaskCreated, DateTime.Parse("1/1/2025"))
                }
            },

            new object[]
            {
                new List<TaskEvent>
                {
                    new TaskEvent(guid, "Any Task", EventType.TaskCreated, DateTime.Parse("1/1/2025")),
                    new TaskEvent(guid, DEFAULT_TASK, EventType.TaskUpdated, DateTime.Parse("1/2/2025"))
                }
            }
        };
    }
}

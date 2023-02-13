using Common;

namespace Models
{
  public class ToDoTaskModels
  {
    public class TaskDto
    {
      public Guid Id { get; set; }
      public DateTimeOffset Day { get; set; }
      public string Description { get; set; }
      public TaskState State { get; set; }
      public TaskPriority Priority { get; set; }
    }

    public class TaskAddRequest
    {
      public DateTimeOffset Day { get; set; }
      public string Description { get; set; }
      public TaskPriority Priority { get; set; }
    }

    public class TaskUpdateRequest
    {
      public Guid Id { get; set; }
      public DateTimeOffset Day { get; set; }
      public string Description { get; set; }
      public TaskState State { get; set; }
      public TaskPriority Priority { get; set; }
    }

    public class TasksPerDay
    {
      public string Day { get; set; }
      public string DayName { get; set; }
      public TaskDto[] Tasks { get; set; }
    }
  }
}
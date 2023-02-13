using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.ToDoTaskModels;

namespace Data.Data
{
  public class ToDoTask
  {
    [Key]
    public Guid Id { get; set; }
    public DateTimeOffset Day { get; set; }
    public string Description { get; set; }
    public TaskState State { get; set; }
    public TaskPriority Priority { get; set; }

    public void AddTask(TaskAddRequest request)
    {
      Id = new Guid();
      Day = request.Day;
      Description = request.Description;
      Priority = request.Priority;
      State = TaskState.ToDo;
    }

    public void UpdateTask(TaskUpdateRequest request)
    {
      Day = request.Day;
      Description = request.Description;
      Priority = request.Priority;
      State = request.State;
    }
  }
}

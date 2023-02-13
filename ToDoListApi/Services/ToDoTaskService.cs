using Data;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static Models.ToDoTaskModels;

namespace ToDoListApi.Services
{
  public class ToDoTaskService
  {
    private readonly ToDoListDbContext _context;

    public ToDoTaskService(ToDoListDbContext context)
    {
      _context = context;
    }

    public async Task<List<TasksPerDay>> GetTeamTasks(JsonElement search, DateTime endDate, DateTime startDate)
    {
      var tasks = _context.ToDoTask.AsQueryable();
      var query = BuildSearchClause(search, tasks);
      var result = await query.ToListAsync();

      var tasksPerDay = new List<TasksPerDay>();
      var days = (endDate - startDate).Days;
      for (int i = 0; i <= days; i++)
      {
        var date = startDate.AddDays(i).Date;

        var tasksDto = result.Where(t => t.Day.Date == date).Select(t => Map(t)).ToArray();

        tasksPerDay.Add(new TasksPerDay()
        {
          Day = date.ToString("yyyy-MM-dd"),
          DayName = date.ToString("ddd"),
          Tasks = tasksDto,
        });
      }

      return tasksPerDay;
    }

    private IQueryable<ToDoTask> BuildSearchClause(JsonElement search, IQueryable<ToDoTask> query)
    {

      foreach (var jp in search.EnumerateObject())
      {
        if (jp.Name == "from" && DateTimeOffset.TryParse(jp.Value.GetString(), out DateTimeOffset dt1))
        {
          query = query.Where(t => t.Day.Date >= dt1.Date);
        }
        else if (jp.Name == "to" && DateTimeOffset.TryParse(jp.Value.GetString(), out DateTimeOffset dt2))
        {
          query = query.Where(t => t.Day.Date <= dt2.Date);
        }
        else if (jp.Name == "states" && jp.Value.ValueKind == JsonValueKind.Array && jp.Value.GetArrayLength() > 0)
        {
          var states = jp.Value.EnumerateArray().Select(je => je.GetInt32());
          query = query.Where(t => states.Contains((int)t.State));
        }
        else if (jp.Name == "priorities" && jp.Value.ValueKind == JsonValueKind.Array && jp.Value.GetArrayLength() > 0)
        {
          var priorities = jp.Value.EnumerateArray().Select(je => je.GetInt32());
          query = query.Where(t => priorities.Contains((int)t.Priority));
        }
      }
      return query;
    }

    public async Task<TaskDto> AddTask([FromBody] TaskAddRequest request)
    {
      var task = new ToDoTask();
      task.AddTask(request);
      _context.ToDoTask.Add(task);
      await _context.SaveChangesAsync();

      var result = Map(task);

      return result;
    }

    public async Task<TaskDto> UpdateTask([FromBody] TaskUpdateRequest request)
    {
      var task = await _context.ToDoTask.SingleOrDefaultAsync(t => t.Id == request.Id);
      if (task == null)
        throw new InvalidOperationException("task_not_found");

      task.UpdateTask(request);
      await _context.SaveChangesAsync();

      var result = Map(task);

      return result;
    }

    public async Task DeleteTask([FromRoute] Guid id)
    {
      var task = await _context.ToDoTask.SingleOrDefaultAsync(t => t.Id == id);
      if (task == null)
        throw new InvalidOperationException("task_not_found");

      _context.ToDoTask.Remove(task);
      await _context.SaveChangesAsync();
    }

    private TaskDto Map(ToDoTask task)
    {
      var result = new TaskDto();
      result.Id = task.Id;
      result.Day = task.Day.Date;
      result.Priority = task.Priority;
      result.State = task.State;
      result.Description = task.Description;

      return result;
    }
  }
}

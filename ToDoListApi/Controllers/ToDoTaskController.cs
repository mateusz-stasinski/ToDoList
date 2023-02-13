using Data;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ToDoListApi.Services;
using static Models.ToDoTaskModels;

namespace ToDoListApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ToDoTaskController : ControllerBase
  {
    private readonly ToDoTaskService _tasks;

    public ToDoTaskController(ToDoTaskService tasks)
    {
      _tasks = tasks;
    }

    [HttpPost("find")]
    public async Task<ActionResult<List<TasksPerDay>>> GetTeamTasks([FromBody] JsonElement search)
    {
      var isStartDateValid = DateTime.TryParse(search.GetProperty("from").GetString(), out DateTime startDate);
      var isEndDateValid = DateTime.TryParse(search.GetProperty("to").GetString(), out DateTime endDate);
      if (!isStartDateValid || !isEndDateValid)
      {
        return BadRequest("arguments-from-and-to-are-required");
      }

      var tasksPerDay = await _tasks.GetTeamTasks(search, endDate, startDate);

      return Ok(tasksPerDay);
    }

    [HttpPost("add")]
    public async Task<ActionResult<TaskDto>> AddTask([FromBody] TaskAddRequest request)
    {
      var result = await _tasks.AddTask(request);

      return Ok(result);
    }

    [HttpPut("update")]
    public async Task<ActionResult<TaskDto>> UpdateTask([FromBody] TaskUpdateRequest request)
    {
      TaskDto result;
      try
      {
        result = await _tasks.UpdateTask(request);
      }
      catch
      {
          return NotFound("task_not_found");
      }

      return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteTask([FromRoute] Guid id)
    {
      try
      {
       await _tasks.DeleteTask(id);
      }
      catch
      {
        return NotFound("task_not_found");
      }

      return NoContent();
    }
  }
}

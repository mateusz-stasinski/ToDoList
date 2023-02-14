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
      List<TasksPerDay> tasksPerDay;
      try
      {
        tasksPerDay = await _tasks.GetTeamTasks(search);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

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
      catch (Exception ex)
      {
          return NotFound(ex.Message);
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
      catch (Exception ex)
      {
        return NotFound(ex.Message);
      }

      return NoContent();
    }
  }
}

using Common;
using Data;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ToDoListApi.Controllers;
using ToDoListApi.Services;
using Xunit;
using static Models.ToDoTaskModels;

namespace IntegrationTests
{
  [Collection("Database collection")]
  public class ToDoListIntegrationTests
  {
    private readonly IntegrationTestsSetup _setup;
    private readonly ToDoTaskController _taskController;
    private readonly ToDoTaskService _taskService;

    public ToDoListIntegrationTests(IntegrationTestsSetup setup)
    {
      _setup = setup;
      _taskService = new ToDoTaskService(CreateDbContext());
      _taskController = new ToDoTaskController(_taskService);
    }


    [Fact]
    public async Task ShouldAddNewTask()
    {
      var newTask = new TaskAddRequest()
      {
        Day = DateTimeOffset.Now.Date,
        Priority = TaskPriority.Medium,
        Description = "Pierwsze zadanie testowe"
      };

      var task = await _taskController.AddTask(newTask);
      var taskResult = task.Result as OkObjectResult;
      var addedTask = (TaskDto)taskResult.Value;

      Assert.NotNull(addedTask);
      Assert.Equal(newTask.Day, addedTask.Day);
      Assert.Equal(newTask.Priority, addedTask.Priority);
      Assert.Equal(newTask.Description, addedTask.Description);
      Assert.Equal(TaskState.ToDo, addedTask.State);
    }

    [Fact]
    public async Task ShouldUpdateTask()
    {
      var newTask = new TaskAddRequest()
      {
        Day = DateTimeOffset.Now.Date,
        Priority = TaskPriority.Medium,
        Description = "Zadanie do zmiany"
      };

      var task = await _taskController.AddTask(newTask);
      var taskResult = task.Result as OkObjectResult;
      var addedTask = (TaskDto)taskResult.Value;

      var updateTaskRequest = new TaskUpdateRequest()
      {
        Id = addedTask.Id,
        Day = DateTimeOffset.Now.AddDays(5).Date,
        Priority = TaskPriority.High,
        Description = "Zadanie po zmianie",
        State = TaskState.Doing,
      };
      var updateTask = await _taskController.UpdateTask(updateTaskRequest);
      var updatedResult = updateTask.Result as OkObjectResult;
      var updatedTask = (TaskDto)updatedResult.Value;

      Assert.NotNull(updatedTask);
      Assert.Equal(updateTaskRequest.Day, updatedTask.Day);
      Assert.Equal(updateTaskRequest.Priority, updatedTask.Priority);
      Assert.Equal(updateTaskRequest.Description, updatedTask.Description);
      Assert.Equal(updateTaskRequest.State, updatedTask.State);
    }

    [Fact]
    public async Task ShouldDeleteTask()
    {
      var newTask = new TaskAddRequest()
      {
        Day = DateTimeOffset.Now.Date,
        Priority = TaskPriority.Medium,
        Description = "Zadanie do zmiany"
      };

      var task = await _taskController.AddTask(newTask);
      var taskResult = task.Result as OkObjectResult;
      var addedTask = (TaskDto)taskResult.Value;

      await _taskController.DeleteTask(addedTask.Id);

      ToDoTask searchTask;
      using (var context = CreateDbContext())
      {
        searchTask = await context.ToDoTask.SingleOrDefaultAsync(t => t.Id == addedTask.Id);
      }

      Assert.Null(searchTask);
    }

    private ToDoListDbContext CreateDbContext()
    {
      var optionsBuilder = new DbContextOptionsBuilder<ToDoListDbContext>();
      optionsBuilder.UseSqlServer(_setup.ConnectionString);
      return new ToDoListDbContext(optionsBuilder.Options);
    }
  }
}
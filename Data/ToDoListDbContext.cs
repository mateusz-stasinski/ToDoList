using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Data
{
  public class ToDoListDbContext : DbContext
  {
    public ToDoListDbContext() { }

    public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : base(options)
    {
    }

    public DbSet<ToDoTask> ToDoTask { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
  }
}
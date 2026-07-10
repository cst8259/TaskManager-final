using Microsoft.EntityFrameworkCore;
using TaskManager.DataAccess;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<TaskManagerContext>(options => 
  options.UseSqlite("Data Source=TaskManager.sqlite"));
  
var app = builder.Build();

app.MapControllers();

app.Run();

using System.Text.Json.Serialization;
using Strider.Posterr.RelationalData.Extensions;
using Strider.Posterr.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//register data project dependencies
var isTesting = Convert.ToBoolean(builder.Configuration["IsTesting"] ?? "false");
builder.Services.RegisterDataServices(builder.Configuration["ConnectionStrings:Default"], isTesting);

//register service project dependencies
builder.Services.RegisterServices();

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault; });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using var serviceScope = ((IApplicationBuilder)app).ApplicationServices.CreateScope();
serviceScope.ServiceProvider.Migrate(isTesting);

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
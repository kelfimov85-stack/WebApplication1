using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ← ЭТО ОБЯЗАТЕЛЬНО ДОБАВИТЬ:
builder.Services.AddDbContext<DeliveryDbContext>(opt =>
    opt.UseInMemoryDatabase("DeliveryDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Drink.API;
using Drink.API.Clients;
using Drink.API.Controllers;
using Drink.API.Extensions;
using Drink.Database.Contexts;
using Drink.Database.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions<Config>()
                .Bind(builder.Configuration.GetSection("Config"));
builder.Services.AddHttpClient("ContentClient");

builder.Services.AddSingleton<IContentController, ContentController>();

builder.Services.AddDbContext<DishDrinkContext>(
options => options.UseSqlServer(
builder.Configuration.GetConnectionString("DrinkConnection")));
//TODO change to Scoped when removing contentClient
builder.Services.AddSingleton<IDbService, DbService>();
//TODO remove IController when no longer needed
builder.Services.AddSingleton<IContentClient, ContentClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
        .WithOrigins("http://localhost:3001")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

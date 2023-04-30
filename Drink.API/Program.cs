using AutoMapper;
using Drink.API;
using Drink.API.Clients;
using Drink.API.Controllers;
using Drink.API.Services;
using Drink.Common.DTOs;
using Drink.Database.Contexts;
using Drink.Database.Entities;
using Drink.Database.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions<Config>()
                .Bind(builder.Configuration.GetSection("Config"));
builder.Services.AddHttpClient("ContentClient");

builder.Services.AddDbContext<DishDrinkContext>(
options => options.UseSqlServer(
builder.Configuration.GetConnectionString("DrinkConnection")));
ConfigureAutomapper(builder.Services);
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddSingleton<IContentClient, ContentClient>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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

static void ConfigureAutomapper(IServiceCollection services)
{
    var config = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<Cuisine, CuisineDTO>().ReverseMap();
        cfg.CreateMap<Dish, DishDTO>().ReverseMap();
        cfg.CreateMap<DishCuisine, DishCuisineDTO>().ReverseMap();
        cfg.CreateMap<DishDishType, DishDishTypeDTO>().ReverseMap();
        cfg.CreateMap<DishDrink, DishDrinkDTO>().ReverseMap();
        cfg.CreateMap<DishType, DishTypeDTO>().ReverseMap();
        cfg.CreateMap<Drink.Database.Entities.Drink, DrinkDTO>().ReverseMap();

    });
    var mapper = config.CreateMapper();
    services.AddSingleton(mapper);
}
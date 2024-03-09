using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectP.Data;
using ProjectP.Data.Entities.Identity;
using ProjectP.Entities;
using ProjectP.Extensions;
using ProjectP.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddSwaggerAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");


app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var roleContext = services.GetRequiredService<RoleManager<AppRole>>();
    var userContext = services.GetRequiredService<UserManager<AppUser>>();
    var mapper = services.GetRequiredService<IMapper>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, roleContext, userContext, mapper);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

app.UseAuthorization();

app.MapControllers();

app.Run();
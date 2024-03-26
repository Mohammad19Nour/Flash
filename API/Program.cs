using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectP.Data;
using ProjectP.Data.Entities.Identity;
using ProjectP.Extensions;
using ProjectP.Helpers;
using ProjectP.Helpers.Converters;
using ProjectP.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5098");
// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.Converters.Add(new RoundedNumberConverter(2));
    opt.SerializerSettings.Converters.Add(new DateTimeConverter());
    
    opt.SerializerSettings.Converters.Add(new PictureUrlConverter(builder.Configuration["ApiUrl"]??""));
    //   opt.JsonSerializerOptions.Converters.Add();
});
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed((_) => true);
}));
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
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed((_) => true));
app.Run();
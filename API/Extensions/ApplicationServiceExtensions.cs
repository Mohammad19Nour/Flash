using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using ProjectP.Data;
using ProjectP.Data.Entities;
using ProjectP.Data.Repositories;
using ProjectP.Dtos.HotelDtos;
using ProjectP.Errors;
using ProjectP.Helpers.Profiles;
using ProjectP.Interfaces;
using ProjectP.Services;

namespace ProjectP.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<KestrelServerOptions>(options => { options.Limits.MaxRequestBodySize = null; });

        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<IOfferService, OfferService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<ITouristPlacesService, TouristPlacesService>();
        services.AddScoped<IRoomTypeService, RoomTypeService>();
        
        
        services.AddAutoMapper(typeof(RoomTypeProfile),typeof(TouristPlacesProfile),typeof(OffersProfile),typeof(HotelsProfile),typeof(Profiles));
        
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
          services.AddDbContext<DataContext>(opt => { opt.UseSqlServer(config.GetConnectionString("DefaultConnection")); });

       // services.AddDbContext<DataContext>(opt => { opt.UseSqlite(config.GetConnectionString("SqliteConnection")); });

        services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        services.Configure<ApiBehaviorOptions>(opt =>
        {
            opt.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(y => new
                    {
                        Field = x.Key,
                        Message = y.ErrorMessage
                    }))
                    .ToArray();

                var tmp = errors.Select(c => $"{c.Field}: {c.Message}").ToArray();
                                var re = string.Join(", ", tmp);
                
                                return new OkObjectResult(new ApiResponse(400,re));
            };
        });
        return services;
    }
}
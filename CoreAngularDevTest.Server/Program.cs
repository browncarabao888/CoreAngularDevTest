using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using CoreAngularDevTest.Server.Models;
using CoreAngularDevTest.Server.Services;
using CoreAngularDevTest.Server.Interfaces;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

      
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite("Data Source=devtest.db"));

        
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(System.Net.IPAddress.Parse("192.168.100.9"), 5000); // HTTP
            options.Listen(System.Net.IPAddress.Parse("192.168.100.9"), 5001, listenOptions =>
            {
                listenOptions.UseHttps();  
            });
        });

       
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:60083")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();

            });
        });

        builder.Services.AddScoped<IUser,UserService>();
        builder.Services.AddScoped<IFoursquareService, FoursquareService>();
        builder.Services.AddScoped<IFlickr, FlickrService>();
        builder.Services.AddScoped<IGoogleService, GoogleService>();


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient();

        var app = builder.Build();

       
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
       
        app.UseHttpsRedirection();
        app.UseCors(); 
        app.UseAuthorization();
         
        app.UseStaticFiles();
        app.MapControllers();
        app.Run();
    }
}

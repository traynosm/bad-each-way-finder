using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using bad_each_way_finder.Areas.Identity.Data;
using bad_each_way_finder.Settings;
using System.Configuration;
using bad_each_way_finder.Interfaces;
using bad_each_way_finder.Services;

namespace bad_each_way_finder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration
                .GetConnectionString("BadEachWayFinder") ?? 
                throw new InvalidOperationException("Connection string 'BadEachWayFinder' not found.");

            builder.Services.AddDbContext<BadEachWayFinderContext>(options => 
                options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => 
                options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<BadEachWayFinderContext>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddHttpClient<IApiService, ApiService>();

            builder.Services.Configure<ApiSettings>(o =>
                builder.Configuration.GetSection("ApiSettings").Bind(o));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
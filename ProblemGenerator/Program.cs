using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ProblemGenerator.Controllers;
using ProblemGenerator.Data;


namespace ProblemGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            
            builder.Services.AddHttpClient();
            builder.Services.AddSqlite<ProblemContext>("Data Source=problems.db");
            builder.Services.AddDbContext<ProblemContext>();
            builder.Services.AddScoped<ProblemServices>();


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

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

             
            // Initialize the database
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ProblemContext>();
                if (db.Database.EnsureCreated())
                {
                    SeedData.Initialize(db);
                }
            }
                      
            app.Run();
        }
    }
}
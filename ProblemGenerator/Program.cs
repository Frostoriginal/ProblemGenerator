using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ProblemGenerator.Controllers;
using ProblemGenerator.Data;
using ProblemGenerator.ForQuartz;
using Quartz;

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

			builder.Services.AddSingleton<MyLogger>();

			builder.Services.AddControllers();
            builder.Services.AddLocalization();

            builder.Services.AddScoped<IAddRecurrentTasks, AddRecurrentTasks>();
            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();
                // Just use the name of your job that you created in the Jobs folder.
                var jobKey = new JobKey("AddRecurrentTasksJob");
                q.AddJob<AddRecurrentTasksJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("AddRecurrentTasksJob-trigger")
                    //This Cron interval can be described as "run every minute" (when second is zero)
                    .WithCronSchedule("* * * ? * *") // "* * * ? * *" every second, "0 * * ? * *" every minute, "0 0 0 * * ?" 	Every day at midnight - 12am
                );
            });
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            
            var app = builder.Build();

            var supportedCultures = new[] { "en-US", "pl-PL" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[1]) //0 for en-us, 1 for pl-PL
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            app.UseRequestLocalization(localizationOptions);


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
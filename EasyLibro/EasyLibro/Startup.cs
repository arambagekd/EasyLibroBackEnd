using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Quartz;
using Buisness_Logic_Layer.AuthHelpers;
using Buisness_Logic_Layer.Interfaces;
using Buisness_Logic_Layer.Services;
using Data_Access_Layer;
using Buisness_Logic_Layer.BackgroundJobs;
using Microsoft.EntityFrameworkCore.Design;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();  // Make sure this is included
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        ConfigureDependencyInjection(services);
        ConfigureAuthentication(services);
        ConfigureQuartz(services);

        services.AddAuthorization();
        services.AddCors();

        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DataContext")));

        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "serviceAccountKey.json")),
        });
    }

    // Separate method for Dependency Injection configuration
    private void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IResourceService, ResourceService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IRequestService, RequestService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<JWTService>();
        services.AddScoped<RefreshTokenService>();
    }

    // Separate method for Authentication configuration
    private void ConfigureAuthentication(IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is a very secure key for me")),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    // Separate method for Quartz configuration
    private void ConfigureQuartz(IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            // Use a Scoped container to create jobs.
            q.UseMicrosoftDependencyInjectionJobFactory();

            // Create a job
            var weeklyJob = new JobKey("WeekJob");
            q.AddJob<WeeklyJob>(opts => opts.WithIdentity(weeklyJob));

            // Create a trigger for the job
            q.AddTrigger(opts => opts
                .ForJob(weeklyJob)
                .WithIdentity("WeekJob-trigger")
                .WithCronSchedule("0 0 0 ? * MON")); // Every Monday 12 AM

            var dailyJobKey = new JobKey("DailyJob");

            // Register the daily job with the DI container
            q.AddJob<DailyJob>(opts => opts.WithIdentity(dailyJobKey));

            // Create a trigger for the daily job to run every day at 12:00 AM
            q.AddTrigger(opts => opts
                .ForJob(dailyJobKey)
                .WithIdentity("DailyJob-trigger")
                .WithCronSchedule("0 0 0 * * ?"));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(options => options
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
        );

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseRouting();  // Ensure routing is added

        app.UseEndpoints(endpoints =>  // Map controllers correctly
        {
            endpoints.MapControllers();
        });
    }
}

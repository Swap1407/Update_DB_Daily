using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using Update_DB_Daily.ServiceLayer;
using Update_DB_Daily.Data;
using Update_DB_Daily.Models;

var builder = WebApplication.CreateBuilder(args);
//master branch commit
//hangfire
var HangFireTaskConnectionString = builder.Configuration.GetConnectionString("HangFireConnectionString");
builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(HangFireTaskConnectionString));
builder.Services.AddHangfireServer();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProjectDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectsConnectionString")));

builder.Services.Configure<MailModel>(builder.Configuration.GetSection("MailModel"));

builder.Services.AddScoped<IReadFileData, ReadFileData>();
builder.Services.AddScoped<IProjectRepository, ProjectDBRepository>();
builder.Services.AddTransient<IMailService, MailService>();

var configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
var section = configuration.GetSection("MailModel");
var mailModelConfiguration = section.Get<MailModel>();
builder.Services.AddSingleton(mailModelConfiguration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHangfireDashboard();

var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());


//builder.Configuration.LoggerFactory.AddFile(@"C:\Users\swapnil.siddheshwar\Documents\C# Tasks\Log.txt");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard();
});

app.Run();

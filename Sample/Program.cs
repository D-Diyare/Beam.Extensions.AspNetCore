using Beam.Extensions.AspNetCore;
using Beam.Extensions.AspNetCore.Sample.Data.Context;
using Beam.Extensions.AspNetCore.Sample.Data.Entities;
using Beam.Extensions.AspNetCore.Sample.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Adding localization ( you can also provide the directory for the .resex files by
// default it's "Resources".
builder.Services.AddLocalizationResource()
// Configure localization.
    .ConfigureLocalization(cultures: new List<string>
    {
        "en", // Add english to localization
        "es", // Add spanish to localization
        "de", // Add german to localization
        "ar"  // Add arabic to localization
    }, mainCulture: "en" /* Main language will be english */);

// Add services to the container.
builder.Services.AddControllersWithViews()
    // We need this line to add localization for the views. 
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    ;

// Registering DbContext as scoped (this can be chained as it resturns IServiceCollection).
builder.Services.RegisterDbContext<SampleContext>(
    configuration: builder.Configuration,
    connectionStringName: "Development",
    timeOut: 30);

// Add JWT identity service.
builder.Services.AddJwtService()
// Add default identity service (you can also configure the identity options inside).
    .AddIdentityService<SampleContext, ApplicationUser, Role, Guid>()
// Configuring TokenLifeSpan
    .ConfigureDataProtection(TimeSpan.FromMinutes(30));

// Add policy authorizations
builder.Services.AddAuthorizations(new Dictionary<string, string[]>
{
    {
        Constants.AdminPolicy,
        new []
        {
            Constants.AdminRole,
            Constants.SuperAdminRole
        }
    },
    {
        Constants.EmployeePolicy,
        new []
        {
            Constants.EmployeeRole
        }
    }
});

// Registering response compression (you can also configure the compression inside).
// the compression providers used are: Brotli & Gzip
builder.Services.RegisterResponseCompression()
// Configuring the compression ( you can also configure the compression level)
    .ConfigureResponseCompression();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Need this middleware to be able to use the resoponse compression.
app.UseResponseCompression();

// Need this middleware to be able to use the localization.
app.UseLocalization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

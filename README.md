# Beam.Extensions.AspNetCore

A Set of ASP.NET Core 7 extensions to register DbContext, identity, JWT, localization, compression...

> These are some useful methods that I use on my regular projects.

# Add & Register & Configure Services

## Adding Localization

```cs
// Adding localization ( you can also provide the directory for the .resex files by
// default is "Resources".
builder.Services.AddLocalizationResource()
// Configure localization ( adding languages to the app).
    .ConfigureLocalization(cultures: new List<string>
    {
        "en",
        "es",
        "de",
        "ar"
    }, mainCulture: "en" /* Main language will be english */);


```
## Razor Pages Or MVC
While you use razor pages or mvc you also need to add the following to be able to use localization for the views.

```cs
builder.Services.AddControllersWithViews()
    // We need this line to add localization for the views. 
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix);
```

## Registering DbContext
The Current registration uses scoped lifetime cycle to register the DbContext.
Store the connectionString in appsettings.json and provide the name in connectionString name as showed below as "Development",
you also need to provide the time out time (in seconds) as showed as 30.

```cs
builder.Services.RegisterDbContext<SampleContext>(
    configuration: builder.Configuration,
    connectionStringName: "Development",
    timeOut: 30);
```
## Add JWT Bear Token As Identity Service
You can add JWT bearer token identity by adding following.

```cs
builder.Services.AddJwtService();
```

Default options are :

```cs 
ValidateIssuer = false,
ValidateAudience = false,
RequireExpirationTime = true,
ClockSkew = TimeSpan.Zero
```
You can also configure the options:

```cs
builder.Services.AddJwtService(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters(
        {
            ValidateIssuer = false,
            ValidateAudience = true,
            ...
        };
    });
```
## Add Identity
You can add identity service using extended identity users and roles.

```cs
builder.Services.AddIdentityService<SampleContext, ApplicationUser, Role, Guid>();
```
Default identity options are :
```cs
Password.RequireDigit = true;
Password.RequiredLength = 5;
Password.RequireUppercase = true;
Password.RequireLowercase = true;
Password.RequireNonAlphanumeric = false;
SignIn.RequireConfirmedEmail = true;
User.RequireUniqueEmail = true;
Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
Lockout.MaxFailedAccessAttempts = 5;
```

You can also configure the identity options:

```cs
builder.Services.AddIdentityService<SampleContext, ApplicationUser, Role, Guid>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 10;
        ...
    });
```
### Set TokeLifeSpan
Set token life span to 30 minutes.

```cs
builder.Services.ConfigureDataProtection(TimeSpan.FromMinutes(30));
```

## Add Policy Authorizations
You can add the policy based authorizations easily using the method bellow

```cs
builder.Services.AddAuthorizations(new Dictionary<string, string[]>
{
    {
    
        "Admin" /* Policy name */,
        new []
        {
            "Administrator",
            "SuperAdmin"
        } /* Required roles to meet the policy. */
    },
    {
        "Employee" /* Policy name */,
        new []
        {
            "Employee"
        } /* Required roles to meet the policy. */
    }
});
```
## Add Response Compression Using (Brotli & GZip)

#### - Registering Response Compression

```cs
builder.Services.RegisterResponseCompression();
```

The MimeTypes used to compression can be found [Here](https://github.com/D-Diyare/Beam.Extensions.AspNetCore/blob/master/Lib/Helpers/MimeTypes.cs)

You can also configure the registration of response compression

```cs
builder.Services.RegisterResponseCompression(options =>
    {
        options.EnableForHttps = true;
            ...
    });
```

#### - Configure Response Compression Providers

```cs
builder.Services.ConfigureResponseCompression();
```

Default : <br>
Brotli Compression = Fastest <br>
GZip Compression = SmallestSize

You can also configure the response compression providers

```cs
builder.Services.ConfigureResponseCompression<BrotliCompressionProviderOptions,GzipCompressionProviderOptions>(brotliCompressionLevel: CompressionLevel.Fastest,
        gzipCompressionLevel: CompressionLevel.SmallestSize);
```
# Using (Middlewares)

## Use Response Compression
Use the bellow middleware to to be able to use response compression.

```cs
app.UseResponseCompression();
```

## Use Localization
Use the bellow middleware to to be able to use localization.

```cs
app.UseLocalization();
```
## Use Authentication (Identity)
> Note: Authentication middleware must come before authorization.
> 
```cs
app.UseAuthentication();
app.UseAuthorization();
```
## Fluent?
All the methods return IServiceCollection which means you can chain the methods (better to only chain related methods),
you can find the sample to see the whole implementation.
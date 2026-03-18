using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using SocialNetworkMicroservices.Identity.Data;
using SocialNetworkMicroservices.Identity.Models;
using SocialNetworkMicroservices.Identity.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Social Network - Identity API",
        Version = "v1",
        Description = "Identity microservice for the Social Network application. Provides OAuth 2.0/OpenID Connect authentication endpoints using OpenIddict.",
        Contact = new OpenApiContact
        {
            Name = "Social Network API Support",
            Email = "support@socialnetwork.com"
        }
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("/connect/token", UriKind.Relative),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "Access to admin features" }
                }
            }
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Register the DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("goingmysocial-identity-db");
    options.UseNpgsql(connectionString);
    options.UseOpenIddict();
});

// Register OpenIddict services
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<ApplicationDbContext>();
    })
    .AddServer(options =>
    {
        // Set the issuer
        options.SetIssuer(new Uri(builder.Configuration["OpenIddict:Issuer"] ?? "https://localhost:7001"));

        // Configure OpenIddict server options
        /*var encryptionKey = builder.Configuration["OpenIddict:Key"] ?? throw new Exception("OpenIddict key is not configured.");
        options.AddEncryptionKey(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey)));*/
        options.SetAccessTokenLifetime(TimeSpan.FromMinutes(int.Parse(builder.Configuration["OpenIddict:AccessTokenLifetime"]!)));
        options.SetRefreshTokenLifetime(TimeSpan.FromMinutes(int.Parse(builder.Configuration["OpenIddict:RefreshTokenLifetime"]!)));

        // Enable the token endpoint
        options.SetTokenEndpointUris("/connect/token");
        options.SetAuthorizationEndpointUris("/connect/authorize");
        options.SetUserinfoEndpointUris("/connect/userinfo");

        // Enable the flows
        options.AllowPasswordFlow();
        options.AllowClientCredentialsFlow();
        options.AllowRefreshTokenFlow();
        options.AllowAuthorizationCodeFlow();

        // Register the signing and encryption credentials
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        // Register the ASP.NET Core host and configure the ASP.NET Core options
        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough()
               .EnableAuthorizationEndpointPassthrough()
               .EnableUserinfoEndpointPassthrough();

        options.DisableAccessTokenEncryption();

        options.RegisterScopes("social_api", "email", "profile", "roles", "openid");
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

builder.Services.AddControllers();

// Register ASP.NET Core Identity services
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;

    // User settings
    options.User.RequireUniqueEmail = true;

    // SignIn settings (optional)
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// Register custom services
builder.Services.AddScoped<IUserService, UserService>();

// Build the app
var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    await OpenIddictSeeder.SeedAsync(scope.ServiceProvider);
    await UserSeeder.SeedUsersAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

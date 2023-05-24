using System.Security.Claims;
using Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusicAppApi.Core;
using MusicAppApi.Core.Configuration;
using MusicAppApi.Core.Filters;
using MusicAppApi.Core.Services;
using MusicAppApi.Models.Configurations;
using MusicAppApi.Models.DbModels;
using System.Text;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using MusicAppApi.API.Hub;
using MusicAppApi.Core.Constants;
using MusicAppApi.Core.Interfaces;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers((options) =>
{
    // model validation
    options.Filters.Add(new ValidateModelFilter());

    // badrequest and unauthorized formatterw
    options.Filters.Add(new HttpResponseFilter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.WebHost.ConfigureAppConfiguration((context, config) =>
{
    var env = context.HostingEnvironment;

    var builtConfiguration = config.Build();

    string kvUrl = builtConfiguration[$"KeyVaultConfig:{env.EnvironmentName}:KVUrl"];
    string tenantId = builtConfiguration[$"KeyVaultConfig:{env.EnvironmentName}:TenantId"];
    string clientId = builtConfiguration[$"KeyVaultConfig:{env.EnvironmentName}:ClientId"];
    string clientSecret = builtConfiguration[$"KeyVaultConfig:{env.EnvironmentName}:ClientSecretId"];

    var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

    var client = new SecretClient(new Uri(kvUrl), credential);
    config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
});
builder.Services.AddCors(opt =>
{
    var frontUrl = builder.Configuration.GetValue<string>("front-url");

    opt.AddDefaultPolicy(builderCors =>
    {
        builderCors.WithOrigins("http://localhost:5173", frontUrl) // Replace 'frontUrl' with the actual allowed origin
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
    });
});

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<MusicAppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<User, Role, MusicAppDbContext, Guid>>()
    .AddRoleStore<RoleStore<Role, MusicAppDbContext, Guid>>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(1);
});

builder.Services.AddTransient<IJwtGenerator, JwtGenerator>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IAzureBlobService, AzureBlobService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(opts =>
               {
                   JWTConfiguration jwtConfiguration = new JWTConfiguration();
                   builder.Configuration.Bind("JWT", jwtConfiguration);

                   opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = jwtConfiguration.Issuer,
                       ValidAudience = jwtConfiguration.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.AccessTokenSecret)),
                       ClockSkew = TimeSpan.Zero,
                   };
               });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthConstants.Policies.RequireAdmin, policy => policy
        .RequireRole(AuthConstants.UserRoles.Admin));

    options.AddPolicy(AuthConstants.Policies.RequireArtistAndAdmin, policy => policy
        .RequireRole(AuthConstants.UserRoles.Artist)
        .RequireRole(AuthConstants.UserRoles.Admin));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your JWT token in the format 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
});

builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

builder.Services.AddDbContext<MusicAppDbContext>(opts =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    opts.UseSqlServer(connStr);
});

builder.Services.RegisterCoreDependencies();

var app = builder.Build();

// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<MusicAppDbContext>();

    // Here is the migration executed
    dbContext.Database.Migrate();
};


app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapHub<TrackCountHub>("/trackCount");

app.MapControllers();

app.Run();
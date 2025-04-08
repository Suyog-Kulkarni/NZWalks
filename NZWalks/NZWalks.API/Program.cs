using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.File("Logs/NzWalks_logs.txt", rollingInterval: RollingInterval.Minute)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders(); // this is used to clear the default logging providers
builder.Logging.AddSerilog(logger); // this is used to add the serilog logger to the logging providers

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor(); // this is used to access the httpcontext in the repository classes and it is used to get the user id from the token in the repository classes
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// add authorization UI to swagger 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "NZWalks.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
            },
            new List<string>()

        }
    });
});

builder.Services.AddDbContext<NZWalksDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("NZWalksConnectionString")
));

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")
    )
);

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddIdentityCore<IdentityUser>() // this lines tells the application to use the IdentityUser class for the identity 
    .AddRoles<IdentityRole>() // identityrole is the class that is used to define the roles in the application 
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks") // dataprotectiontokenprovider is the class that is used to generate the token for the user and the string "NZWalks" is the name of the token provider 
    .AddEntityFrameworkStores<NZWalksAuthDbContext>() // this line tells the application to use the NZWalksAuthDbContext as the database for the identit
    .AddDefaultTokenProviders(); // this line tells the application to use the default token providers for the identity
// why is the code above necessary? 
// The code above is necessary because it tells the application to use the IdentityUser class for the identity and the IdentityRole class for the roles in the application
// It also tells the application to use the NZWalksAuthDbContext as the database for the identity and to use the default token providers for the identity.
// plus it tells the application to use the DataProtectorTokenProvider class to generate the token for the user and the string "NZWalks" is the name of the token provider

builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
    });
// this whole code block is used to configure the identity options for the application 
// it is overriding the default identity options for the application which are set in the IdentityOptions class 
// it is setting the password options for the application like RequireDigit, RequireLowercase, RequireNonAlphanumeric, RequireUppercase, RequiredLength, RequiredUniqueChars


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // this tells the bydefault authentication scheme to be JwtBearer
    .AddJwtBearer(options => // this is the configuration for the JwtBearer authentication scheme or it is a handler for the JwtBearer authentication scheme
    options.TokenValidationParameters = new TokenValidationParameters // TokenValidationParameters is a class that is used to configure the validation of the token
    {
        ValidateIssuer = true, // this tells the handler to validate the issuer of the token 
        ValidateAudience = true, // this tells the handler to validate the audience of the token
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true, // this tells the handler to validate the signing key of the token
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // value of the issuer is taken from the appsettings.json file
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey( // this is the key that is used to sign the token
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // encoding because the key is a string and it needs to be converted to bytes
        // symmetric key because the same key is used to sign and validate the token
    });
// the flow is like this:
// 1. the authentication middleware checks the Authorization header after the request is received
// 2. It extracts the JWT and uses the configured TokenValidationParameters to validate the token against the parameters
// 3. If the token is valid,  the middleware extracts the claims and creates a ClaimsPrincipal object and sets it on the HttpContext.User property 
// 4. The Authorize attribute checks if HttpContext.User is authenticated. If it's not, the request is rejected with 401 Unauthorized.
builder.Services.AddScoped<IRegionRepo, SQLRegionRepo>();
builder.Services.AddScoped<IWalksRepo, SQLWalksRepo>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();
var app = builder.Build();


// Once you've configured authentication in Program.cs, ASP.NET Core automatically hooks into
// the middleware pipeline and handles token validation for every request.
// The [Authorize] attribute just checks the result.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); //  Extracts the JWT from the Authorization header and validates it.
app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
    // this is used to serve the images from the Images folder in the root directory of the project
});
app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using interview_dotnet_api.Data;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var allowedOriginsString = builder.Configuration["Cors:AllowedOrigins"] ?? "http://localhost:3000";
var allowedOrigins = allowedOriginsString
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Add Database Context
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=notifications.db"));

// Configure Authentication Services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "NotificationIssuer",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "NotificationAudience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["Jwt:Key"] ?? "SuperSecretSecurityKeyThatIsLongEnoughToMeetRequirements32Chars!"))
    };
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Drop and recreate the database on startup so schema changes are picked up.
// NOTE: this wipes all data — intended for local/dev only, never production.
// Skip the wipe under `dotnet watch` so hot-reload restarts keep existing data.
var isHotReload = Environment.GetEnvironmentVariable("DOTNET_WATCH") == "1";
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
    if (!isHotReload)
    {
        await db.Database.EnsureDeletedAsync();
    }
    await db.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    // Serves the OpenAPI document at /openapi/v1.json
    app.MapOpenApi();

    // Interactive API reference UI (Swagger UI replacement) at /scalar
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Notification API")
            .WithTheme(ScalarTheme.Purple);
    });
}

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

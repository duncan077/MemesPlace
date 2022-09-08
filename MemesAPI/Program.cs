using MemesAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MemesAPI.Extension;
using AspNetCoreRateLimit;
using MemesAPI.BgService;
using System.Configuration;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "server=localhost;user=meme;password=meme;database=memesplace";
var serverVersion = ServerVersion.AutoDetect(connectionString);
// Add services to the container.
builder.Services.AddDbContext<AppDBContext>(options => options
                .UseMySql(connectionString, serverVersion)
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
builder.Services.AddIdentityCore<MemeUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>();
//builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddHostedService<TrendingService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
    };
});
builder.Services.AddAuthentication().AddGoogle("google", opt =>
{
    var googleAuth = builder.Configuration.GetSection("Authentication:Google");
    opt.ClientId = googleAuth["ClientId"];
    opt.ClientSecret = googleAuth["ClientSecret"];
    opt.SignInScheme = IdentityConstants.ExternalScheme;
});
builder.Services.ConfigureCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

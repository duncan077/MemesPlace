using MemesAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MemesAPI.Extension;

using MemesAPI.BgService;

using MemesAPI.Repository;

using MemesAPI.Repository.Interface;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);
var connectionString = "server=b7eq1ylon8jl9gx2yi3k-mysql.services.clever-cloud.com;Port=3306;user=ubeodcgjq8g5pogs;password=ixfwoisUNTkr2NTvcFVC;database=b7eq1ylon8jl9gx2yi3k";//Environment.GetEnvironmentVariable("ConnectionString");
var googleId=Environment.GetEnvironmentVariable("GoogleClientId");
var googleSecret = Environment.GetEnvironmentVariable("GoogleClientSecret");
// Add services to the container.
builder.Services.AddDbContext<AppDBContext>(options => options
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
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
builder.Services.AddScoped<IFileRepository, ImgurRepository>();

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
}).AddGoogle("google", opt =>
{
    
    opt.ClientId = googleId;
    opt.ClientSecret = googleSecret;
    opt.SignInScheme = IdentityConstants.ExternalScheme;
});
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseIpRateLimiting();
app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using AspNetCoreRateLimit;

namespace MemesAPI.Extension
{
    public static class ServiceExtensions
    {
       

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule> {
                 new RateLimitRule {
                     Endpoint = "*",
                     Limit= 60,
                     Period = "1m"
                 }
            };
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });
       
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
        public static void ConfigureCors(this IServiceCollection services) =>
 services.AddCors(options =>
 {
     options.AddPolicy("CorsPolicy", builder =>
     builder.AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());
 });
    }
}

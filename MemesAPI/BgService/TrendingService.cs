using MemesAPI.Data;

namespace MemesAPI.BgService
{
    public class TrendingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public TrendingService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           int refreshTimeMs = 1000*60; //How much delay you want
            while (!stoppingToken.IsCancellationRequested)
            {
                //call your function here
                
                await Task.Delay(refreshTimeMs*5, stoppingToken);
            }
            
        }
        private void SelectTrending()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider
           .GetRequiredService<AppDBContext>();
            var likes = context.MemeLike.ToList();
        }
        private void RemoveTrending()
        {

        }
    }
}

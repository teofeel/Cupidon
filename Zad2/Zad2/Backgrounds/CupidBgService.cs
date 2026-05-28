
using Zad2.Services.Interfaces;

namespace Zad2.Backgrounds
{
    public class CupidBgService : BackgroundService
    {
        private readonly ICupidService _cupidService;

        public CupidBgService(ICupidService cupidService)
        {
            this._cupidService = cupidService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    this._cupidService.SendLetter();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cupid BG Error: {ex.Message}");
                }

                await Task.Delay(60000, stoppingToken);
            }

        }
    }
}

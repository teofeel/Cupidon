
using Cupid.Services.Interfaces;

namespace Cupid.Backgrounds
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
                    Console.WriteLine($"[CupidBg] Running SendLetter at {DateTime.Now}");
                    await this._cupidService.SendLetter();
                    Console.WriteLine($"[CupidBg] SendLetter completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cupid BG Error: {ex}");
                }

                await Task.Delay(60000, stoppingToken);
            }

        }
    }
}

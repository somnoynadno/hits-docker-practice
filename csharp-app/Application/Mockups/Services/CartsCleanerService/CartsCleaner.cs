using Mockups.Configs;
using Mockups.Repositories.Carts;

namespace Mockups.Services.CartsCleanerService
{
    public class CartsCleaner : IHostedService, IDisposable
    {
        private Timer _timer = null!;
        private readonly CartsCleanerConfig _config;
        private readonly CartsRepository _rep;

        public CartsCleaner(CartsCleanerConfig config, CartsRepository rep)
        {
            _config = config;
            _rep = rep;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _rep.ClearCarts(_rep.GetInactiveCarts(_config.Time));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

using PNJGoldValue.Models;
using PNJGoldValue.Services;

namespace PNJGoldValue
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DbHelper dbHelper;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            dbHelper = new DbHelper();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<GoldValue> goldValues = dbHelper.GetGoldValues();
                if (goldValues.Count == 0)
                    dbHelper.addGoldValues("hn");
                else
                    printGoldValues(goldValues);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(300000, stoppingToken);
            }
        }

        private void printGoldValues (List<GoldValue> goldValues)
        {
            goldValues.ForEach((goldValue) =>
            {
                _logger.LogInformation($"{goldValue.Name} | Gi� mua: {goldValue.BuyPrice} | Gi� b�n: {goldValue.SellPrice}");
            });
        }
    }
}
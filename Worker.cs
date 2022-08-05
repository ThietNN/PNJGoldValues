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
                dbHelper.addGoldValues("hn");
                printGoldValues(goldValues);
                await Task.Delay(5000, stoppingToken);
            }
        }
        //300000
        private void printGoldValues (List<GoldValue> goldValues)
        {
            goldValues.ForEach((goldValue) =>
            {
                _logger.LogInformation($"{goldValue.Name} | Giá mua: {goldValue.BuyPrice} | Giá bán: {goldValue.SellPrice} | Thời gian cập nhật: {goldValue.GetDateTime}");
            });
        }
    }
}
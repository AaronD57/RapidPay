using RapidPay.Interfaces;
using System;
using System.Timers;

namespace RapidPay.services
{
    public class UniversalFeeExchange : IUniversalFeeExchange
    {
        private static readonly Lazy<UniversalFeeExchange> _instance = new Lazy<UniversalFeeExchange>(() => new UniversalFeeExchange());
        private decimal _currentFee;
        private System.Timers.Timer _timer;

        private UniversalFeeExchange()
        {
            UpdateCurrentFee();
            _timer = new System.Timers.Timer(3600000);
            _timer.Elapsed += (sender, e) => UpdateCurrentFee();
            _timer.Start();
        }

        public static UniversalFeeExchange Instance => _instance.Value;

        public decimal GetCurrentFee()
        {
            return _currentFee;
        }

        private void UpdateCurrentFee()
        {
            Random random = new Random();
            decimal randomMultiplier = (decimal)(random.NextDouble() * 2);
            _currentFee = _currentFee == 0 ? 1 : _currentFee * randomMultiplier;
        }
    }

}
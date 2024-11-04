namespace RapidPay.Models
{
    public class FeeRate
    {
        public int FeeRateId { get; set; }
        public decimal FeeRateValue { get; set; }
        public DateTime EffectiveFrom { get; set; } = DateTime.Now;
    }

}

using RapidPay.Models;
using System.Threading.Tasks;

public interface ICardService
{
    Task<Card> CreateCardAsync(string cardNumber);
    Task<decimal> GetCardBalanceAsync(int cardId);
    Task<bool> ProcessPaymentAsync(int cardId, decimal amount);
    Task<bool> UpdateBalanceAsync(int cardId, decimal newBalance);
}

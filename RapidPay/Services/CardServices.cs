using Microsoft.EntityFrameworkCore;
using RapidPay.Data;
using RapidPay.Interfaces;
using RapidPay.Models;

namespace RapidPay.services
{
    public class CardServices : ICardService
    {
        private readonly AppDbContext _context;
        private readonly IUniversalFeeExchange _feeExchange;

        public CardServices(AppDbContext dbContext, IUniversalFeeExchange feeExchange)
        {
            _context = dbContext;
            _feeExchange = feeExchange;
        }

        public async Task<Card> CreateCardAsync(string cardNumber)
        {
            var card = new Card { CardNumber = cardNumber, Balance = 0 };
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task<decimal> GetCardBalanceAsync(int cardId)
        {
            var card = await _context.Cards.FindAsync(cardId);
            if (card == null) throw new ArgumentException("Card not found.");
            return card.Balance;
        }



        public async Task<bool> ProcessPaymentAsync(int cardId, decimal amount)
        {
            var card = await _context.Cards.FindAsync(cardId);
            if (card == null)
            {
                return false;
            }

            // Calculate the payment fee
            decimal paymentFee = _feeExchange.GetCurrentFee();
            decimal totalAmount = amount + paymentFee;

            if (card.Balance < totalAmount)
            {
                return false;
            }

            // Update the card balance
            card.Balance -= totalAmount;
            _context.Cards.Update(card);

            // Create a new transaction with the fee included
            var transaction = new Transaction
            {
                CardId = cardId,
                Amount = amount,
                Fee = paymentFee,
                TransactionDate = DateTime.UtcNow
            };
            await _context.Transactions.AddAsync(transaction);

            // Save changes
            await _context.SaveChangesAsync();
            return true;
        }





        public async Task<bool> UpdateBalanceAsync(int cardId, decimal newBalance)
        {
            // Find the card in the database
            var card = await _context.Cards.FindAsync(cardId);

            // Check if the card exists
            if (card == null)
            {
                return false;
            }

            // Update the balance
            card.Balance = newBalance;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }

    }
}

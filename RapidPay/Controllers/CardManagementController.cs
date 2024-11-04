using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidPay.Data;
using RapidPay.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace RapidPay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CardManagementController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IUniversalFeeExchange _feeExchange;

        public CardManagementController(ICardService cardService, IUniversalFeeExchange feeExchange)
        {
            _cardService = cardService;
            _feeExchange = feeExchange;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCard([FromBody] string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length != 15 || !long.TryParse(cardNumber, out _))
            {
                return BadRequest("Invalid card number. Card number must be 15 digits.");
            }

            var card = await _cardService.CreateCardAsync(cardNumber);
            return Ok(card);
        }

        [HttpGet("{cardId:int}/balance")]
        public async Task<IActionResult> GetBalance(int cardId)
        {
            var balance = await _cardService.GetCardBalanceAsync(cardId);
            if (balance == 0) return NotFound("Card not found or balance is zero.");
            return Ok(balance);
        }

        [HttpPost("{cardId}/pay")]
        public async Task<IActionResult> ProcessPayment(int cardId, [FromBody] decimal amount)
        {
            if (cardId <= 0)
            {
                return BadRequest("Invalid card ID.");
            }

            if (amount <= 0)
            {
                return BadRequest("Payment amount must be greater than zero.");
            }

            var success = await _cardService.ProcessPaymentAsync(cardId, amount);
            if (!success) return BadRequest("Insufficient balance or invalid card.");

            return Ok("Payment processed successfully.");
        }

        [HttpPut("update-balance/{cardId}")]
        public async Task<IActionResult> UpdateBalance(int cardId, [FromBody] decimal newBalance)
        {
            var success = await _cardService.UpdateBalanceAsync(cardId, newBalance);

            if (success)
            {
                return Ok("Balance updated successfully.");
            }
            else
            {
                return NotFound("Card not found.");
            }
        }

        [HttpGet("current-fee")]
        public IActionResult GetCurrentFee()
        {
            var currentFee = _feeExchange.GetCurrentFee();
            return Ok(new { Fee = currentFee });
        }
    }

}
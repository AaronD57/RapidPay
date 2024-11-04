using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Moq;
using RapidPay.Controllers;
using RapidPay.Models;
using RapidPay.services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RapidPay.Tests
{
    public class CardControllerTests
    {
        private readonly Mock<ICardService> _cardServiceMock;
        private readonly CardManagementController _controller;

        public CardControllerTests()
        {
            _cardServiceMock = new Mock<ICardService>();
            _controller = new CardController(_cardServiceMock.Object);
        }

        [Fact]
        public async Task GetCard_ReturnsOkResult_WithCard()
        {
            // Arrange
            var cardId = 1;
            var expectedCard = new Card { Id = cardId, Name = "Test Card" };
            _cardServiceMock.Setup(s => s.GetCardByIdAsync(cardId)).ReturnsAsync(expectedCard);

            // Act
            var result = await _controller.GetCard(cardId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedCard);
        }

        [Fact]
        public async Task CreateCard_ReturnsCreatedAtActionResult_WithNewCard()
        {
            // Arrange
            var newCard = new Card { Name = "New Card" };
            var createdCard = new Card { Id = 2, Name = "New Card" };
            _cardServiceMock.Setup(s => s.CreateCardAsync(newCard)).ReturnsAsync(createdCard);

            // Act
            var result = await _controller.CreateCard(newCard);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(createdCard);
        }

        [Fact]
        public async Task UpdateCard_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var cardId = 1;
            var updatedCard = new Card { Id = cardId, Name = "Updated Card" };
            _cardServiceMock.Setup(s => s.UpdateCardAsync(cardId, updatedCard)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCard(cardId, updatedCard);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteCard_ReturnsOkResult_WhenDeletionIsSuccessful()
        {
            // Arrange
            var cardId = 1;
            _cardServiceMock.Setup(s => s.DeleteCardAsync(cardId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCard(cardId);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}

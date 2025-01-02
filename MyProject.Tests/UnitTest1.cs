using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MyLibrarySite.Services;
using MyLibrarySite.Models;
using Moq.Protected;
namespace MyProject.Tests;


public class Tests
{
   public class GutenbergServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private GutenbergService _gutenbergService;

        [SetUp]
        public void Setup()
        {
            // Mock HttpMessageHandler
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            // GutenbergService'i oluştur
            _gutenbergService = new GutenbergService(_httpClient);
        }

        [Test]
        public async Task SearchBooksAsync_ValidQuery_ReturnsBooks()
        {
            // Arrange
            var query = "C#";
            var expectedBooks = new List<Book>
            {
                new Book { Id = 1, Title = "C# Programming" },
                new Book { Id = 2, Title = "Advanced C#" }
            };

            var jsonResponse = JsonSerializer.Serialize(new { results = expectedBooks });
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            var books = await _gutenbergService.SearchBooksAsync(query);

            // Assert
            Assert.IsNotNull(books);
            Assert.AreEqual(2, books.Count);
            Assert.AreEqual("C# Programming", books[0].Title);
        }

        [Test]
        public void SearchBooksAsync_FailedRequest_ThrowsException()
        {
            // Arrange
            var query = "C#";
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(async () =>
                await _gutenbergService.SearchBooksAsync(query));
        }

        [Test]
        public async Task SearchBooksAsync_EmptyResults_ReturnsEmptyList()
        {
            // Arrange
            var query = "NonExistingBook";
            var jsonResponse = JsonSerializer.Serialize(new { results = new List<Book>() });
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            var books = await _gutenbergService.SearchBooksAsync(query);

            // Assert
            Assert.IsNotNull(books);
            Assert.IsEmpty(books);
        }
    }
}
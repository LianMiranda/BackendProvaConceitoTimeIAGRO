using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

using Project.API.Controllers;
using Project.Application.Dtos;
using Project.Application.Services;
using Project.Domain.Entities;

namespace Project.API.Tests.Controllers;

public class BooksControllerTests
{
    private readonly Mock<IBookService> _bookServiceMock;
    private readonly Mock<ILogger<BooksController>> _loggerMock;
    private readonly BooksController _controller;

    public BooksControllerTests()
    {
        _bookServiceMock = new Mock<IBookService>();
        _loggerMock = new Mock<ILogger<BooksController>>();

        _controller = new BooksController(
            _bookServiceMock.Object,
            _loggerMock.Object
        );
    }
    
    [Fact]
    public async Task SearchBooks_ShouldReturnOk_WhenServiceReturnsBooks()
    {
        var books = new List<Book> { new Book { Id = 1, Name = "Clean Code" } };

        _bookServiceMock
            .Setup(s => s.SearchBooksAsync(It.IsAny<BookSearchDto>()))
            .ReturnsAsync(books);

        var result = await _controller.SearchBooks(
            name: "Clean",
            author: null,
            genre: null,
            illustrator: null,
            sortByPrice: null
        );

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);

        Assert.Single(value);
        Assert.Equal("Clean Code", value.First().Name);
    }

    [Fact]
    public async Task SearchBooks_ShouldReturn500_WhenServiceThrowsException()
    {
        _bookServiceMock
            .Setup(s => s.SearchBooksAsync(It.IsAny<BookSearchDto>()))
            .ThrowsAsync(new Exception("error"));

        var result = await _controller.SearchBooks();

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task SearchBooks_ShouldCallServiceWithCorrectDto()
    {
        _bookServiceMock
            .Setup(s => s.SearchBooksAsync(It.IsAny<BookSearchDto>()))
            .ReturnsAsync(new List<Book>());

        await _controller.SearchBooks(
            name: "Clean",
            author: "Martin",
            genre: "Programming",
            illustrator: "John",
            sortByPrice: "asc"
        );

        _bookServiceMock.Verify(s =>
                s.SearchBooksAsync(It.Is<BookSearchDto>(dto =>
                    dto.Name == "Clean" &&
                    dto.Author == "Martin" &&
                    dto.Genre == "Programming" &&
                    dto.Illustrator == "John" &&
                    dto.SortByPrice == "asc"
                )),
            Times.Once
        );
    }


    [Fact]
    public async Task CalculateShipping_ShouldReturnOk_WhenBookExists()
    {
        _bookServiceMock
            .Setup(s => s.CalculateShippingAsync(1))
            .ReturnsAsync(20m);

        var result = await _controller.CalculateShipping(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(20m, okResult.Value);
    }

    [Fact]
    public async Task CalculateShipping_ShouldReturnNotFound_WhenBookDoesNotExist()
    {
        _bookServiceMock
            .Setup(s => s.CalculateShippingAsync(1))
            .ReturnsAsync((decimal?)null);

        var result = await _controller.CalculateShipping(1);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CalculateShipping_ShouldReturn500_WhenServiceThrowsException()
    {
        _bookServiceMock
            .Setup(s => s.CalculateShippingAsync(1))
            .ThrowsAsync(new Exception("error"));

        var result = await _controller.CalculateShipping(1);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
    }
}
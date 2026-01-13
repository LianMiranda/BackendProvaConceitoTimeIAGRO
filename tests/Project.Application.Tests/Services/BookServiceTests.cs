using Moq;

using Project.Application.Dtos;
using Project.Application.Services;
using Project.Domain.Entities;
using Project.Domain.Interfaces;

namespace Project.Application.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly BookService _service;

    public BookServiceTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _service = new BookService(_bookRepositoryMock.Object);
    }


    [Fact]
    public async Task SearchBooksAsync_ShouldReturnAllBooks_WhenNoFiltersAreProvided()
    {
        var books = CreateBooks();
        _bookRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(books);

        var searchDto = new BookSearchDto();

        var result = await _service.SearchBooksAsync(searchDto);

        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task SearchBooksAsync_ShouldFilterByName()
    {
        var books = CreateBooks();
        _bookRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

        var searchDto = new BookSearchDto { Name = "Clean" };

        var result = await _service.SearchBooksAsync(searchDto);

        Assert.Single(result);
        Assert.Equal("Clean Code", result.First().Name);
    }

    [Fact]
    public async Task SearchBooksAsync_ShouldFilterByAuthor()
    {
        var books = CreateBooks();
        _bookRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

        var searchDto = new BookSearchDto { Author = "Martin" };

        var result = await _service.SearchBooksAsync(searchDto);

        Assert.Single(result);
        Assert.Equal("Robert C. Martin", result.First().Specifications.Author);
    }

    [Fact]
    public async Task SearchBooksAsync_ShouldFilterByGenre()
    {
        var books = CreateBooks();
        _bookRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

        var searchDto = new BookSearchDto { Genre = "Fantasy" };

        var result = await _service.SearchBooksAsync(searchDto);

        Assert.Single(result);
        Assert.Equal("The Hobbit", result.First().Name);
    }

    [Fact]
    public async Task SearchBooksAsync_ShouldFilterByIllustrator()
    {
        var books = CreateBooks();
        _bookRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

        var searchDto = new BookSearchDto { Illustrator = "John" };

        var result = await _service.SearchBooksAsync(searchDto);

        Assert.Single(result);
    }

    [Theory]
    [InlineData("asc")]
    [InlineData("ASC")]
    public async Task SearchBooksAsync_ShouldSortByPriceAscending(string sort)
    {
        var books = CreateBooks();
        _bookRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

        var searchDto = new BookSearchDto { SortByPrice = sort };

        var result = (await _service.SearchBooksAsync(searchDto)).ToList();

        Assert.True(result.SequenceEqual(result.OrderBy(b => b.Price)));
    }

    [Theory]
    [InlineData("desc")]
    [InlineData("DESC")]
    public async Task SearchBooksAsync_ShouldSortByPriceDescending(string sort)
    {
        var books = CreateBooks();
        _bookRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

        var searchDto = new BookSearchDto { SortByPrice = sort };

        var result = (await _service.SearchBooksAsync(searchDto)).ToList();

        Assert.True(result.SequenceEqual(result.OrderByDescending(b => b.Price)));
    }


    [Fact]
    public async Task CalculateShippingAsync_ShouldReturnNull_WhenBookDoesNotExist()
    {
        _bookRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Book?)null);

        var result = await _service.CalculateShippingAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task CalculateShippingAsync_ShouldReturnShipping_WhenBookExists()
    {
        var book = new Book { Price = 100m };

        _bookRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(book);

        var result = await _service.CalculateShippingAsync(1);

        Assert.Equal(20m, result);
    }

    private static List<Book> CreateBooks()
    {
        return new()
        {
            new Book
            {
                Id = 1,
                Name = "Clean Code",
                Price = 100,
                Specifications =
                    new BookSpecifications
                    {
                        Author = "Robert C. Martin",
                        Genres = System.Text.Json.JsonDocument.Parse("[\"Programming\"]").RootElement
                    }
            },
            new Book
            {
                Id = 2,
                Name = "The Hobbit",
                Price = 50,
                Specifications =
                    new BookSpecifications
                    {
                        Author = "J.R.R. Tolkien",
                        Genres = System.Text.Json.JsonDocument.Parse("[\"Fantasy\"]").RootElement
                    }
            },
            new Book
            {
                Id = 3,
                Name = "Illustrated Book",
                Price = 80,
                Specifications = new BookSpecifications
                {
                    Illustrator = System.Text.Json.JsonDocument.Parse("\"John Doe\"").RootElement
                }
            }
        };
    }
}
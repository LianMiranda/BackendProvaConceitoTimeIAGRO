using System.Text.Json;
using Project.Domain.Entities;
using Project.Infrastructure.Repositories;



namespace Project.Infrastructure.Tests.Repositories;

public class BookRepositoryTests : IDisposable
{
    private readonly string _tempFilePath;

    public BookRepositoryTests()
    {
        _tempFilePath = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.json"
        );
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnBooks_WhenJsonIsValid()
    {
        var books = CreateBooks();
        WriteJsonToFile(books);

        var repository = new BookRepository(_tempFilePath);

        var result = await repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, b => b.Name == "Clean Code");
    }

    [Fact]
    public async Task GetAllAsync_ShouldCacheBooks_AfterFirstRead()
    {
        var books = CreateBooks();
        WriteJsonToFile(books);

        var repository = new BookRepository(_tempFilePath);

        var firstCall = await repository.GetAllAsync();

        File.Delete(_tempFilePath);

        var secondCall = await repository.GetAllAsync();

        Assert.Equal(firstCall.Count(), secondCall.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        var repository = new BookRepository(_tempFilePath);

        await Assert.ThrowsAsync<FileNotFoundException>(() =>
            repository.GetAllAsync());
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowInvalidOperationException_WhenJsonIsInvalid()
    {
        File.WriteAllText(_tempFilePath, "invalid json");

        var repository = new BookRepository(_tempFilePath);

        await Assert.ThrowsAsync<JsonException>(() =>
            repository.GetAllAsync());
    }


    [Fact]
    public async Task GetByIdAsync_ShouldReturnBook_WhenIdExists()
    {
        var books = CreateBooks();
        WriteJsonToFile(books);

        var repository = new BookRepository(_tempFilePath);

        var book = await repository.GetByIdAsync(1);

        Assert.NotNull(book);
        Assert.Equal("Clean Code", book!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        var books = CreateBooks();
        WriteJsonToFile(books);

        var repository = new BookRepository(_tempFilePath);

        var book = await repository.GetByIdAsync(999);

        Assert.Null(book);
    }
    
    private void WriteJsonToFile(List<Book> books)
    {
        var json = JsonSerializer.Serialize(books);
        File.WriteAllText(_tempFilePath, json);
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
                Specifications = new BookSpecifications { Author = "Robert C. Martin" }
            },
            new Book
            {
                Id = 2,
                Name = "The Hobbit",
                Price = 50,
                Specifications = new BookSpecifications { Author = "J.R.R. Tolkien" }
            }
        };
    }
    
    public void Dispose()
    {
        if (File.Exists(_tempFilePath))
            File.Delete(_tempFilePath);
    }
}
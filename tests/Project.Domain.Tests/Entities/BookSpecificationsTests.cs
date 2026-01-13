using System.Text.Json;

using Project.Domain.Entities;

namespace Project.Domain.Tests.Entities;

public class BookSpecificationsTests
{
    [Fact]
    public void GetIllustrators_ShouldReturnEmptyList_WhenIllustratorIsNull()
    {
        var specs = new BookSpecifications()
        {
            Illustrator = null
        };

        var result = specs.GetIllustrators();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetIllustrators_ShouldReturnSingleItem_WhenIllustratorIsString()
    {
        var json = JsonDocument.Parse("\"John Doe\"").RootElement;

        var specs = new BookSpecifications
        {
            Illustrator = json
        };

        var result = specs.GetIllustrators();

        Assert.Single(result);
        Assert.Equal("John Doe", result[0]);
    }

    [Fact]
    public void GetIllustrators_ShouldReturnList_WhenIllustratorIsArray()
    {
        var json = JsonDocument.Parse("[\"John Doe\", \"Jane Doe\"]").RootElement;

        var specs = new BookSpecifications
        {
            Illustrator = json
        };

        var result = specs.GetIllustrators();

        Assert.Equal(2, result.Count);
        Assert.Contains("John Doe", result);
        Assert.Contains("Jane Doe", result);
    }

    [Fact]
    public void GetIllustrators_ShouldReturnEmptyList_WhenIllustratorIsInvalidType()
    {
        var json = JsonDocument.Parse("123").RootElement;

        var specs = new BookSpecifications
        {
            Illustrator = json
        };

        var result = specs.GetIllustrators();

        Assert.Empty(result);
    }
    
    [Fact]
    public void GetGenres_ShouldReturnEmptyList_WhenGenresIsNull()
    {
        var specs = new BookSpecifications
        {
            Genres = null
        };

        var result = specs.GetGenres();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetGenres_ShouldReturnSingleItem_WhenGenresIsString()
    {
        var json = JsonDocument.Parse("\"Fantasy\"").RootElement;

        var specs = new BookSpecifications
        {
            Genres = json
        };

        var result = specs.GetGenres();

        Assert.Single(result);
        Assert.Equal("Fantasy", result[0]);
    }

    [Fact]
    public void GetGenres_ShouldReturnList_WhenGenresIsArray()
    {
        var json = JsonDocument.Parse("[\"Fantasy\", \"Adventure\"]").RootElement;

        var specs = new BookSpecifications
        {
            Genres = json
        };

        var result = specs.GetGenres();

        Assert.Equal(2, result.Count);
        Assert.Contains("Fantasy", result);
        Assert.Contains("Adventure", result);
    }

    [Fact]
    public void GetGenres_ShouldReturnEmptyList_WhenGenresIsInvalidType()
    {
        var json = JsonDocument.Parse("true").RootElement;

        var specs = new BookSpecifications
        {
            Genres = json
        };

        var result = specs.GetGenres();

        Assert.Empty(result);
    }
}
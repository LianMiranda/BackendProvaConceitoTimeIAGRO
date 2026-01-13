using Project.Domain.Entities;

namespace Project.Domain.Tests.Entities;

public class BookTests
{
    [Fact]
    public void CalculateShipping_ShouldReturn20PercentOfPrice()
    {
        var book = new Book { Price = 100m };

        var shipping = book.CalculateShipping();

        Assert.Equal(20m, shipping);
    }

    [Fact]
    public void CalculateShipping_ShouldReturnZero_WhenPriceIsZero()
    {
        var book = new Book { Price = 0m };

        var shipping = book.CalculateShipping();

        Assert.Equal(0m, shipping);
    }

    [Theory]
    [InlineData(50, 10)]
    [InlineData(99.90, 19.98)]
    [InlineData(10.50, 2.10)]
    public void CalculateShipping_ShouldCalculateCorrectly_ForDecimalPrices(
        decimal price,
        decimal expectedShipping)
    {
        var book = new Book { Price = price };

        var shipping = book.CalculateShipping();

        Assert.Equal(expectedShipping, shipping);
    }
}
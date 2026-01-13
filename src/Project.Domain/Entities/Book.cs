using System.Text.Json.Serialization;

namespace Project.Domain.Entities;

public class Book
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("price")] public decimal Price { get; set; }
    [JsonPropertyName("specifications")] public BookSpecifications Specifications { get; set; } = new();

    /// <summary>
    /// Calcula o valor do frete (20% do valor do livro)
    /// </summary>
    /// <returns>Valor do frete</returns>
    public decimal CalculateShipping()
    {
        return Price * 0.20m;
    }
}
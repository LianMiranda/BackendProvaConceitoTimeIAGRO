using System.Text.Json;
using System.Text.Json.Serialization;

namespace Project.Domain.Entities;

public class BookSpecifications
{
    [JsonPropertyName("Originally published")]
    public string OriginallyPublished { get; set; } = string.Empty;

    [JsonPropertyName("Author")] public string Author { get; set; } = string.Empty;

    [JsonPropertyName("Page count")] public int PageCount { get; set; }

    [JsonPropertyName("Illustrator")] public object? Illustrator { get; set; }

    [JsonPropertyName("Genres")] public object? Genres { get; set; }

    /// <summary>
    /// Obtém a lista de ilustradores associados ao livro.
    /// </summary>
    /// <returns>
    /// Lista de ilustradores do livro.  
    /// </returns>
    public List<string> GetIllustrators()
    {
        if (Illustrator == null) return new List<string>();

        return Illustrator switch
        {
            JsonElement element when element.ValueKind == JsonValueKind.String
                => new List<string> { element.GetString() ?? string.Empty },
            JsonElement element when element.ValueKind == JsonValueKind.Array
                => element.EnumerateArray()
                    .Select(e => e.GetString() ?? string.Empty)
                    .ToList(),
            _ => new List<string>()
        };
    }

    /// <summary>
    /// Obtém a lista de gêneros associados ao livro.
    /// </summary>
    /// <returns>
    /// Lista de gêneros do livro.  
    /// </returns>
    public List<string> GetGenres()
    {
        if (Genres == null) return new List<string>();

        return Genres switch
        {
            JsonElement element when element.ValueKind == JsonValueKind.String
                => new List<string> { element.GetString() ?? string.Empty },
            JsonElement element when element.ValueKind == JsonValueKind.Array
                => element.EnumerateArray()
                    .Select(e => e.GetString() ?? string.Empty)
                    .ToList(),
            _ => new List<string>()
        };
    }
}
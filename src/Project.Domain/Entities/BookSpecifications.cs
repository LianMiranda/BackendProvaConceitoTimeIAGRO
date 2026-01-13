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

    public List<string> GetIllustrators()
    {
        return Illustrator switch
        {
            string single => new List<string> { single },
            List<object> list => list.Select(i => i.ToString() ?? string.Empty).ToList(),
            _ => new List<string>()
        };
    }

    public List<string> GetGenres()
    {
        return Genres switch
        {
            string single => new List<string> { single },
            List<object> list => list.Select(g => g.ToString() ?? string.Empty).ToList(),
            _ => new List<string>()
        };
    }
}
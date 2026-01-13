namespace Project.Application.Dtos;

public record BookSearchDto
{
    public string? Name { get; init; }
    public string? Author { get; init; }
    public string? Genre { get; init; }
    public string? Illustrator { get; init; }
    public string? SortByPrice { get; init; }
}
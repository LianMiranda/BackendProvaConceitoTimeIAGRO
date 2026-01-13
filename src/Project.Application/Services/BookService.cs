using Project.Application.Dtos;
using Project.Domain.Entities;
using Project.Domain.Interfaces;

namespace Project.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    
    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
    }

   
    /// <summary>
    /// Busca os livros usando filtros e ordenação caso sejam especificados
    /// </summary>
    /// <param name="searchDto">Dto com as especificações de busca</param>
    /// <returns>Lista de livros que correspondem aos critérios enviados</returns>
    public async Task<IEnumerable<Book>> SearchBooksAsync(BookSearchDto searchDto)
    {
        var books = await _bookRepository.GetAllAsync();
        
        var booksList = books.ToList();
        
        var filteredBooks = ApplyFilters(booksList, searchDto);
        var sortedBooks = ApplySortingStrategy(filteredBooks, searchDto.SortByPrice!);

        return sortedBooks;
    }

    /// <summary>
    /// Calcula o frete de um livro específico
    /// </summary>
    /// <param name="bookId">Book ID</param>
    /// <returns>Valor do frete</returns>
    public async Task<decimal?> CalculateShippingAsync(int bookId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId);
        
        if (book == null)
        {
            return null;
        }

        return book.CalculateShipping();
    }

    /// <summary>
    /// Aplicando estratégia de ordenação (Strategy Pattern)
    /// </summary>
    private IEnumerable<Book> ApplySortingStrategy(IEnumerable<Book> books, string orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return books;

        return orderBy.ToLowerInvariant() switch
        {
            "asc" => books.OrderBy(b => b.Price),
            "desc" => books.OrderByDescending(b => b.Price),
            _ => books
        };
    }
    
    /// <summary>
    /// Aplicando os filtros 
    /// </summary>
    private static IEnumerable<Book> ApplyFilters(IEnumerable<Book> books, BookSearchDto searchDto)
    {
        var query = books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(searchDto.Name))
        {
            query = query.Where(b => 
                b.Name.Contains(searchDto.Name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(searchDto.Author))
        {
            query = query.Where(b => 
                b.Specifications.Author.Contains(searchDto.Author, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(searchDto.Genre))
        {
            query = query.Where(b => 
                b.Specifications.GetGenres()
                    .Any(g => g.Contains(searchDto.Genre, StringComparison.OrdinalIgnoreCase)));
        }

        if (!string.IsNullOrWhiteSpace(searchDto.Illustrator))
        {
            query = query.Where(b => 
                b.Specifications.GetIllustrators()
                    .Any(i => i.Contains(searchDto.Illustrator, StringComparison.OrdinalIgnoreCase)));
        }

        return query;
    }
}
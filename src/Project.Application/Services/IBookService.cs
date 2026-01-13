using Project.Application.Dtos;
using Project.Domain.Entities;

namespace Project.Application.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> SearchBooksAsync(BookSearchDto searchDto);
    Task<decimal?> CalculateShippingAsync(int bookId);
}
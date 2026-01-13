using Microsoft.AspNetCore.Mvc;

using Project.Application.Dtos;
using Project.Application.Services;
using Project.Domain.Entities;

namespace Project.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IBookService bookService, ILogger<BooksController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    /// <summary>
    /// Busca os livros usando filtros e ordenação caso sejam especificados
    /// </summary>
    /// <param name="name">Nome do livro</param>
    /// <param name="author">Nome do autor</param>
    /// <param name="genre">Gênero do livro</param>
    /// <param name="illustrator">Nome do ilustrador(a)</param>
    /// <param name="sortByPrice">Ordenar por preço: 'asc' or 'desc'</param>
    /// <returns>Lista de livros que correspondem aos critérios enviados</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Book>>> SearchBooks(
        [FromQuery] string? name = null,
        [FromQuery] string? author = null,
        [FromQuery] string? genre = null,
        [FromQuery] string? illustrator = null,
        [FromQuery] string? sortByPrice = null)
    {
        try
        {
            var searchDto = new BookSearchDto()
            {
                Name = name,
                Author = author,
                Genre = genre,
                Illustrator = illustrator,
                SortByPrice = sortByPrice
            };

            var results = await _bookService.SearchBooksAsync(searchDto);
            return Ok(new { Data = results });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching books");
            return StatusCode(500, "An error occurred while searching books");
        }
    }

    /// <summary>
    /// Calcula o frete de um livro (20% do preço do livro).
    /// </summary>
    /// <param name="bookId">Book ID</param>
    /// <returns>Valor do frete</returns>
    [HttpGet("{bookId}/shipping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<decimal>> CalculateShipping([FromRoute] int bookId)
    {
        try
        {
            var result = await _bookService.CalculateShippingAsync(bookId);

            if (result == null)
            {
                return NotFound($"Book with ID {bookId} not found");
            }

            return Ok(new { ShippingCost = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating shipping for book {BookId}", bookId);
            return StatusCode(500, "An error occurred while calculating shipping");
        }
    }
}
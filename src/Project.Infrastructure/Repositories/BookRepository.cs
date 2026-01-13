using System.Text.Json;

using Project.Domain.Entities;
using Project.Domain.Interfaces;

namespace Project.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string _jsonFilePath;
        private List<Book> _cachedBooks;
        private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);

        public BookRepository(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath ?? throw new ArgumentNullException(nameof(jsonFilePath));
        }

        /// <summary>
        /// Busca todos os livros do arquivo JSON
        /// Implementa cache em memória para evitar leitura repetida
        /// </summary>
        /// <returns>
        /// Lista de todos os livros
        /// </returns>
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            if (_cachedBooks != null)
                return _cachedBooks;

            await _cacheLock.WaitAsync();
            try
            {
                if (_cachedBooks != null)
                    return _cachedBooks;

                if (!File.Exists(_jsonFilePath))
                    throw new FileNotFoundException($"JSON file not found: {_jsonFilePath}");

                var jsonContent = await File.ReadAllTextAsync(_jsonFilePath);
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                _cachedBooks = JsonSerializer.Deserialize<List<Book>>(jsonContent, options)!;
                
                if (_cachedBooks == null)
                    throw new InvalidOperationException("Falha ao deserializar o arquivo JSON");

                return _cachedBooks;
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        /// <summary>
        /// Busca um livro específico por ID
        /// </summary>
        /// <returns>
        /// Livro com o id especificado ou NULL 
        /// </returns>
        public async Task<Book?> GetByIdAsync(int id)
        {
            var books = await GetAllAsync();
            return books.FirstOrDefault(b => b.Id == id);
        }
    }
}
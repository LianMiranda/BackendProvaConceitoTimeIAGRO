# Desafio Prova Conceito Time IAGRO

API REST para busca e consulta de catálogo de livros em um arquivo JSON.

## Sumário
- [Requisitos Atendidos](#requisitos-atendidos)
- [Arquitetura](#arquitetura)
- [Como executar](#como-executar)
- [Endpoints](#endpoints)
- [Testes](#testes)
- [Funcionalidades Tecnicas](#funcionalidades-técnicas)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)

## Requisitos Atendidos

- Busca de livros por atributos (título, autor, gênero e ilustrador)
- Ordenação por preço (ascendente e descendente)
- Cálculo de frete (20% do valor do livro)
- Leitura de arquivo JSON sem modificação
- Organização de código com Clean Architecture
- Princípios SOLID aplicados
- Padrões de projeto (Repository, Strategy, Dependency Injection)
- Testes unitários completos

## Arquitetura

### Estrutura de Pastas

```
BookCatalogAPI/
├── src/
│   ├── Project.API/                # Entidades e interfaces
│   ├── Project.Application/        # Serviços e lógica de negócio
│   ├── Project.Domain/             # Repositórios e acesso a dados
│   └── Project.Infrastructure/     # Controllers e configuração
└── tests/
    ├── Project.API.Tests/                  # Testes da camada API
    ├── Project.Application.Tests/          # Testes da camada Application
    ├── Project.Domain.Tests/               # Testes da camada Domain
    └── Project.Infrastructure.Tests/       # Testes da camada Infrastructure
```

### Padrões de Projeto

- **Repository Pattern**: Abstração do acesso a dados
- **Dependency Injection**: Inversão de controle
- **Strategy Pattern**: Estratégias de ordenação
- **Singleton Pattern**: Cache do arquivo JSON

## Como Executar

### Pré-requisitos

- .NET 6.0 ou superior
- Visual Studio 2022, Rider ou VS Code

### Passos

1. **Clone o repositório**

```bash
git clone https://github.com/LianMiranda/BackendProvaConceitoTimeIAGRO.git
```

2. **Restaure as dependências**

```bash
dotnet restore
```

4. **Execute os testes**

```bash
dotnet test
```

5. **Execute a API**

```bash
cd src/Project.API
dotnet run
```

ou 

```bash
dotnet run --project src/Project.API/
```

6. Para acessar a documentação do Swagger acesse a URL: `http://localhost:5271/index.html`


## Endpoints

### 1. Listar todos os livros

```http
GET /api/books
```

**Resposta:**

```json
{
  "data": [
    {
      "id": 1,
      "name": "Journey to the Center of the Earth",
      "price": 10,
      "specifications": {
        "Originally published": "November 25, 1864",
        "Author": "Jules Verne",
        "Page count": 183,
        "Illustrator": "Édouard Riou",
        "Genres": [
          "Science Fiction",
          "Adventure fiction"
        ]
      }
    },
    {
      "id": 2,
      "name": "20,000 Leagues Under the Sea",
      "price": 10.1,
      "specifications": {
        "Originally published": "June 20, 1870",
        "Author": "Jules Verne",
        "Page count": 213,
        "Illustrator": [
          "Édouard Riou",
          "Alphonse-Marie-Adolphe de Neuville"
        ],
        "Genres": "Adventure fiction"
      }
    },
    {
      "id": 3,
      "name": "Harry Potter and the Goblet of Fire",
      "price": 7.31,
      "specifications": {
        "Originally published": "July 8, 2000",
        "Author": "J. K. Rowling",
        "Page count": 636,
        "Illustrator": [
          "Cliff Wright",
          "Mary GrandPré"
        ],
        "Genres": [
          "Fantasy Fiction",
          "Drama",
          "Young adult fiction",
          "Mystery",
          "Thriller",
          "Bildungsroman"
        ]
      }
    },
    {
      "id": 4,
      "name": "Fantastic Beasts and Where to Find Them: The Original Screenplay",
      "price": 11.15,
      "specifications": {
        "Originally published": "November 18, 2016",
        "Author": "J. K. Rowling",
        "Page count": 457,
        "Illustrator": "Cliff Wright",
        "Genres": [
          "Fantasy Fiction",
          "Contemporary fantasy",
          "Screenplay"
        ]
      }
    },
    {
      "id": 5,
      "name": "The Lord of the Rings",
      "price": 6.15,
      "specifications": {
        "Originally published": "July 29, 1954",
        "Author": "J. R. R. Tolkien",
        "Page count": 715,
        "Illustrator": [
          "Alan Lee",
          "Ted Nashmith",
          "J. R. R. Tolkien"
        ],
        "Genres": [
          "Fantasy Fiction",
          "Adventure Fiction"
        ]
      }
    }
  ]
}
```

### 2. Buscar livros com filtros

```http
GET /api/books/search?query={termo}&sortByPrice={ordenacao}
```

**Parâmetros:**

- `query` (opcional): Termo de busca (título, autor, gênero e ilustrador)
- `sortByPrice`(opcional): Ordenação
    - `asc`: Preço crescente
    - `desc`: Preço decrescente

**Exemplos:**

```http
GET /api/Books/search?name=Harry%20Potter%20and%20the%20Goblet%20of%20Fire'
GET /api/Books/search?genre=Fantasy%20Fiction&sortByPrice=desc
GET /api/Books/search?illustrator=Cliff%20Wright
```

**Exemplo com a requisição e a resposta:**

`GET /api/Books/search?genre=Fantasy%20Fiction&sortByPrice=desc`

```json
{
  "data": [
    {
      "id": 4,
      "name": "Fantastic Beasts and Where to Find Them: The Original Screenplay",
      "price": 11.15,
      "specifications": {
        "Originally published": "November 18, 2016",
        "Author": "J. K. Rowling",
        "Page count": 457,
        "Illustrator": "Cliff Wright",
        "Genres": [
          "Fantasy Fiction",
          "Contemporary fantasy",
          "Screenplay"
        ]
      }
    },
    {
      "id": 3,
      "name": "Harry Potter and the Goblet of Fire",
      "price": 7.31,
      "specifications": {
        "Originally published": "July 8, 2000",
        "Author": "J. K. Rowling",
        "Page count": 636,
        "Illustrator": [
          "Cliff Wright",
          "Mary GrandPré"
        ],
        "Genres": [
          "Fantasy Fiction",
          "Drama",
          "Young adult fiction",
          "Mystery",
          "Thriller",
          "Bildungsroman"
        ]
      }
    },
    {
      "id": 5,
      "name": "The Lord of the Rings",
      "price": 6.15,
      "specifications": {
        "Originally published": "July 29, 1954",
        "Author": "J. R. R. Tolkien",
        "Page count": 715,
        "Illustrator": [
          "Alan Lee",
          "Ted Nashmith",
          "J. R. R. Tolkien"
        ],
        "Genres": [
          "Fantasy Fiction",
          "Adventure Fiction"
        ]
      }
    }
  ]
}
```

### 3. Calcular frete

```http
GET /api/Books/{bookId}/shipping
```

**Respostas:**

```json
{
  "shippingCost": x.xx
}
```

**Exemplos** 

`GET /api/Books/1/shipping`

```json
{
  "shippingCost": 2
}
```
**Exemplo com Id inexistente**

`GET /api/Books/11/shipping`

```json
Book with ID 11 not found
```

## Testes

O projeto possui cobertura completa de testes unitários:

### Executar todos os testes

```bash
dotnet test
```

### Testes Implementados

**Testes no `BookController`**:

* **Busca de livros (`SearchBooks`)**

    * Retorno **200 OK** quando o serviço encontra livros
    * Retorno **500 Internal Server Error** quando o serviço lança exceção
    * Verificação do **envio correto dos filtros** para o serviço (nome, autor, gênero, ilustrador e ordenação por preço)

* **Cálculo de frete (`CalculateShipping`)**

    * Retorno **200 OK** quando o livro existe e o frete é calculado
    * Retorno **404 Not Found** quando o livro não é encontrado
    * Retorno **500 Internal Server Error** quando ocorre erro no serviço


**Testes no `BookService`**:

* **Busca de livros (`SearchBooksAsync`)**

    * Retorno de **todos os livros** quando nenhum filtro é informado
    * Filtro por **nome do livro**
    * Filtro por **autor**
    * Filtro por **gênero**
    * Filtro por **ilustrador**
    * Ordenação por **preço crescente** (`asc`, case insensitive)
    * Ordenação por **preço decrescente** (`desc`, case insensitive)

* **Cálculo de frete (`CalculateShippingAsync`)**

    * Retorno **null** quando o livro não existe
    * Retorno do **valor do frete** quando o livro existe



**Testes da entidade `Book`**:

* **Cálculo ne frete (`CalculateShipping`)**

    * Retorno de **20% do valor do preço**
    * Retorno **0** quando o preço é zero
    * Cálculo correto para **valores decimais** (incluindo casas decimais)


**Testes na entidade `BookSpecifications`**:

* **Manipulação de ilustradores (`GetIllustrators`)**

    * Retorno de **lista vazia** quando o ilustrador é `null`
    * Retorno de **um item** quando o ilustrador é uma string
    * Retorno de **lista** quando o ilustrador é um array
    * Retorno de **lista vazia** para tipos inválidos

* **Manipulação de gêneros (`GetGenres`)**

    * Retorno de **lista vazia** quando os gêneros são `null`
    * Retorno de **um item** quando os gêneros são uma string
    * Retorno de **lista** quando os gêneros são um array
    * Retorno de **lista vazia** para tipos inválidos

**Testes no `BookRepository`**:

* **Leitura de livros (`GetAllAsync`)**

    * Retorno da **lista de livros** quando o JSON é válido
    * **Cache em memória** após a primeira leitura do arquivo
    * Lançamento de **`FileNotFoundException`** quando o arquivo não existe
    * Lançamento de **exceção** quando o JSON é inválido

* **Busca por ID (`GetByIdAsync`)**

    * Retorno do **livro correto** quando o ID existe
    * Retorno **null** quando o ID não é encontrado

## Funcionalidades Técnicas

### 1. Busca utilizando filtros

A busca procura em múltiplos campos:

- Nome do livro
- Autor
- Gênero
- Ilustrador

### 2. Cache

O arquivo JSON é lido apenas uma vez e mantido em cache, melhorando performance.

### 3. Thread-Safe

Utiliza `SemaphoreSlim` para garantir leitura thread-safe do arquivo.

### 4. Validações

- Validação de parâmetros
- Tratamento de erros
- Mensagens descritivas


## Tecnologias Utilizadas

- .NET 9.0
- ASP.NET Core Web API
- System.Text.Json (sem frameworks externos)
- xUnit (testes)
- Moq (mocks para testes)
- Swagger/OpenAPI (documentação)

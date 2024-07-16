using System.ComponentModel.Design;
using LinQ_Lab.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinQ_Lab.Controllers;


public static class Library
{
    public static List<Author> Authors { get; set; } = new List<Author>
    {
        new Author { AuthorId = 1, Name = "Mark Twain", BirthDate = new DateTime(1835, 11, 30), CountryName = "France" },
        new Author { AuthorId = 2, Name = "J.K. Rowling", BirthDate = new DateTime(1835, 7, 31), CountryName = "France" },
        new Author { AuthorId = 3, Name = "Margaret Atwood", BirthDate = new DateTime(1835, 11, 18), CountryName = "United Kingdom" },
        new Author { AuthorId = 4, Name = "Tim Winton", BirthDate = new DateTime(1960, 8, 4), CountryName = "Lebanon" },
        new Author { AuthorId = 5, Name = "Johann Wolfgang von Goethe", BirthDate = new DateTime(1749, 8, 28), CountryName = "Spain" }
    };

    public static List<Book> Books { get; set; } = new List<Book>
    {
        new Book { BookId = 1, Title = "The Adventures of Tom Sawyer", AuthorId = 1, ISBN = "9780143039563", PublishedYear = new DateTime(1876, 6, 1) },
        new Book { BookId = 2, Title = "Harry Potter and the Philosopher's Stone", AuthorId = 2, ISBN = "9780747532699", PublishedYear = new DateTime(1876, 6, 26) },
        new Book { BookId = 3, Title = "The Handmaid's Tale", AuthorId = 3, ISBN = "9780771008795", PublishedYear = new DateTime(1985, 9, 1) },
        new Book { BookId = 4, Title = "Cloudstreet", AuthorId = 4, ISBN = "9780140273984", PublishedYear = new DateTime(1991, 4, 1) },
        new Book { BookId = 5, Title = "Faust", AuthorId = 5, ISBN = "9780140449014", PublishedYear = new DateTime(1808, 1, 1) },
        new Book { BookId = 6, Title = "The Adventures of Tom Sawyer", AuthorId = 1, ISBN = "9780143039563", PublishedYear = new DateTime(1876, 6, 1) },
        new Book { BookId = 7, Title = "Harry Potter and the Philosopher's Stone", AuthorId = 2, ISBN = "9780747532699", PublishedYear = new DateTime(1876, 6, 26) },
        new Book { BookId = 8, Title = "The Handmaid's Tale", AuthorId = 3, ISBN = "9780771008795", PublishedYear = new DateTime(1985, 9, 1) },
        new Book { BookId = 9, Title = "Cloudstreet", AuthorId = 4, ISBN = "9780140273984", PublishedYear = new DateTime(1991, 4, 1) },
        new Book { BookId = 10, Title = "Faust", AuthorId = 5, ISBN = "9780140449014", PublishedYear = new DateTime(1808, 1, 1) },
        new Book { BookId = 11, Title = "The Adventures of Tom Sawyer", AuthorId = 1, ISBN = "9780143039563", PublishedYear = new DateTime(1876, 6, 1) },
        new Book { BookId = 12, Title = "Harry Potter and the Philosopher's Stone", AuthorId = 2, ISBN = "9780747532699", PublishedYear = new DateTime(1876, 6, 26) },
        new Book { BookId = 13, Title = "The Handmaid's Tale", AuthorId = 3, ISBN = "9780771008795", PublishedYear = new DateTime(1985, 9, 1) },
        new Book { BookId = 14, Title = "Cloudstreet", AuthorId = 4, ISBN = "9780140273984", PublishedYear = new DateTime(1991, 4, 1) },
        new Book { BookId = 15, Title = "Faust", AuthorId = 5, ISBN = "9780140449014", PublishedYear = new DateTime(1808, 1, 1) }
    };
}


[ApiController]
[Route("[controller]/[action]")]
public class LibraryController : ControllerBase
{

    private readonly ILogger<LibraryController> _logger;

    public LibraryController(ILogger<LibraryController> logger)
    {
        _logger = logger;
    }


    [HttpGet("{year}")]
    public ActionResult<List<Book>> getBooksByYear(int year, [FromQuery] string order)
    {
        
        var res_books = order.ToLower() == "ascending"
            ? Library.Books
                .Where(x => x.PublishedYear.Year == year)
                .OrderBy(x => x.Title)
            : Library.Books
                .Where(x => x.PublishedYear.Year == year)
                .OrderByDescending(x => x.Title);
        return Ok(res_books);
    }

    [HttpGet]
    public ActionResult<IEnumerable<dynamic>> getAuthorGroupedByYear()
    {
        var groupedAuthors = Library.Authors
            .GroupBy(a => a.BirthDate.Year);
        return Ok(groupedAuthors);
    }

    [HttpGet]
    public ActionResult<IEnumerable<dynamic>> getAuthorGroupedByYearAndCountry()
    {
        var groupedAuthors = Library.Authors
            .GroupBy(a => new { a.BirthDate.Year , a.CountryName
    });

    return Ok(groupedAuthors);
    }

    [HttpGet]
    public ActionResult<long> getNumberOfBooks()
    {
         long count = (from book in Library.Books
            select book).Count();
        return Ok(count);
    }
    
    [HttpGet]
    public ActionResult<List<Book>> getBookPage([FromQuery] int pageSize, [FromQuery] int pageNumber)
    {
        var books = (from book in Library.Books
                select book)
            .Skip(pageSize*(pageNumber-1))
            .Take(pageSize)
            .ToList();
        return Ok(books);
    }
    
}

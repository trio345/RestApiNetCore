using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiNetCore.Models;
using RestApiNetCore.Services;

namespace RestApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet]
        public ActionResult<List<Book>> Get() => _bookService.Get();

        // GET: api/Books/5
        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> Get(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
            var book = _bookService.Get(id);
            if ( book.Id != id)
            {
                return NotFound();
            }
            _bookService.Update(id, bookIn);

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            _bookService.Create(book);   

            return CreatedAtAction("GetBook", new { id = book.Id.ToString() }, book);
        }
        
        // DELETE: api/Books/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            _bookService.Remove(book.Id);
            return NoContent();
        }

        /*private bool BookExists(string id)
        {
            return _bookService.Get(book => bo\);
        }*/
    }
}

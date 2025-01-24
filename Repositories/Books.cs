using System.Collections.Concurrent;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Repositories
{
    public class BookRepository
    {
        private static ConcurrentDictionary<string, Book> _books = new();

        public IEnumerable<Book> GetAllBooks() => _books.Values;

        public Book? GetBookByISBN(string isbn) =>
            _books.TryGetValue(isbn, out var book) ? book : null;

        public IEnumerable<Book> SearchBooksByTitle(string title) =>
            _books.Values.Where(b =>
                b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        public bool AddBook(Book book)
        {
            return _books.TryAdd(book.ISBN, book);
        }

        public bool UpdateBook(string isbn, Book updatedBook)
        {
            if (!_books.ContainsKey(isbn)) return false;
            _books[isbn] = updatedBook;
            return true;
        }

        public bool RemoveBook(string isbn) =>
            _books.TryRemove(isbn, out _);
    }
}
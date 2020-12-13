using System.Threading.Tasks;
using BookInventory.Models;
using System.Collections.Generic;
using System;

namespace BookInventory.Services
{
    public interface IDataService : IDisposable
    {
        #region Book related services

        public Task<bool> BookExists(int? id);
        public Task<Book> GetBook(int? id);

        public Task<IEnumerable<Book>> GetAllBooks();
        public Task<IEnumerable<Book>> GetAllBooksByAuthorName(string author);
        public Task<IEnumerable<Book>> GetAllBooksByTitle(string title);
        public Task<IEnumerable<Book>> GetAllBooksByPublishedYear(int? year);

        public Task<bool> AddBook(Book book);
        public Task<bool> UpdateBook(Book book);
        public Task<bool> ActivateBook(int? id);
        public Task<bool> DeactivateBook(int? id);

        #endregion Book related services

        #region Author related services

        public Task<bool> AuthorExists(int? id);
        public Task<Author> GetAuthor(int? id);

        public Task<IEnumerable<Author>> GetAllAuthors();
        public Task<IEnumerable<Author>> GetAuthorsFilteredByName(string name);

        public Task<bool> AddAuthor(Author author);
        public Task<bool> UpdateAuthor(Author author);
        public Task<bool> ActivateAuthor(int? id);
        public Task<bool> DeactivateAuthor(int? id);

        #endregion Author related services
    }
}

using System.Linq;
using System.Threading.Tasks;
using BookInventory.Models;
using BookInventory.Data.Repository;
using System.Collections.Generic;
using BookInventory.Authorization;

namespace BookInventory.Services
{
    public class DataService : IDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Book related services

        public async Task<bool> BookExists(int? id)
        {
            return await _unitOfWork.Book.Exists(id);
        }

        public async Task<Book> GetBook(int? id)
        {
            return await _unitOfWork.Book.GetFirstOrDefault(filter: b => b.Id == id, includeProperties: "Author");
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _unitOfWork.Book.GetAll(filter: b => b.IsActive == true, orderBy: x => x.OrderBy(b => b.Title), includeProperties: "Author");
        }

        public async Task<IEnumerable<Book>> GetAllAdultBooks()
        {
            return await _unitOfWork.Book.GetAll(filter: b => b.AgeLimit > Policies.AgeLimit && b.IsActive == true, orderBy: x => x.OrderBy(b => b.Title), includeProperties: "Author");
        }

        public async Task<IEnumerable<Book>> GetAllBooksByAuthorName(string author)
        {
            return await _unitOfWork.Book.GetAll(filter: b => b.Author.Name.Contains(author),
                    orderBy: x => x.OrderBy(b => b.Title), includeProperties: "Author");
        }

        public async Task<IEnumerable<Book>> GetAllBooksByTitle(string title)
        {
            return await _unitOfWork.Book.GetAll(filter: b => b.Title.Contains(title),
                    orderBy: x => x.OrderBy(b => b.Title), includeProperties: "Author");
        }

        public async Task<IEnumerable<Book>> GetAllBooksByPublishedYear(int? year)
        {
            if (year == null) return null;
            return await _unitOfWork.Book.GetAll(filter: b => b.PublishedYear == year,
                    orderBy: x => x.OrderBy(b => b.Title), includeProperties: "Author");
        }


        public async Task<bool> AddBook(Book book)
        {
            _unitOfWork.Book.Add(book);
            var result = await _unitOfWork.Save();
            return result;
        }

        public async Task<bool> UpdateBook(Book book)
        {            
            var result = await _unitOfWork.Book.UpdateAndSave(book);
            return result;
        }

        public async Task<bool> ActivateBook(int? id)
        {
            return await _unitOfWork.Book.ActivateAndSave(id);
        }

        public async Task<bool> DeactivateBook(int? id)
        {
            return await _unitOfWork.Book.DeactivateAndSave(id);
        }

        #endregion Book related services

        #region Author related services

        public async Task<bool> AuthorExists(int? id)
        {
            return await _unitOfWork.Author.Exists(id);
        }

        public async Task<Author> GetAuthor(int? id)
        {
            return await _unitOfWork.Author.GetFirstOrDefault(filter: a => a.Id == id, includeProperties: "Books");
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _unitOfWork.Author.GetAll(orderBy: x => x.OrderBy(a => a.Name), includeProperties: "Books");
        }

        public async Task<IEnumerable<Author>> GetAuthorsFilteredByName(string name)
        {
            return await _unitOfWork.Author.GetAll(filter: a => a.Name.Contains(name),
                    orderBy: x => x.OrderBy(a => a.Name), includeProperties: "Books");
        }

        public async Task<bool> AddAuthor(Author author)
        {
            _unitOfWork.Author.Add(author);
            var result = await _unitOfWork.Save();
            return result;
        }

        public async Task<bool> UpdateAuthor(Author author)
        {
            var result = await _unitOfWork.Author.UpdateAndSave(author);
            return result;
        }

        public async Task<bool> ActivateAuthor(int? id)
        {
            return await _unitOfWork.Author.ActivateAndSave(id);
        }

        public async Task<bool> DeactivateAuthor(int? id)
        {
            return await _unitOfWork.Author.DeactivateAndSave(id);
        }

        #endregion Author related services

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }        
    }
}

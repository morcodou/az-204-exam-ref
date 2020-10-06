using LogicAppCustomConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicAppCustomConnector.Helpers
{
    public sealed class BooksSingleton
    {
        private static readonly BooksSingleton m_instance = null;

        private static readonly List<Book> _books;
        public static BooksSingleton Instance
        {
            get
            {
                return m_instance;
            }
        }

        static BooksSingleton()
        {
            m_instance = new BooksSingleton();
            _books = new List<Book>();
        }

        private BooksSingleton()
        {
        }

        public void AddBook(Book book)
        {
            if (book != null)
            {
                _books.Add(book);
            }
        }

        public void ModifyBook(Book book)
        {
            var bookToModify = _books.SingleOrDefault(b => b.Id.Equals(book.Id));
            if (bookToModify != null)
            {
                bookToModify.Author = book.Author;
                bookToModify.Title = book.Title;
            }
        }

        public IEnumerable<Book> GetBooks()
        {
            return _books.ToArray();
        }

        public bool DeleteBookById(string id)
        {
            bool deleted = false;
            Guid guidToRemove = Guid.Parse(id);
            var booktToRemove = _books.SingleOrDefault(b => b.Id.Equals(guidToRemove));
            if (booktToRemove != null)
            {
                _books.Remove(booktToRemove);
                deleted = true;
            }
            return deleted;
        }

        public Book GetBookById(string id)
        {
            Guid guid = Guid.Parse(id);
            return _books.SingleOrDefault(b => b.Id.Equals(guid));
        }
    }
}
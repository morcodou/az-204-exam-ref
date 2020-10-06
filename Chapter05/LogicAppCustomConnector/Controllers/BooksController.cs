using LogicAppCustomConnector.Helpers;
using LogicAppCustomConnector.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using TRex.Metadata;

namespace LogicAppCustomConnector.Controllers
{
    public class BooksController : ApiController
    {
        private readonly BooksSingleton _books = BooksSingleton.Instance;
        private static readonly CallbacksSingleton _callbacks = CallbacksSingleton.
        Instance;

        // Subscribe to newly created books
        [HttpPost, Route("books/subscribe")]
        [Metadata("New book created", "Fires whenever a new book is added to the list.", VisibilityType.Important)]
        [Trigger(TriggerType.Subscription, typeof(Book), "Book")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "Subscription created")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Invalid subscription configuration")]
        public IHttpActionResult Subscribe(Callback callback)
        {
            _callbacks.AddCallback(callback);
            return CreatedAtRoute(nameof(Unsubscribe), new
            {
                subscriptionId = callback.
            Id
            }, string.Empty);
        }

        [HttpDelete, Route("books/subscribe/{callbackId}", Name = nameof
        (Unsubscribe))]
        [Metadata("Unsubscribe", Visibility = VisibilityType.Internal)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult Unsubscribe(string callbackID)
        {
            _callbacks.DeleteCallbackById(callbackID);
            return Ok();
        }

        [HttpGet, Route("books/subscriptions")]
        [Metadata("Get subscriptions", "Get all the current subscriptions")]
        [SwaggerResponse(HttpStatusCode.OK, "An array of subscriptions",
        typeof(Array))]

        public IEnumerable<Callback> GetCallbacks()
        {
            return _callbacks.GetCallbacks();
        }


        // GET api/books
        [HttpGet, Route("books")]
        [Metadata("Get books", "Get all the books objects stored in the App")]
        [SwaggerResponse(HttpStatusCode.OK, "An array of books", typeof(Array))]
        public IEnumerable<Book> Get()
        {
            return _books.GetBooks();
        }

        // GET api/books/5
        [HttpGet, Route("books/{id}", Name = "GetBook")]
        [Metadata("Get single book", "Get a single book object by its id. You can use any GUID valid string")]
        [SwaggerResponse(HttpStatusCode.OK, "An object represeting a book", typeof(Book))]
        public Book Get(string id)
        {
            return _books.GetBookById(id);
        }

        // POST api/books
        [HttpPost, Route("books")]
        [Metadata("Add a new book", "Add a new book object to the system. A value object is compound of a Title and an Author.")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public IHttpActionResult Post([FromBody] Book book)
        {
            _books.AddBook(book);

            foreach (var callback in _callbacks.GetCallbacks())
            {
                callback.InvokeAsync(book);
            }

            return CreatedAtRoute("GetBook", new { id = book.Id }, book);
        }

        // PUT api/books/5
        [HttpPut, Route("books/{id}")]
        [Metadata("Modify an existing book object", "Modify an existing book. You need to provide the new values for the Title or Author of the book.You look for the book object using its id")]
        public void Put([FromBody] Book book)
        {
            _books.ModifyBook(book);
        }

        // DELETE api/books/5
        [Metadata("Delete a book object", "Delete a book object by its id.")]
        [HttpDelete, Route("books/{id}")]
        public void Delete(string id)
        {
            _books.DeleteBookById(id);
        }
    }
}
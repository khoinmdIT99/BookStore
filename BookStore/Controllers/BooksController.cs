namespace BookStore.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using BookStore.Models;

    /// <summary>
    /// Defines the <see cref="BooksController" />.
    /// </summary>
    public class BooksController : Controller
    {
        /// <summary>
        /// Defines the db.
        /// </summary>
        private BookStoreEntities db = new BookStoreEntities();

        // GET: Books
        /// <summary>
        /// The Index.
        /// </summary>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public ActionResult Index()
        {
            Book b = new Book();
            b.Author = "asdas";
            b.Name = "llll";
            b.Count = 100500;
            ViewBag.Massage = "asdasdas";
            ViewBag.b = b;
            return View(db.Books.ToList());
        }

        // GET: Books/Details/5
        /// <summary>
        /// The Details.
        /// </summary>
        /// <param name="id">The id<see cref="int?"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        /// <summary>
        /// The Create.
        /// </summary>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="book">The book<see cref="Book"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Author,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        /// <summary>
        /// The Edit.
        /// </summary>
        /// <param name="id">The id<see cref="int?"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// The Edit.
        /// </summary>
        /// <param name="book">The book<see cref="Book"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Author,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="id">The id<see cref="int?"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        /// <summary>
        /// The DeleteConfirmed.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            //db.Database.SqlQuery<string>("")
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        /// <param name="disposing">The disposing<see cref="bool"/>.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

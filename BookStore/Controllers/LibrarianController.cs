using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    [Authorize]
    public class LibrarianController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();

        // GET: Librarian
        public ActionResult Index()
        {
            SelectList Users = new SelectList(db.Users, "Email", "Email");

            ViewBag.UserID = Users;
            ViewBag.result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.result = "Вы авторизированы как " + User.Identity.Name; 

            }
            return View();
        }

        [HttpPost]
        public ActionResult BookMovements( string Id, string StatusId)
        {
            StringBuilder SQL = new StringBuilder();

            SQL.Append(@" declare @Id int="+ Id+ ",@StatusId int=" + StatusId +
                @" begin tran
  update b
  set Actual = 0
  from BookMovementStatuses b where b.BookMovementId =@Id

    update b
  set CurrentStatus = @StatusId
  from BookMovementS b where b.Id = @Id

  INSERT INTO[dbo].[BookMovementStatuses]
        ([BookMovementId]
          ,[StatusId]

          ,[Actual])
select @Id,@StatusId,1
  commit tran select '1'");
            string strSQL = SQL.ToString();
            List<string> list = BookStoreProvider.db.Database.SqlQuery<string>(strSQL).ToList();



            return View(BookStoreProvider.db.BookMovements.ToList());
        }
     
       
        public ActionResult BookMovements(string UserEmail)
        {
            return PartialView(BookStoreProvider.db.BookMovements.Where(u => u.User.Email == UserEmail || u.User.Email =="").ToList());
        }
    [HttpPost]
        public ActionResult partialBuy(string id, string ch, string countb)
        {
            string res = id.Replace("Check_", "");
            StringBuilder SQL = new StringBuilder();

            SQL.Append("INSERT INTO BookMovements (UserId, BookId, BookMovement, BookCount,CurrentStatus) " +

                "Select Id ," + res + ",1," + countb.ToString() + ",1 from Users where Email = '" + User.Identity.Name + "' Select '1'");
            string strSQL = SQL.ToString();
            List<string> list = BookStoreProvider.db.Database.SqlQuery<string>(strSQL).ToList();



            return View(BookStoreProvider.UserBookRet());
        }
        public ActionResult PartialIndex(string bn)
        {
            return View(BookStoreProvider.db.Books.Where(b => b.Name.Contains(bn)).ToList());
        }
        // GET: Librarian/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Librarian/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Librarian/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Librarian/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Librarian/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Librarian/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Librarian/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

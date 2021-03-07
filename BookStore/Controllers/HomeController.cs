using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Util;
using System.Text;

namespace BookStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();

        public List<UserBook> UserBookRet()
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("exec SelectUserBook @Username='" + User.Identity.Name+"'");
            var UserBooks = db.Database.SqlQuery<UserBook>(SQL.ToString()).ToList();
       
            return UserBooks;
            
        }

        public List<UserBook> UserBookRet(string UserEmail)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("exec SelectUserBook @Username='" + UserEmail + "'");
            var UserBooks = db.Database.SqlQuery<UserBook>(SQL.ToString()).ToList();

            return UserBooks;

        }


        public ActionResult UserCard(string UserEmail)
        {
            var v = UserBookRet(UserEmail);
            return PartialView("~/Views/Home/partialBuy.cshtml", v);

        }
        public ActionResult Index2()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Index2(string Author, string Name)
        {
            return View();
        }
        public ActionResult ListUsers()
        {
         
                SelectList Users = new SelectList(db.Users, "Email", "Email");

                ViewBag.UserID = Users;
          
            return View();//db.Users
        }
        // GET: Users
        public ActionResult PartialIndex(string bn)
        {  
            return View(db.Books.Where(b=>b.Name.Contains(bn)).ToList());
        }
        public ActionResult Index()
        {
            

            ViewBag.result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.result = "Вы авторизированы как " + User.Identity.Name;

                StringBuilder SQL = new StringBuilder();
                SQL.Append("Select Role from Users where Email = '"+User.Identity.Name+"'");
                string strSQL = SQL.ToString();
                List<int> list = db.Database.SqlQuery<int>(strSQL).ToList();
                if (list[0] == 2) {
                    return View("~/Views/Home/Index2.cshtml");
                }
                
            }
            return View();
        }

       
        [HttpGet]
        public ActionResult Buy()
        {
            ViewBag.result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.result = "Вы авторизированы как " + User.Identity.Name;
            }
            return View();
        }
        [HttpPost]
        public ActionResult PartialRetBook(string BookId, string countb) {
         
            StringBuilder SQL = new StringBuilder();

            SQL.Append("INSERT INTO BookMovements (UserId, BookId, BookMovement, BookCount) " +

                "Select Id ," + BookId + ",-1," + countb.ToString() + " from Users where Email = '" + User.Identity.Name + "' Select '1'");
            string strSQL = SQL.ToString();
            List<string> list = db.Database.SqlQuery<string>(strSQL).ToList();


          
            return View("~/Views/Home/partialBuy.cshtml",UserBookRet());
        }
        [HttpPost]
        public ActionResult partialBuy(string id, string ch, string countb)
        {
            string res = id.Replace("Check_","");
            StringBuilder SQL = new StringBuilder();
            
            SQL.Append("BEGIN TRAN "+
                " INSERT INTO BookMovements (UserId, BookId, BookMovement, BookCount,CurrentStatus) " +
               
                " Select Id ," + res + ",1,"+countb.ToString()+",1 from Users where Email = '"+User.Identity.Name+"'" +
                " declare @BookMovementId int"+
                " select @BookMovementId=max(Id) from BookMovements" +
                " update [dbo].[BookMovementStatuses] set Actual = 0 from BookMovementStatuses where BookMovementId=@BookMovementId" +
                " INSERT INTO [dbo].[BookMovementStatuses] ([BookMovementId],[StatusId],[InsertDate],[Actual])"+
                " SELECT @BookMovementId,1,getdate(),1"+
                " Select cast(@BookMovementId as varchar)" +
                " COMMIT TRAN");
            string strSQL = SQL.ToString();
              List<string> list = db.Database.SqlQuery<string>(strSQL).ToList();


         
            return View(UserBookRet());
        }
        public ActionResult partialBuy()
        {
          
            return View(UserBookRet());
        }
        public ActionResult Buy(int id)
        {
            ViewBag.result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.result = "Вы авторизированы как " + User.Identity.Name;
            }
            ViewBag.bookId = id;
            return View();
        }

        public string Square()
        {
            int a = Int32.Parse(Request.Params["a"]);
            int h = Int32.Parse(Request.Params["h"]);
            double s = a * h / 2.0;
            return "<h2>Площадь треугольника с основанием " + a + " и высотой " + h + " равна " + s + "</h2>";
        }
      
        public ActionResult About()
        {
            ViewBag.result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.result = "Вы авторизированы как " + User.Identity.Name;
            }
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
          if(  db.Users.Where(U => U.Role == 2).Where(W => W.Email == User.Identity.Name).ToList().Count > 0)
            {
                ViewBag.result = "Вы авторизированы как " + User.Identity.Name;
            }
          else
                ViewBag.result = "Вы не авторизованы";

            return View();
        }

        public ActionResult GetHtml()
        {
            return new HtmlResult("<h2>Привет мир!</h2>");
        }
        
        [HttpPost]
        public string PartialReceiveBook(string BookMovementId, string statusId )
        {

            StringBuilder SQL = new StringBuilder();

            SQL.Append("exec ReceiveBook  @BookMovementId = "+BookMovementId+", @statusId = "+ statusId);
            string strSQL = SQL.ToString();
            List<string> list = db.Database.SqlQuery<string>(strSQL).ToList();



            return list[0];
        }
        public ActionResult SelectRetBooks(int BookId, int StatusId)
        {
            return View(BookStoreProvider.db.BookMovements.Where(b => (b.BookId == BookId) && (b.CurrentStatus == StatusId)).ToList());
        }
    }
}
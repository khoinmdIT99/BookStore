using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            ViewBag.result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.result = "Вы авторизированы как " + User.Identity.Name;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            ViewBag.result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.result = "Вы авторизированы как " + User.Identity.Name;
            }
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                using (BookStoreEntities db = new BookStoreEntities())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == model.Name && u.Password == model.Password);

                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    Session["UserId"] = user.Id;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            ViewBag.result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.result = "Вы авторизированы как " + User.Identity.Name;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (BookStoreEntities db = new BookStoreEntities())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == model.Name);
                }
                if (user == null)
                {
                    // создаем нового пользователя
                    using (BookStoreEntities db = new BookStoreEntities())
                    {
                        db.Users.Add(new User { Email = model.Name, Password = model.Password, Age = model.Age });
                        db.SaveChanges();

                        user = db.Users.Where(u => u.Email == model.Name && u.Password == model.Password).FirstOrDefault();
                    }
                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }
        public ActionResult Logoff()
        {
            ViewBag.result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.result = "Вы авторизированы как " + User.Identity.Name;
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


    }
}
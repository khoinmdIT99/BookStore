using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BookStore.Models
{


    public static class BookStoreProvider
    {
        const string BookStoreContextKey = "BookStoreContext";
        public static BookStoreEntities db
        {
            get
            {
                if (HttpContext.Current.Items[BookStoreContextKey] == null)
                {
                    BookStoreEntities dfe = new BookStoreEntities();
                    //  string REMOTE_ADDR =  HttpContext.Current.Items["REMOTE_ADDR"].ToString();
                    string UserHostAddress = "";
                    try
                    {

                        UserHostAddress = HttpContext.Current.Request != null ? HttpContext.Current.Request.UserHostAddress : "";
                    }
                    catch { }
                  //  string ConnectionString = dfe.Database.Connection.ConnectionString;
                  //  string UserName = HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "";
                  //  ConnectionString = ConnectionString.Replace("application name=DocFlowEntities", "application name=" + UserName + "\\" + UserHostAddress + "\\" + "DocFlowEntities");

                  //  dfe.Database.Connection.ConnectionString = ConnectionString;
                    HttpContext.Current.Items[BookStoreContextKey] = dfe;
                }
                return (BookStoreEntities)HttpContext.Current.Items[BookStoreContextKey];
            }
        }
        public static List<UserBook> UserBookRet()
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("exec SelectUserBook @Username='" + HttpContext.Current.User.Identity.Name + "'");
            var UserBooks = db.Database.SqlQuery<UserBook>(SQL.ToString()).ToList();

            return UserBooks;

        }
        public static List<Role> Role()
        {     
            return db.Roles.ToList();
        }
    }
    }
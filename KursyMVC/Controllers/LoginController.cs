using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KursyMVC.Controllers
{
    public class LoginController : Controller
    {
        private Models.LoginEntities db = new Models.LoginEntities();
        // GET: Login
        public ActionResult Index()
        {
            try
            {
                Session["UserID"] = null;
            }
            catch { }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Models.Students objUser)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    if (db.Students.Any(x => x.StudentName == objUser.StudentName && x.Password == objUser.Password))
                    {
                        var temp = db.Students.First(x =>
                            x.StudentName == objUser.StudentName && x.Password == objUser.Password);
                        Session["UserID"] = Convert.ToInt32(temp.StudentID);
                        return RedirectToAction("Index", "Student");
                    }
                    else
                        ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(objUser);
        }
    }
}
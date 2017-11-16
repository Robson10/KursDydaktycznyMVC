using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KursyMVC.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student - KategoryList
        string _connectionString = @"Data Source=DESKTOP-M4NPT9R; Initial Catalog=Database_1;Integrated Security=SSPI";
        private Models.CategoryEntities dbCategory = new Models.CategoryEntities();
        private Models.CategoryContentEntities dbContent = new Models.CategoryContentEntities();
        private Models.TutorialEntities dbTutorial = new Models.TutorialEntities();
        private Models.TestEntities dbTest = new Models.TestEntities();

        public ActionResult Index()
        {
            return View(dbCategory.Courses.ToList());
        }
        public ActionResult CategoryContent(int id, string name)
        {
            Session["CategoryID"] = id;
            Session["Kategoria"] = name;
            return View(dbContent.CourseSections.Where(s => s.Courses.CourseID == id));
        }
        public ActionResult Tutorial(int id)
        {
            return View(dbTutorial.SectionContents.Where(s => s.CourseSections.CourseSectionID == id));
        }
        public ActionResult Test(int id)
        {
            int categoryId = (int)Session["CategoryID"];
            var temp = dbTest.SectionQuestions.Where(s => s.CourseSectionID == id).ToList();

            questionList result = new questionList();
            for (int i = 0; i < temp.Count(); i++)
            {
                question q = new question();
                q.QuestionText = temp[i].QuestionText;
                q.CorrectAnswer = temp[i].CorrectAnswer;
                int SectionQuestionID = (int)temp[i].SectionQuestionID;
                var temp2 = dbTest.QuestionAnswers.Where(s => s.SectionQuestionID == SectionQuestionID).ToList();
                for (int j = 0; j < 1; j++)
                {
                    q.Checkboxes = false;//.Add(false);
                    q.Answers= temp2[j].AnswerText;//.Add(temp2[j].AnswerText);
                }
                result.Add(q);
            }

            return View(result[0]);
        }
        [HttpPost]
        public ActionResult Test(question model)
        {
            if (ModelState.IsValid)
            {
                var temp = model;
            }

            return View(model);
        }

        public ActionResult Results(int id)
        {
            int categoryId = (int)Session["CategoryID"];
            return View(dbContent.CourseSections.Where(s => s.Courses.CourseID == id));
        }

    }


    public class questionList : List<question>
    {

    }
    public class question
    {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public bool Checkboxes { get; set; } //= new List<bool>();
        public string Answers { get; set; } //= new List<string>();
    }
}
using KursyMVC.Models;
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
        private Models.ResultsEntities dbResults = new Models.ResultsEntities();

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
            Session["TestID"] = id;
            int pytanieNR;
            if (Session["PytanieNR"] != null)
            {
                pytanieNR = (int) Session["PytanieNR"];
                pytanieNR++;
                Session["PytanieNR"] = pytanieNR;
            }
            else
            {
                pytanieNR = 0;
                Session["PytanieNR"] = pytanieNR;
            }
            var pytania = dbTest.SectionQuestions.Where(s => s.CourseSectionID == id).ToList();
            if (pytanieNR >= pytania.Count)
            {
                //wynik testu
                //RedirectToAction("TestResult", "Student");
                return RedirectToAction("TestResult", "Student");
            }
            else
            {
                question q = new question();
                q.QuestionText = pytania[pytanieNR].QuestionText;
                q.CorrectAnswer = pytania[pytanieNR].CorrectAnswer;
                int SectionQuestionID = (int) pytania[pytanieNR].SectionQuestionID;
                var temp2 = dbTest.QuestionAnswers.Where(s => s.SectionQuestionID == SectionQuestionID).ToList();
                for (int j = 0; j < 3; j++)
                {
                    q.Checkboxes.Add(false); //.Add(false);
                    q.Answers.Add(temp2[j].AnswerText); //.Add(temp2[j].AnswerText);
                }
                return View(q);
            }
        }

        [HttpPost]
        public ActionResult Test(question model)
        {
            int testID = (int) Session["TestID"];
            Session["Odp" + Session["PytanieNR"]] = model.Checkboxes;
            ModelState.Clear();
            return Test(testID);
        }

        public ActionResult TestResult()
        {
            int testID = (int) Session["TestID"];
            int pytanieNR = (int) Session["PytanieNR"];
            int studentID = (int) Session["UserID"];
            int courseSectionID = (int) Session["CategoryID"];
            int score = 0;
            int totalQuestions = pytanieNR;
            // string dateTaken = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            var pytania = dbTest.SectionQuestions.Where(s => s.CourseSectionID == courseSectionID).ToList();
            for (int i = 0; i < pytanieNR; i++)
            {
                var temp = (List<bool>) Session["Odp" + i];
                if (pytania[i].CorrectAnswer.Equals("A"))
                    score += (temp[0]) ? 1 : 0;
                else if (pytania[i].CorrectAnswer.Equals("B"))
                    score += (temp[1]) ? 1 : 0;
                else if (pytania[i].CorrectAnswer.Equals("C"))
                    score += (temp[2]) ? 1 : 0;
            }
            dbResults.QuizResults.Add(
                new Models.QuizResults()
                {
                    CourseSectionID = courseSectionID,
                    Score = score,
                    StudentID = studentID,
                    TotalQuestions = totalQuestions,
                    DateTaken = DateTime.Now //zamiana w bazie typu na time oraz autoincrement na kluczu głównym
                });
            dbResults.SaveChanges();
            Session["Result"] = "Odpowiedziałeś poprawnie na " + score + " pytania z " + totalQuestions;
            return View();
        }

        public ActionResult Results()
        {
            string ConnectionString =
                @"data source=DESKTOP-M4NPT9R;initial catalog=Kursy;Integrated Security=SSPI"; //important!!!
            string query =
                "SELECT Courses.CourseName,CourseSections.SectionName,QuizResults.Score, QuizResults.TotalQuestions " +
                "FROM Kursy.dbo.QuizResults " +
                "inner join Students on Students.StudentID = QuizResults.StudentID " +
                "inner join CourseSections on CourseSections.CourseSectionID = QuizResults.CourseSectionID " +
                "inner join Courses on Courses.CourseID = CourseSections.CourseID " +
                "where Students.StudentID = " + Session["UserID"];
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.ExecuteNonQuery();
                conn.Close();
                DataSet ds = new DataSet();
                da.Fill(ds);
                List<Models.Result> temp = new List<Result>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    temp.Add(new Result()
                    {
                        CourseName = (string) ds.Tables[0].Rows[i][0],
                        SectionName = (string) ds.Tables[0].Rows[i][1],
                        Score = ((Int32) ds.Tables[0].Rows[i][2]).ToString(),
                        TotalQuestions = ((Int32) ds.Tables[0].Rows[i][3]).ToString()
                    });
                }
                return View(temp);
            }
        }
    }

}


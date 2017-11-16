using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KursyMVC.Models
{
    public class Result
    {
        public string Score { get; set; }
        public string TotalQuestions { get; set; }
        public string CourseName { get; set; }
        public string SectionName { get; set; }
    }
}
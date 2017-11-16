using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KursyMVC.Models
{
    public class question
    {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public List<bool> Checkboxes { get; set; } = new List<bool>();
        public List<string> Answers { get; set; } = new List<string>();
    }
}
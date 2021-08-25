using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestCaseMVC.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Display(Name = "Project Name")]
        public string Name { get; set; }
        [Display(Name = "Project Author")]
        public string Author { get; set; }
        public int Stargazers { get; set; }
        public int Watchers { get; set; }
        public string Url { get; set; }
    }

    public class GitJSON
    {
        public List<Project> projectList { set; get; }
    }
}

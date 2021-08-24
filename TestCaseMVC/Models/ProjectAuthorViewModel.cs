using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace TestCaseMVC.Models
{
    public class ProjectAuthorViewModel
    {
        public List<Project> Projects { get; set; }
        public SelectList Authors { get; set; }
        public string ProjectAuthor { get; set; }
        public string SearchString { get; set; }

    }
}

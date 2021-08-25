using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestCaseMVC.Data;
using TestCaseMVC.Models;

namespace TestCaseMVC.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly TestCaseMVCContext _context;

        public ProjectsController(TestCaseMVCContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index(string searchString, string projectAuthor)
        {
            IQueryable<string> authorQuery = from p in _context.Project
                                             orderby p.Author
                                             select p.Author;

            var projects = from p in _context.Project
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name.Contains(searchString));
                if (projects.Count() == 0)
                {
                    string url = $"https://api.github.com/search/repositories?q={searchString}";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    request.Headers.Add("Accept-Language", "ru,en-GB;q=0.9,en;q=0.8,en-US;q=0.7");
                    request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36 Edg/92.0.902.78";

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        var reader = new StreamReader(dataStream);
                        string responseFromServer = reader.ReadToEnd();
                    }
                        
                    response.Close();
                }
            }

            if (!string.IsNullOrEmpty(projectAuthor))
            {
                projects = projects.Where(x => x.Author == projectAuthor);
            }

            var projectAuthorVM = new ProjectAuthorViewModel
            {
                Authors = new SelectList(await authorQuery.Distinct().ToListAsync()),
                Projects = await projects.ToListAsync()
            };
            return View(projectAuthorVM);
        }
      
        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Author,Stargazers,Watchers,Url")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Author,Stargazers,Watchers,Url")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.Id == id);
        }
    }
}

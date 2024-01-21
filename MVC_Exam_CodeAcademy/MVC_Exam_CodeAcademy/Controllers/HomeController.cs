using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Exam_CodeAcademy.DAL;
using MVC_Exam_CodeAcademy.Models;
using System.Diagnostics;

namespace MVC_Exam_CodeAcademy.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.Where(b => b.IsDeleted == false).ToListAsync();
            return View(blogs);
        }
    }
}

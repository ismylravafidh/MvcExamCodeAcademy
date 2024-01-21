using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Exam_CodeAcademy.Areas.Manage.ViewModels;
using MVC_Exam_CodeAcademy.DAL;
using MVC_Exam_CodeAcademy.Models;
using MVC_Exam_CodeAcademy.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace MVC_Exam_CodeAcademy.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize (Roles = "Admin")]
    public class BlogController : Controller
    {
        AppDbContext _context;
        IWebHostEnvironment _env;
        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.Where(b=>b.IsDeleted==false).ToListAsync();
            return View(blogs);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateVm createVm)
        {
            if (!ModelState.IsValid)
            {
                return View(createVm);
            }
            var resultImage = createVm.Image.CheckImage(3);
            if (!resultImage)
            {
                ModelState.AddModelError("", "Yalniz Sekil Yukluye Bilersiz, Max olcu 3mb.");
                return View();
            }
            var resultLogo = createVm.Logo.CheckImage(3);
            if (!resultLogo)
            {
                ModelState.AddModelError("", "Yalniz Sekil Yukluye Bilersiz, Max olcu 3mb.");
                return View();
            }
            createVm.ImageUrl =await createVm.Image.UploadImageAsync(_env.WebRootPath, @"\Upload\BlogImages\");
            createVm.LogoUrl =await createVm.Logo.UploadImageAsync(_env.WebRootPath, @"\Upload\AuthorLogos\");

            Blog blog = new Blog()
            {
                Title = createVm.Title,
                Description = createVm.Description,
                Author = createVm.Author,
                ImageUrl = createVm.ImageUrl,
                LogoUrl = createVm.LogoUrl,
            };
            await _context.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Blog");
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            Blog blog = await _context.Blogs.Where(b=> b.IsDeleted==false && b.Id==id).FirstOrDefaultAsync();
            if (blog == null)
            {
                return BadRequest();
            }
            BlogUpdateVm vm = new BlogUpdateVm()
            {
                Title=blog.Title,
                Description=blog.Description,
                Author = blog.Author,
                ImageUrl = blog.ImageUrl,
                LogoUrl = blog.LogoUrl,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(BlogUpdateVm updateVm)
        {
            if (!ModelState.IsValid)
            {
                return View(updateVm);
            }
            if (updateVm.Image != null)
            {
                var resultImage = updateVm.Image.CheckImage(3);
                if (!resultImage)
                {
                    ModelState.AddModelError("", "Yalniz Sekil Yukluye Bilersiz, Max olcu 3mb.");
                    return View();
                }
                updateVm.ImageUrl = await updateVm.Image.UploadImageAsync(_env.WebRootPath, @"\Upload\BlogImages\");
            }
            if (updateVm.Logo != null)
            {
                var resultLogo = updateVm.Logo.CheckImage(3);
                if (!resultLogo)
                {
                    ModelState.AddModelError("", "Yalniz Sekil Yukluye Bilersiz, Max olcu 3mb.");
                    return View();
                }
                updateVm.LogoUrl = await updateVm.Logo.UploadImageAsync(_env.WebRootPath, @"\Upload\BlogImages\");
            }
            Blog oldBlog = await _context.Blogs.Where(b=>b.Id==updateVm.Id).FirstOrDefaultAsync();
            oldBlog.Title= updateVm.Title;
            oldBlog.Description= updateVm.Description;
            oldBlog.Author= updateVm.Author;
            if(updateVm.LogoUrl != null)
            {
                oldBlog.LogoUrl= updateVm.LogoUrl;
            }
            if (updateVm.ImageUrl != null)
            {
                oldBlog.ImageUrl = updateVm.ImageUrl;
            }
            _context.Update(oldBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Blog");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if (id <= 0)
            {
                return NotFound();
            }
            Blog blog = await _context.Blogs.Where(b=>b.Id==id && b.IsDeleted==false).FirstOrDefaultAsync();
            if(blog == null)
            {
                return BadRequest();
            }
            blog.IsDeleted=true;
            _context.Update(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Blog");
        }
    }
}

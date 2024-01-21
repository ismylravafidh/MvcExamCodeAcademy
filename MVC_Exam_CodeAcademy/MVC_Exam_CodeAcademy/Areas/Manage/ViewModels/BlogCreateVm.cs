namespace MVC_Exam_CodeAcademy.Areas.Manage.ViewModels
{
    public class BlogCreateVm
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public IFormFile Image { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile Logo { get; set; }
        public string? LogoUrl { get; set; }
    }
}

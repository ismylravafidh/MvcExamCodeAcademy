using MVC_Exam_CodeAcademy.Models.Base;

namespace MVC_Exam_CodeAcademy.Models
{
    public class Blog : BaseEntity
    {
        public string Title {  get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string LogoUrl { get; set; }
    }
}

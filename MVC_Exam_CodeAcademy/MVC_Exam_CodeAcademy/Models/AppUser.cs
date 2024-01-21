using Microsoft.AspNetCore.Identity;

namespace MVC_Exam_CodeAcademy.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}

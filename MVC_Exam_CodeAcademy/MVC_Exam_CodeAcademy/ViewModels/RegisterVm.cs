using System.ComponentModel.DataAnnotations;

namespace MVC_Exam_CodeAcademy.ViewModels
{
    public class RegisterVm
    {
        [Required(ErrorMessage ="Name Bos Ola Bilmez!")]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname Bos Ola Bilmez!")]
        [MinLength(3)]
        [MaxLength(30)]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Email Bos Ola Bilmez!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "UserName Bos Ola Bilmez!")]
        [MinLength(3)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Bos Ola Bilmez!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}

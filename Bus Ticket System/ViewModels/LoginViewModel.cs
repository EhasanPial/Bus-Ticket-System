using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}

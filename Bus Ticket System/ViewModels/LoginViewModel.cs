using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_System.ViewModels
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

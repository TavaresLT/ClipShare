using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClipShare.ViewModels.Account
{
    public class LoginViewModel : ViewModel
    {
        private string _userName;
        [DisplayName("Username or Email")]
        [Required(ErrorMessage = "Username is required")]
        public string UserName { 
            get => _userName; 
            set => _userName = !string.IsNullOrEmpty(value) ? value.ToLower() : null; 
        }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}

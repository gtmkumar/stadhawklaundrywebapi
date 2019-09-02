using StadhwkLaundry.MoboApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace StadhwkLaundry.MoboApp.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string EmailId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} charaters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public ICommand RegistorCommand
        {
            get
            {
                return new Command(async () =>
                {
                    LoginApiService services = new LoginApiService();
                    await services.userLoginAsync(EmailId, Password);
                });
            }

        }
    }
}

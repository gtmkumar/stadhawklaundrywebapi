using StadhwkLaundry.MoboApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace StadhwkLaundry.MoboApp.ViewModel
{
    public class RegistorViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string EmailId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} charaters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Contactno { get; set; }

        public ICommand RegistorCommand
        {
            get {
                return new Command(async () =>
                {
                    ApiServices services = new ApiServices();
                  await services.RegistorUserAsync(EmailId, Password, contactNo: Contactno);
                });
            }

        }

    }
}

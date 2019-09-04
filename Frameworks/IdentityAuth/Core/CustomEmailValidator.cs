//using IdentityAuth.Managers;
//using IdentityAuth.Models;
//using Microsoft.AspNetCore.Identity;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace IdentityAuth.Core
//{
//    public class CustomEmailValidator : IIdentityValidator<AppIdentityUser>
//    {
//        private readonly string _Expresstion;
//        private readonly string _ErrorMessage;
//        private readonly AppUserManager _manager;

//        public CustomEmailValidator(AppUserManager manager, string expression, string errorMessage)
//        {
//            _Expresstion = expression;
//            _ErrorMessage = errorMessage;
//            _manager = manager;
//        }

//        public async Task<IdentityResult> ValidateAsync(AppIdentityUser item)
//        {
//            string EmailPattern = _Expresstion;//@"^(?=.*[0-9])(?=.*[!@#$%^&*])[0-9a-zA-Z!@#$%^&*0-9]{10,}$";
//            var errors = new List<string>();

//            if (!System.Text.RegularExpressions.Regex.IsMatch(item.Email, EmailPattern))
//                errors.Add(_ErrorMessage);

//            if (_manager != null)
//            {
//                var otherAccount = await _manager.FindByEmailAsync(item.Email);
//                if (otherAccount != null && otherAccount.Id != item.Id)
//                    errors.Add("Select a different email Id. An account has already been created with this Email id.");
//            }

//            return errors.Any()
//                ? IdentityResult.Failed(errors.ToArray())
//                : IdentityResult.Success;
//        }
//    }
//}

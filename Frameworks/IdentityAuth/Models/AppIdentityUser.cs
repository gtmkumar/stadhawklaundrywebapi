//using System;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using System.Collections.Generic;

//namespace IdentityAuth.Models
//{
//    public class AppIdentityUser : IdentityUser
//    {
//        public AppIdentityUser()
//        {
//            Id = Guid.NewGuid();
//            this.ForgotLinks = new List<AspForgotLinks>();
//            PreviousPasswords = new List<AspPreviousPasswords>();
//            LoginHistory = new List<AspLoginHistory>();
//        }

//        public string FirstName { get; set; }

//        public string LastName { get; set; }

//        public DateTime CreateDate { get; set; }

//        public Guid CreatedBy { get; set; }

//        public DateTime? LastLogin { get; set; }

//        public int PassIncorrectNoOfTimes { get; set; }

//        public Guid CompanyID { get; set; }

//        public string PasswordQuestion { get; set; }

//        public string PasswordAnswer { get; set; }

//        public bool? IsApproved { get; set; }

//        public bool? IsDeactive { get; set; }

//        public string ModifyBy { get; set; }

//        public Nullable<System.DateTime> ModifyDate { get; set; }

//        //public string TableName { get; set; }

//        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(AppUserManager manager)
//        {
//            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
//            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
//            // Add custom user claims here
//            return userIdentity;
//            // return null;
//        }

//        public virtual IList<AspForgotLinks> ForgotLinks { get; set; }

//        public virtual ICollection<AspLoginHistory> LoginHistory { get; set; }

//        public virtual ICollection<AspPreviousPasswords> PreviousPasswords { get; set; }
//    }
//}

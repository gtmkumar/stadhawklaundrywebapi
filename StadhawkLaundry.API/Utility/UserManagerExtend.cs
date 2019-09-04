using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using StadhawkLaundry.API.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace StadhawkLaundry.API.Utility
{
    //public class UserManagerExtend : UserManager<ApplicationUser>
    //{
    //    public UserManagerExtend(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    //    {

    //    }

    //    public virtual async Task<ApplicationUser> FindByPhoneAsync(string phoneNo)
    //    {
    //        ThrowIfDisposed();
    //        IUserPhoneNumberStore<ApplicationUser> store = GetPhoneStore();
    //        if (phoneNo == null)
    //        {
    //            throw new ArgumentNullException("email");
    //        }
    //        phoneNo = NormalizeKey(phoneNo);
    //        ApplicationUser val = await store.GetPhoneNumberAsync(phoneNo, CancellationToken);
    //        if (val == null && Options.Stores.ProtectPersonalData)
    //        {
    //            ILookupProtectorKeyRing service = _services.GetService<ILookupProtectorKeyRing>();
    //            ILookupProtector protector = _services.GetService<ILookupProtector>();
    //            if (service != null && protector != null)
    //            {
    //                foreach (string allKeyId in service.GetAllKeyIds())
    //                {
    //                    val = await store.FindByEmailAsync(protector.Protect(allKeyId, email), CancellationToken);
    //                    if (val != null)
    //                    {
    //                        return val;
    //                    }
    //                }
    //            }
    //        }
    //        return val;
    //    }

    //    private IUserPhoneNumberStore<ApplicationUser> GetPhoneStore(bool throwOnFail = true)
    //    {
    //        IUserPhoneNumberStore<ApplicationUser> userPhoneStore = Store as IUserPhoneNumberStore<ApplicationUser>;
    //        if (throwOnFail && userPhoneStore == null)
    //        {
    //            throw new NotSupportedException(ChangePhoneNumberTokenPurpose);
    //        }
    //        return userPhoneStore;
    //    }
    //}
}

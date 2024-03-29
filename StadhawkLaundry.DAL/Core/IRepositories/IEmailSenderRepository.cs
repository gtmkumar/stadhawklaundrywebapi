﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface IEmailSenderRepository
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

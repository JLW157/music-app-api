﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.GeneralModels;

namespace MusicAppApi.Core.interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Message message);
    }
}

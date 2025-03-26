﻿using OnlineMusicProject.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace OnlineMusicProject.Services
{
    public interface IMail
    {
        public Task SendEmail(Message message);
        public Task SendMailConfirmAsync(string email, string urlUser);
        public Task SendMailForgotPassWord(string email, string token);
    }
}

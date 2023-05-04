using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MusicAppApi.Core.Interfaces;
using MusicAppApi.Models.Configurations;
using MusicAppApi.Models.GeneralModels;

namespace MusicAppApi.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(IOptions<EmailConfiguration> options)
        {
            _emailConfiguration = options.Value;
        }

        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await Send(emailMessage);
        }


        #region Private methods

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password);

                await client.SendAsync(mailMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred - {e.Message}");
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
        #endregion
    }
}

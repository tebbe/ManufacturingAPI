using PPS.Shared.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.Shared.Service.Vm;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Configuration;

namespace PPS.Shared.Service.Services
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendRegisterEmail(EmailVm vm)
        {
            var fromEmailPassword = ConfigurationManager.AppSettings["EmailForRegisterPassword"];
            var fromAddress = ConfigurationManager.AppSettings["EmailForRegister"];

            var emailFromAddress = new MailAddress(fromAddress, "Do not reply");
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailFromAddress.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(emailFromAddress, vm.ToAddress)
            {
                Subject = vm.Subject,
                Body = vm.Body
            })
            {
                message.IsBodyHtml = true;
                await smtp.SendMailAsync(message);
            }
            return true;
        }
    }
}
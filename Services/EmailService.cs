using MailKit.Net.Smtp;
using MimeKit;
using System.IO;
using System.Threading.Tasks;


namespace DonationManagmentServer.Services
{
public class EmailService
    {
        private readonly string smtpServer = "smtp.office365.com";  
        private readonly int smtpPort = 587; 
        private readonly string smtpUser = "";
        private readonly string smtpPass = ""; 

        public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, IFormFile file)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", smtpUser));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { TextBody = body };

            if (file != null && file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    bodyBuilder.Attachments.Add(file.FileName, stream.ToArray(), ContentType.Parse(file.ContentType));
                }
            }

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUser, smtpPass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }

}


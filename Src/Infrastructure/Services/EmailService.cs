using Application.Abstractions.Services;
using Application.Abstracts.Services;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    public async Task SendAsync(string to, string subject, string body)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("yourmail@gmail.com", "your_app_password"),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("yourmail@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(to);

        await client.SendMailAsync(mailMessage);
    }
}

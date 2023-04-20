using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly AuthMessageSenderOptions _options;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
    {
        _options = optionsAccessor.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(_options.GmailUserName, _options.GmailPassword);
        smtpClient.EnableSsl = true;

        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_options.GmailUserName);
        mailMessage.To.Add(email);
        mailMessage.Subject = subject;
        mailMessage.Body = htmlMessage;
        mailMessage.IsBodyHtml = true;

        await smtpClient.SendMailAsync(mailMessage);
    }
}

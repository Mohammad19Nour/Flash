using System.Net.Mail;
using ProjectP.Interfaces;

namespace ProjectP.Services;
public class EmailService : IEmailService
{
    private string _senderEmail;
    private  string  _senderPassword;

    public EmailService(IConfiguration configuration)
    {
        var mailSettings = configuration.GetSection("MailSettings");
        _senderEmail = mailSettings["SenderEmail"]!;
        _senderPassword = mailSettings["SenderPassword"]!;
    }

    public async Task <bool> SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var mail = new MailMessage();
        mail.To.Add(email);
        mail.From = new MailAddress(_senderEmail);
        mail.Subject = subject;
        mail.Body = htmlMessage;
        mail.IsBodyHtml = true;
        
        var smtp = new SmtpClient("smtp.gmail.com", 587);
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new System.Net.NetworkCredential(_senderEmail, _senderPassword);

        try
        {
             await Task.Run(() => smtp.Send(mail));
             smtp.Dispose();
             mail.Dispose();
             return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("\n\n\nFailed to send email to " + email);
            return false;
        }
    }
    
}
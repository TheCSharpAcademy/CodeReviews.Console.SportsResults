using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailSenderLib;

public class MailSender
{
    string SmtpAddress;
    int PortNumber;
    bool EnableSsl;
    string EmailFromAddress;
    string Password;
    string EmailToAddress;
    string Subject;
    string Body;

    private Task? task;

    public MailSender(string SmtpAddress, int PortNumber, bool EnableSsl, 
                    string EmailFromAddress, string Password, string EmailToAddress, 
                    string Subject, string Body)
    {
        this.SmtpAddress = SmtpAddress;
        this.PortNumber = PortNumber;
        this.EnableSsl = EnableSsl;
        this.EmailFromAddress = EmailFromAddress;
        this.Password = Password;
        this.EmailToAddress = EmailToAddress;
        this.Subject = Subject;
        this.Body = Body;
    }

    public void SendMail()
    {
        try
        {
            using (MailMessage mail = new MailMessage())
            {
                // Email addresses
                mail.From = new MailAddress(this.EmailFromAddress);
                mail.To.Add(this.EmailToAddress);

                // Email content
                mail.Subject = this.Subject;
                mail.Body = this.Body;
                mail.IsBodyHtml = true;

                // Configure SMTP
                using (SmtpClient smtp = new SmtpClient(this.SmtpAddress, this.PortNumber))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(this.EmailFromAddress, this.Password);
                    smtp.EnableSsl = this.EnableSsl; 
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.Send(mail);
                    Console.WriteLine("Email sent successfully.");
                }
            }
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"SMTP error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
        }
    }

    public void SendMailAsync()
    {
        this.task = new Task(() => SendMail());
        this.task.Start();
    }

    public void EnsureTaskIsFinished()
    {
        if (this.task is not null) { this.task.Wait(); }
    }
}
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using Microsoft.Extensions.Options;

namespace backend.Services.impl {
    public class EmailServiceImpl : EmailService {


        // gmail
        // smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        public void sendEmail(string to, string subject, string body, string from = null) {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("axel.nienow@ethereal.email", "paaYrG2MDE2Fd4srGx");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}

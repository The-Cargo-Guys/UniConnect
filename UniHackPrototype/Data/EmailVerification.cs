using System;
using System.Net.Mail;

namespace UniHackPrototype.Data
{
    public class EmailVerification
    {
        private const string SenderEmail = "noreply@yourdomain.com";

        public bool VerifyAndSendEmail(string emailAddress)
        {
            if (IsValidUtsEmail(emailAddress))
            {
                SendVerificationEmail(emailAddress);
                return true;
            }
            return false;
        }

        private bool IsValidUtsEmail(string email)
        {
            return email.EndsWith("@uts.edu.au", StringComparison.OrdinalIgnoreCase);
        }

        private void SendVerificationEmail(string recipientEmail)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(SenderEmail);
                mail.To.Add(recipientEmail);
                mail.Subject = "Email Verification";
                mail.Body = "Your email has been verified successfully.";

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send verification email: {ex.Message}");
            }
        }
    }
}

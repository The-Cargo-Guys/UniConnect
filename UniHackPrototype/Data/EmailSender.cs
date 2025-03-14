using System.Net.Mail;

namespace UniHack.Data
{
    public static class EmailSender
    {
        private const string SenderEmail = "place holder Email for sender";
        public static void SendEmail(
            string recipientEmail, 
            string body,
            string Subject)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(SenderEmail);
                mail.To.Add(recipientEmail);
                mail.Subject = Subject;
                mail.Body = body;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}

using System.Net;
using System.Net.Mail;

namespace UniHack.Data
{
	public static class EmailSender
	{
		private const string SmtpServer = "mail253.mailasp.net";
		private const int SmtpPort = 587;
		private const string SenderEmail = "notifications@uniconnect.dev";
		private const string SenderPassword = "notifications";
		private const string SenderName = "UniConnect";

		public static void SendEmail(
			string recipientEmail,
			string body,
			string subject)
		{
			try
			{
				var mail = new MailMessage
				{
					From = new MailAddress(SenderEmail, SenderName),
					Subject = subject,
					Body = body,
					IsBodyHtml = true
				};

				mail.To.Add(recipientEmail);

				var smtpClient = new SmtpClient(SmtpServer)
				{
					Port = SmtpPort,
					EnableSsl = true,
					Credentials = new NetworkCredential(SenderEmail, SenderPassword),
					DeliveryMethod = SmtpDeliveryMethod.Network
				};

				smtpClient.Send(mail);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to send email: {ex.Message}");
			}
		}

		public static void SendEventNotification(
			string recipientEmail,
			string recipientName,
			string eventName,
			string eventDate,
			string eventDescription,
			bool isDayBefore)
		{
			string subject = isDayBefore ?
				$"Reminder: {eventName} is tomorrow" :
				$"Reminder: {eventName} starts in 1 hour";

			string body = EmailTemplates.GenerateEventNotificationEmail(
				recipientName,
				eventName,
				eventDate,
				eventDescription,
				isDayBefore);

			SendEmail(recipientEmail, body, subject);
		}
	}
}
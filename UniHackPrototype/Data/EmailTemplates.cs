using System;
using System.IO;
using System.Xml;

namespace UniHack.Data
{
	public static class EmailTemplates
	{
		public static string GenerateEventNotificationEmail(string recipientName, string eventName, string eventDate, string eventDescription, bool isDayBefore)
		{
			string templateXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<EmailTemplate>
  <Subject>{isDayBefore}</Subject>
  <Body>
    <![CDATA[
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset=""utf-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>UniConnect Event Notification</title>
        <style>
            body {
                font-family: Arial, sans-serif;
                line-height: 1.6;
                color: #333333;
                margin: 0;
                padding: 0;
            }
            .container {
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
            }
            .header {
                background-color: #4b6cb7;
                color: white;
                padding: 20px;
                text-align: center;
                border-radius: 5px 5px 0 0;
            }
            .content {
                background-color: #ffffff;
                padding: 20px;
                border-left: 1px solid #dddddd;
                border-right: 1px solid #dddddd;
            }
            .footer {
                background-color: #f5f5f5;
                padding: 15px;
                text-align: center;
                font-size: 12px;
                color: #666666;
                border-radius: 0 0 5px 5px;
                border: 1px solid #dddddd;
            }
            .button {
                display: inline-block;
                background-color: #4b6cb7;
                color: white;
                padding: 10px 20px;
                text-decoration: none;
                border-radius: 4px;
                margin-top: 15px;
            }
            h1, h2 {
                color: #4b6cb7;
            }
            .highlight {
                font-weight: bold;
                color: #4b6cb7;
            }
        </style>
    </head>
    <body>
        <div class=""container"">
            <div class=""header"">
                <h1>UniConnect</h1>
            </div>
            <div class=""content"">
                <h2>Event Reminder</h2>
                <p>Hi {recipientName},</p>
                <p>{timeMessage}</p>
                <p>Event: <span class=""highlight"">{eventName}</span></p>
                <p>When: <span class=""highlight"">{eventDate}</span></p>
                <p>Description:</p>
                <p>{eventDescription}</p>
                <a href=""https://uniconnect.dev/events"" class=""button"">View Event Details</a>
                <p>We look forward to seeing you there!</p>
                <p>Best regards,<br>The UniConnect Team</p>
            </div>
            <div class=""footer"">
                <p>This email was sent to you because you are a member of a society hosting this event.</p>
                <p>© 2025 UniConnect. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>
    ]]>
  </Body>
</EmailTemplate>";

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(templateXml);

				string subject = isDayBefore ?
					$"Reminder: {eventName} is tomorrow" :
					$"Reminder: {eventName} starts in 1 hour";

				XmlNode? subjectNode = doc.SelectSingleNode("//Subject"); 
                if (subjectNode != null)
				{
					subjectNode.InnerText = subject;
				}

				XmlNode? bodyNode = doc.SelectSingleNode("//Body"); 
                if (bodyNode?.FirstChild is XmlCDataSection cdataSection)
				{
					string emailBody = cdataSection.Data;

					string timeMessage = isDayBefore ?
						$"This is a reminder that <span class=\"highlight\">{eventName}</span> is happening tomorrow." :
						$"<span class=\"highlight\">{eventName}</span> is starting in about an hour.";

					emailBody = emailBody.Replace("{recipientName}", recipientName ?? "Member")
										.Replace("{eventName}", eventName ?? "Event")
										.Replace("{eventDate}", eventDate ?? "")
										.Replace("{eventDescription}", eventDescription ?? "")
										.Replace("{timeMessage}", timeMessage);

					return emailBody;
				}

				return "Error: Could not process email template";
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error processing email template: {ex.Message}");
				return "Error processing email template";
			}
		}
	}
}
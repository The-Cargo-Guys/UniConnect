using System;
using System.Net.Mail;
using UniHack.Data;

namespace UniHackPrototype.Data
{
    public class EmailVerification
    {
        public bool VerifyAndSendEmail(string emailAddress)
        {
            if (IsValidUtsEmail(emailAddress))
            {
                EmailSender.SendEmail(emailAddress, "Your email has been verified successfully.", "Email Verification");
                return true;
            }
            return false;
        }

        private bool IsValidUtsEmail(string email)
        {
            return email.EndsWith("@uts.edu.au", StringComparison.OrdinalIgnoreCase);
        }
    }
}

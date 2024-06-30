using MimeKit;
using Buisness_Logic_Layer.Interfaces;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;



namespace Buisness_Logic_Layer.Services
{
        public class EmailService : IEmailService
        {
            private readonly IConfiguration config;

            public EmailService(IConfiguration config)
            {
                // Initialize the EmailService with IConfiguration dependency
                this.config = config;
            }

            // Method to send an email
            public async Task<string> SendEmail(TextPart text,String To,String Subject)
            {

            var htmlBody = text;
            
            // Create a new MimeMessage for composing the email
            var email = new MimeMessage();

                // Set the sender's email address
                email.From.Add(MailboxAddress.Parse(config.GetSection("EmailUserName").Value));

                // Set the recipient's email address
                email.To.Add(MailboxAddress.Parse(To));

                // Set the email subject
                email.Subject = Subject;

                // Set the email body as HTML text
                email.Body =htmlBody;

                // Create an instance of SmtpClient for sending the email
                using var smtp = new SmtpClient();

                // Connect to the SMTP server with the specified host and port using StartTLS for security
                smtp.Connect(config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);

                // Authenticate with the SMTP server using the provided username and password
                smtp.Authenticate(config.GetSection("EmailUserName").Value, config.GetSection("EmailPassword").Value);

            // Send the composed email
            try
            {
                smtp.Send(email);
            } catch (Exception ex) 
            {
                    // Return an error message if the email could not be sent
                    return ex.Message;
                };

                // Disconnect from the SMTP server after sending the email
                smtp.Disconnect(true);

                // Return a success message
                return "Mail Sent";
           
        }
    }
}


using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MailKit.Net.Smtp;
using MimeKit;

namespace Ainab.Function
{
    public static class send_email
    {
        [FunctionName("send_email")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            EmailData data = JsonConvert.DeserializeObject<EmailData>(requestBody);

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your Name", Environment.GetEnvironmentVariable("SMTP_USERNAME")));
            emailMessage.To.Add(new MailboxAddress("", data.To));
            emailMessage.Subject = data.Subject;
            emailMessage.Body = new TextPart("plain") { Text = data.Body };
            if (data.Important) emailMessage.Priority = MessagePriority.Urgent;

            using var client = new SmtpClient();
            client.Connect(Environment.GetEnvironmentVariable("SMTP_SERVER"), Convert.ToInt32(Environment.GetEnvironmentVariable("SMTP_PORT")), bool.Parse(Environment.GetEnvironmentVariable("SMTP_SSL")));
            client.Authenticate(Environment.GetEnvironmentVariable("SMTP_USERNAME"), Environment.GetEnvironmentVariable("SMTP_PASSWORD"));
            client.Send(emailMessage);
            client.Disconnect(true);

            return new OkObjectResult($"Email sent to {data.To}");
        }
    }

    public class EmailData
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Important { get; set; } = false;
    }
}

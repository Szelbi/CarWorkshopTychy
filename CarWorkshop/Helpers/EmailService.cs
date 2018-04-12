using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CarWorkshop.Database;

namespace CarWorkshop.Helpers
{
    public class EmailService
    {


        public async static Task EmailSendOrder(string to, string subject, Order order, bool isHtmlBody = true)
        {
            var deliverName = Database.TypesRepository.GetDeliverType(order.DeliverTypeId);
            var paymentName = TypesRepository.GetPaymentType(order.PaymentTypeId);

            StringBuilder sb = new StringBuilder();
            sb.Append($"Twoje zamówienie o numerze {order.PaymentIdentifier} zostało utworzone. <br /> <br />");
            sb.Append($"<b>Osoba</b>: {order.Name} <br />");
            sb.Append($"<b>Adres</b>: {order.Address}, {order.PostCode}, {order.City}<br />");
            sb.Append($"<b>Typ dostawy</b>: {deliverName.Name}<br/>");
            sb.Append($"<b>Typ płatności</b>: {paymentName.Name}<br/><br/>");
            sb.Append($"<b>Zamówienie</b>:<br/>");
            foreach (var item in order.OrderProducts)
            {
                sb.Append($"{item.Product.ProductName} - {item.Price} PLN - {item.Count} szt. <br>");
            }

            await SendEmail(to, subject, sb.ToString());
        }

        public async static Task SendEmail(string to, string subject, string body, bool isHtmlBody = true)
        {
            if (!EmailSettings.IsDataValid())
            {
                return;
            }

            using (SmtpClient client = new SmtpClient(EmailSettings.SMTPServer, EmailSettings.Port))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(EmailSettings.Username, EmailSettings.Password);
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(EmailSettings.Email);
                    mailMessage.To.Add(to);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = isHtmlBody;
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.HeadersEncoding = Encoding.UTF8;

                    await client.SendMailAsync(mailMessage);
                }
            }
        }

        private static class EmailSettings
        {
            public static string SMTPServer
            {
                get
                {
                    return ConfigurationManager.AppSettings["SMTPServer"];
                }
            }

            public static int Port
            {
                get
                {
                    int value = -1;
                    return int.TryParse(ConfigurationManager.AppSettings["Port"], out value) ? value : -1;
                }
            }

            public static string Username
            {
                get
                {
                    return ConfigurationManager.AppSettings["Username"];
                }
            }

            public static string Password
            {
                get
                {
                    return ConfigurationManager.AppSettings["Password"];
                }
            }

            public static string Email
            {
                get
                {
                    return ConfigurationManager.AppSettings["Email"];
                }
            }


            public static bool IsDataValid()
            {
                if (!string.IsNullOrWhiteSpace(EmailSettings.SMTPServer) && !string.IsNullOrWhiteSpace(EmailSettings.Username) && !string.IsNullOrWhiteSpace(EmailSettings.Password) && EmailSettings.Port > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
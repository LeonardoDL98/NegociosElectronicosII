using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace NegociosElectronicosII.Email
{
    public class Mail
    {
        public String From { get; set; }
        public List<String> To { get; set; }
        public String Subject { get; set; }
        public string Body { get; set; }
        public string BBC { get; set; }
        public string Host { get; set; }
        public string AccountServer { get; set; }
        public string PasswordServer { get; set; }
        public int? Port { get; set; }
        public String EmailAdmin { get; set; }
        #region Errors

        private static readonly string MailErrorSubject = "Error en la aplicacion";
        private const string MailErrorTemplate = @"<h1> {0} </h1>
	                                            URL:          <b>{1}</b>  <hr />
	                                            Params:       <b>{2}</b>  <hr />
	                                            TargetSite:   <b>{3}</b>  <hr />
	                                            Source:       <b>{4}</b>  <hr />
                                                StackTrace:   <b>{5}</b>  <hr />
	                                            DateTime:     <b>{6}</b>  <hr />
	                                            User:         <b>{7}</b>";

        public void SendErrorMail(Exception ex, string URL, List<string> UrlParams, String User)
        {
            var message = "|=>" + ex.Message;
            try
            {
                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    message += "<br>|=>" + innerEx.Message;
                    innerEx = innerEx.InnerException;
                }
            }
            catch { }

            SmtpClient smtp = new SmtpClient();
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(this.From);

            foreach (var item in this.To)
                mailMessage.To.Add(item);

            if (this.Port != null)
                smtp.Port = this.Port.Value;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // [2] Added this
            smtp.UseDefaultCredentials = false; // [3] Changed this
            smtp.Credentials = new System.Net.NetworkCredential(this.AccountServer, this.PasswordServer);  // [4] Added this. Note, first parameter is NOT string.
            smtp.Host = this.Host;

            if (!string.IsNullOrEmpty(this.BBC))
                mailMessage.Bcc.Add(this.BBC);

            //Formatted mail body
            mailMessage.IsBodyHtml = true;

            mailMessage.Subject = MailErrorSubject;
            mailMessage.Body = string.Format(MailErrorTemplate, message, URL, string.Join("<br>", UrlParams), ex.TargetSite, ex.Source, ex.StackTrace, DateTime.Now.ToString("MMMM dd, yyyy HH:mm tt"), User);
            smtp.Send(mailMessage);
        }

        #endregion

        public bool Send()
        {
            try
            {
                if (this.To == null || this.To.Count == 0 || string.IsNullOrWhiteSpace(this.Body) || string.IsNullOrWhiteSpace(this.Subject))
                    return false;

                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress(this.From);

                // The important part -- configuring the SMTP client
                SmtpClient smtp = new SmtpClient();
                //smtp.Port = 587;   // [1] You can try with 465 also, I always used 587 and got success
                if (this.Port != null)
                    smtp.Port = this.Port.Value;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // [2] Added this
                smtp.UseDefaultCredentials = false; // [3] Changed this
                smtp.Credentials = new System.Net.NetworkCredential(this.AccountServer, this.PasswordServer);  // [4] Added this. Note, first parameter is NOT string.
                smtp.Host = this.Host;

                if (!string.IsNullOrEmpty(this.BBC))
                    mail.Bcc.Add(this.BBC);

                //recipient address
                foreach (var item in this.To)
                    mail.To.Add(new MailAddress(item));

                //Formatted mail body
                mail.IsBodyHtml = true;

                mail.Subject = this.Subject;
                mail.Body = this.Body;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                return false;
            }


            return true;
        }

    }
}
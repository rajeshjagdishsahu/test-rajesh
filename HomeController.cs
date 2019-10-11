using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HealthCare.Models;

namespace HealthCare.Controllers
{
    public class HomeController : Controller
    {
        public const string COMPANY_EMAIL = "info@healthcareaccessltd.co.uk";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(FeedbackForm feedbackForm)
        {
            StringBuilder sbEmailBody = new StringBuilder();
            string strSMTPHost = string.Empty;
            int iSMTPPort = 0;
            string strUserName = string.Empty;
            string strUsePwd = string.Empty;
            string strRecipientEmail = string.Empty;
            string strEmailSubject = string.Empty;
            try
            {
                string IsEmailEnabled = ConfigurationManager.AppSettings["IsEmailEnabled"].ToString();
                if (IsEmailEnabled == "YES")
                {
                    strSMTPHost = ConfigurationManager.AppSettings["SmtpHost"].ToString();
                    iSMTPPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                    strUserName = ConfigurationManager.AppSettings["NetworkCredentialUserName"].ToString();
                    strUsePwd = ConfigurationManager.AppSettings["NetworkCredentialUserPassword"].ToString();
                    strRecipientEmail = feedbackForm.UserEmailAddress.ToString();
                    strEmailSubject = "Feedback from Healthcare Access Ltd";

                    //To revisit this as this a feedback from website and not 
                    //Append the collections from the form
                    sbEmailBody.Append("Dear ");
                    sbEmailBody.Append(feedbackForm.UserName + "\n\n");                    
                    sbEmailBody.Append("Below is your details: \n");
                    sbEmailBody.Append("User Postcode: " + feedbackForm.UserPostcode + "\n");
                    sbEmailBody.Append("User Email " + feedbackForm.UserEmailAddress + "\n");
                    sbEmailBody.Append("User Phone Number: " + feedbackForm.UserPhoneNumber + "\n\n");
                    sbEmailBody.Append("User Message: " + feedbackForm.UserMessage + "\n");
                    sbEmailBody.Append("Thanks for the feedback...\n");
                    sbEmailBody.Append("\n\nKind Regards,");
                    sbEmailBody.Append("\n Healthcare Access Ltd");

                    SmtpClient client = new SmtpClient();
                    client.Host = strSMTPHost;
                    client.Port = iSMTPPort;
                    client.UseDefaultCredentials = true;
                    client.EnableSsl = true;

                    client.Credentials = new NetworkCredential(strUserName, strUsePwd);
                    client.Send(strUserName, strRecipientEmail, strEmailSubject, sbEmailBody.ToString());
                    // If some error related to security
                    //The SMTP server requires a secure connection or the client was not authenticated. The server response was: 5.5.1 Authentication Required.

                    /*
                    // Resolution: https://stackoverflow.com/questions/20906077/gmail-error-the-smtp-server-requires-a-secure-connection-or-the-client-was-not
                    Its a security issue, Gmail by default prevents access for your e-mail account from custom applications.You can set it up to accept the login from your application.
                    After Logging in to your e - mail, CLICK HERE -> https://myaccount.google.com/lesssecureapps

                    This will take you to the following page
                    Make that "ON" for less secure
                    */
                }
                else
                {
                    // Any message wanted to show or not
                }
            }
            catch (Exception ex)
            {

            }

            return View();

        }
/*        [HttpGet]
        public ActionResult Contacts()
        {
            FeedbackForm temp = new FeedbackForm();
            temp.UserMessage = @Resources.Global.Contacts_Form_Message_Field;
            return View(temp);
        }


        [HttpPost]
        public ActionResult Contacts(FeedbackForm Model)
        {
            string Text = "<html> <head> </head>" +
                          " <body style= \" font-size:12px; font-family: Arial\">" +
                          Model.UserMessage +
                          "</body></html>";

            SendEmail("tayna-anita@mail.ru", Text);
            FeedbackForm tempForm = new FeedbackForm();
            return View(tempForm);
        }


        public static bool SendEmail(string SentTo, string Text)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("Test@mail.ru");
            msg.To.Add(SentTo);
            msg.Subject = "Password";
            msg.Body = Text;
            msg.Priority = MailPriority.High;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.mail.ru", 25);



            client.UseDefaultCredentials = false;
            client.EnableSsl = false;
            client.Credentials = new NetworkCredential("TestLogin", "TestPassword");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.EnableSsl = true;

            try
            {
                client.Send(msg);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }*/


    }
}
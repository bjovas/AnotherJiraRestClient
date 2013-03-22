using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using AnotherJiraRestClient;
using AnotherJiraRestClient.JiraModel;

namespace AspnetSampleApp
{
    public class Global : System.Web.HttpApplication
    {     
        protected void Application_Error(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var exception = context.Server.GetLastError();
            string subject = "";
            StringBuilder sb = new StringBuilder();

            if (exception.InnerException != null)
            {
                subject = String.Format("{0}: {1}", exception.InnerException.GetType(), exception.InnerException.Message);

                sb.Append(String.Format("Inner Exception:{1}{2}", Environment.NewLine, context.Request.Url, Environment.NewLine));
                sb.Append(String.Format("Type:{0}{1}", exception.InnerException.GetType(), Environment.NewLine));
                sb.Append(String.Format("Message:{0}{1}", exception.InnerException.Message, Environment.NewLine));
                sb.Append(String.Format("Stack Trace:{0}{1}", exception.InnerException.StackTrace, Environment.NewLine));
                sb.Append(String.Format("{0}{1}", Environment.NewLine, Environment.NewLine));
            }

            sb.Append(String.Format("Url:{0}{1}", context.Request.Url, Environment.NewLine));
            sb.Append(String.Format("Source:{0}{1}", exception.Source, Environment.NewLine));
            sb.Append(String.Format("Message:{0}{1}", exception.Message, Environment.NewLine));
            sb.Append(String.Format("Stack Trace:{0}{1}", exception.StackTrace, Environment.NewLine));
            if (subject == "") subject = "Unhandled exception logged in global.asax";
             
            var account = new JiraAccount()
            {
                ServerUrl = "URL!!!",
                User = "USERNAME!!!",
                Password = "PASSWORD!!!"
            };
            var client = new JiraClient(account);

            var newIssue = new CreateIssue("TEST", subject, sb.ToString(), "1", "1", new string[] { "label1", "label2" });
            var createdIssue = client.CreateIssue(newIssue);

            // Return error page
            HttpContext.Current.ClearError();
            Response.StatusCode = 500;
            Response.Write("OH NO! An unhandled error occured, but luckily it was save in JIRA with ID: " + createdIssue.key);
        }     
    }
}
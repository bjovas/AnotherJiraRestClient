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
                subject = String.Format("{0}: {1}", exception.InnerException.GetType(), exception.InnerException.Message).Substring(0, 120);

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

            var account = new JiraAccount("", "", "");

            var client = new JiraClient(account);
            


           
            // Must find algorithm to extract relevant stuff from stacktrace
            var issues = client.GetIssuesByJql(
                "text ~ \"DataBaseStuff.cs:line 22\" AND project = Intouch",
                0,
                25,
                new string[] { AnotherJiraRestClient.Issue.FIELD_SUMMARY, AnotherJiraRestClient.Issue.FIELD_STATUS, AnotherJiraRestClient.Issue.FIELD_PRIORITY });



            if (issues.total == 0)
            {
                // We cant find an issue like this before in Jira
                // What we will do is to create it here
                var newIssue = new CreateIssue("IN", subject, sb.ToString(), "1", "1", new string[] { "label1", "label2" });
                var createdIssue = client.CreateIssue(newIssue);

                // And here we would show a screen where the user could input some valuable information


                // Return error page
                HttpContext.Current.ClearError();
                Response.StatusCode = 500;
                Response.Write("OH NO! An unhandled error occured, but luckily it was save in JIRA with ID: " + createdIssue.key);

            }
            else if (issues.total == 1)
            {
                // We like this one, we have managed to avoid posting a duplicated bug.
                // Normally we would give the customer a chance to add his input here as well.
                client.AddComment(issues.issues[0].key, "This exact issue happened again!!!");


                // It may well be that i should use the LINK AS DUPLICATE feature here.


                // Return error page
                HttpContext.Current.ClearError();
                Response.StatusCode = 500;
                Response.Write("OH NO! An unhandled error occured AGAIN!!!, so we have updated it. Sorry :(");

            }
            else
            {
                client.AddComment(issues.issues[0].key, "An issue like this has happened before!!!");

                // Return error page
                HttpContext.Current.ClearError();
                Response.StatusCode = 500;
                Response.Write("OH NO! An unhandled error occured AGAIN!!!, and we didnt know what to do.(");

            }
        }     
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using EasyHttp;
using System.Configuration;
using AnotherJiraRestClient;
using AnotherJiraRestClient.JiraModel;
using System.Diagnostics;
using EasyHttp.Http;

namespace AspnetSampleApp
{
    public class Global : System.Web.HttpApplication
    {     
        protected void Application_Error(object sender, EventArgs e)
        {
            var url = ConfigurationManager.AppSettings["url"];
            var username = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];
            var timeout = ConfigurationManager.AppSettings["timeout"];
            var meta = ConfigurationManager.AppSettings["meta"];
            var jiraProject = ConfigurationManager.AppSettings["jiraProject"];
            var customfield_message = ConfigurationManager.AppSettings["cf_exceptionmessage"];
            var customfield_stacktrace = ConfigurationManager.AppSettings["cf_exceptionstacktrace"];
            var emulatedResponseFromCustomer = EmulateInputFromCustomer();

            var context = HttpContext.Current;
            var exception = context.Server.GetLastError();

            string methodName = "";
            string linenumber = "";
            string subject = "Unhandled exception logged in global.asax";

            StringBuilder sb = new StringBuilder();           
            sb.Append(String.Format("Url:{0}{1}", context.Request.Url, Environment.NewLine));
            
            
            if (exception.InnerException != null)
            {
                var stackTrace = new StackTrace(exception.InnerException, true);

                var methodInfo = GetMethodNameAndLineNumberFromFrames(meta, stackTrace.GetFrames());
                methodName = methodInfo.Item1;
                linenumber = methodInfo.Item2.ToString();
                var path = GetRelevantPathFromFrames(meta, stackTrace.GetFrames());

                subject = string.Format("Exception in {0}, Method: {1}, Line: {2}", path, methodInfo.Item1, methodInfo.Item2);
             
                sb.Append(subject + "   ---   ");
                sb.Append(String.Format("Inner Exception:{0}{1}   ---   ", Environment.NewLine, Environment.NewLine));
                sb.Append(String.Format("Type:{0}{1}   ---   ", exception.InnerException.GetType(), Environment.NewLine));
                sb.Append(String.Format("Message:{0}{1}   ---   ", exception.InnerException.Message, Environment.NewLine));
                sb.Append(String.Format("Stack Trace:{0}{1}   ---   ", exception.InnerException.StackTrace, Environment.NewLine));
                sb.Append(String.Format("{0}{1}   ---   ", Environment.NewLine, Environment.NewLine));
            }
       
            sb.Append(String.Format("Source:{0}{1}", exception.Source, Environment.NewLine));
            sb.Append(String.Format("Message:{0}{1}", exception.Message, Environment.NewLine));
            sb.Append(String.Format("Stack Trace:{0}{1}", exception.StackTrace, Environment.NewLine));

            var account = new JiraAccount(url, username, password);
            var client = new JiraClient(account);
            client.Timeout = int.Parse(timeout);
                
            var jqlQuery = String.Format("project = \"{0}\" AND text ~ \"{1}\" ORDER BY key ASC", jiraProject, subject).Replace("\\", "\\\\");
            var issues = client.GetIssuesByJql(jqlQuery, 0, 3,
                new string[] { 
                    Issue.FIELD_SUMMARY, Issue.FIELD_STATUS, Issue.FIELD_PRIORITY });

            if (issues.total == 0)
            {
                var newIssue = new CreateIssue(jiraProject, subject, sb.ToString(), "1", "1", new string[] { "label1", "label2" });
                newIssue.AddField(customfield_message, exception.Message);
                newIssue.AddField(customfield_stacktrace, exception.StackTrace);
                var createdIssue = client.CreateIssue(newIssue);

                // And here we would show a screen where the user could input some valuable information
                // and afterwards we would have updated the issue with the new info.

                // Return error page
                HttpContext.Current.ClearError();
                Response.StatusCode = 500;
                Response.Write("OH NO! An unhandled error occured, but luckily it was save in JIRA with ID: " + createdIssue.key);

            }
            else if (issues.total == 1)
            {          
                client.AddComment(issues.issues[0].key, "This is the first time this exact issue has happened again!");

                var newIssue = new CreateIssue("IN", subject, sb.ToString(), "1", "1", new string[] { "Autocreated"});
                newIssue.AddField(customfield_message, exception.Message);
                newIssue.AddField(customfield_stacktrace, exception.StackTrace);
                var createdIssue = client.CreateIssue(newIssue);
                client.AddIssueLink(issues.issues[0].key, createdIssue.key, "Duplicate");

                if (emulatedResponseFromCustomer.Item1)
                {
                    Issue issueToUpdate = client.GetIssue(createdIssue.key, new string[] { "description", "labels"});



                    UpdateIssue updateIssue = new UpdateIssue();
                    
                    updateIssue.fields.description = emulatedResponseFromCustomer.Item2;
                    //updateIssue.fields.labels.Add("Customer feedback");

                    client.UpdateIssue(updateIssue, issueToUpdate.key);

                }

                // Return error page
                HttpContext.Current.ClearError();
                Response.StatusCode = 500;
                Response.Write("OH NO! This is not the first time this exception has reoccured. Customer feedback? " + emulatedResponseFromCustomer.Item1);

            }
            else
            {
                client.AddComment(issues.issues[0].key, "This is not the first time this exception has reoccured");

                var newIssue = new CreateIssue("IN", subject, sb.ToString(), "1", "4", new string[] { "Autocreated"});
                newIssue.AddField(customfield_message, exception.Message);
                newIssue.AddField(customfield_stacktrace, exception.StackTrace);

                var createdIssue = client.CreateIssue(newIssue);

                client.AddIssueLink(issues.issues[0].key, createdIssue.key, "Duplicate");

                // And here we would show a screen where the user could input some valuable information
                // and afterwards we would have updated the issue with the new info.

                if (emulatedResponseFromCustomer.Item1)
                {
                    var issueToUpdate = client.GetIssue(createdIssue.key, new string[] { "description", "labels" });


                    UpdateIssue updateIssue = new UpdateIssue();
                   

                    updateIssue.fields = new UpdateIssue.Fields();
                    updateIssue.fields.description = emulatedResponseFromCustomer.Item2;
                    //updateIssue.fields.labels.Add("Customer feedback");
                    //issueToUpdate.fields.description = emulatedResponseFromCustomer.Item2;
                    //issueToUpdate.fields.labels.Add("Customer feedback");

                    client.UpdateIssue(updateIssue, issueToUpdate.key);

                }

                // Return error page
                HttpContext.Current.ClearError();
                Response.StatusCode = 500;
                Response.Write("OH NO! This is not the first time this exception has reoccured. Customer feedback? " + emulatedResponseFromCustomer.Item1);
            }           
        }

        public Tuple<bool, string> EmulateInputFromCustomer()
        {

            var http = new HttpClient();
            var response = http.Get("http://www.iheartquotes.com/api/v1/random");
            

         



            if (response.RawText.Length > 50)          
                return new Tuple<bool, string>(true, response.RawText );
            
            else
                return new Tuple<bool, string>(false, "");
        }

        public Tuple<string, int> GetMethodNameAndLineNumberFromFrames(string meta, StackFrame[] frames)
        {
            foreach (var frame in frames)
                if (!String.IsNullOrEmpty(frame.GetFileName()) && frame.GetFileName().Contains(meta))
                    return new Tuple<string, int>(frame.GetMethod().Name, frame.GetFileLineNumber());

            return new Tuple<string, int>("Unknown method name", 0);
        }

        public string GetRelevantPathFromFrames(string meta, StackFrame[] frames)
        {
            foreach (var frame in frames)
            {
                var framePath = frame.GetFileName();

                if (!String.IsNullOrEmpty(framePath) && framePath.Contains(meta))
                {                 
                    var filenamesplit = framePath.Split(new string[] { "\\" }, StringSplitOptions.None);

                    return String.Format("{0}\\{1}", 
                        filenamesplit[filenamesplit.Length - 2], 
                        filenamesplit[filenamesplit.Length - 1]);
                }
            }

            return "Unknown path";
        }
    }
}
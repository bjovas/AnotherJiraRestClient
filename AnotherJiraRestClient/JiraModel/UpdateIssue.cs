using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnotherJiraRestClient.JiraModel;

namespace AnotherJiraRestClient
{
    public class UpdateIssue
    {
        // Field names

        public const string FIELD_DESCRIPTION = "description";
        public const string FIELD_LABELS = "labels";

        public Fields fields { get; set; }

        public class Fields
        {
            public Priority priority { get; set; }
            public string description { get; set; }
            public List<string> labels { get; set; }
        }
    }
}

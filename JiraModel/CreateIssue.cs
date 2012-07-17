﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnotherJiraRestClient.JiraModel
{
    /// <summary>
    /// Class used to create a new issue in the Jira API.
    /// </summary>
    public class CreateIssue
    {
        public readonly Dictionary<string, object> fields;

        public CreateIssue(
            string projectKey,
            string summary,
            string description,
            string issueTypeId,
            string priorityId,
            IEnumerable<string> labels)
        {
            fields = new Dictionary<string, object>
            {
                { "project", new { key = projectKey } },
                { "summary", summary },
                { "description", description },
                { "issuetype", new { id = issueTypeId } },
                { "priority", new { id = priorityId } },
                { "labels", labels }
            };
        }

        public void AddField(string fieldName, object value)
        {
            fields.Add(fieldName, value);
        }
    }
}

namespace Skypebot.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    using Newtonsoft.Json.Linq;

    using Skypebot.Core.Commands.Abstract;
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// The jira command.
    /// </summary>
    public class JiraCommand : AbstractReplaceCommand
    {
        #region Constants and Fields

        /// <summary>
        /// The login.
        /// </summary>
        private readonly string login;

        /// <summary>
        /// The password.
        /// </summary>
        private readonly string password;

        /// <summary>
        /// The regex.
        /// </summary>
        private readonly Regex regex;

        /// <summary>
        /// The url.
        /// </summary>
        private readonly string url;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JiraCommand"/> class.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="projectNames">
        /// The project names.
        /// </param>
        public JiraCommand(string url, string login, string password, List<string> projectNames)
        {
            if (!url.EndsWith("/"))
            {
                url += "/";
            }

            this.url = url;
            this.login = login;
            this.password = password;

            string regexstring = string.Empty;

            if (projectNames.Count > 0)
            {
                regexstring = string.Format(@"\b({0}", projectNames[0]);
                for (int i = 1; i < projectNames.Count; i++)
                {
                    regexstring += string.Format("|{0}", projectNames[i]);
                }

                regexstring += @")\-\d+";
            }

            this.regex = new Regex(regexstring, RegexOptions.IgnoreCase);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets pattern for searching jira issues thru the text.
        /// </summary>
        private Regex Pattern
        {
            get
            {
                return this.regex;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Executes the command on answer to the specified chat name.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <param name="chat">
        /// The chat.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The result string to send back as the answer.
        /// </returns>
        public override string Execute(SkypeContact contact, SkypeChat chat, string message)
        {
            string json = string.Empty;

            string argument = this.regex.Match(message).Value;

            try
            {
                var request = WebRequest.Create(this.url + "rest/api/latest/issue/" + argument) as HttpWebRequest;
                request.ContentType = "application/json";
                request.Method = "GET";

                string base64Credentials = this.GetEncodedCredentials();
                request.Headers.Add("Authorization", "Basic " + base64Credentials);

                var response = request.GetResponse() as HttpWebResponse;

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    json = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(json))
            {
                JObject jsonObj = JObject.Parse(json);
                string key = jsonObj["key"].ToString().ToUpper();
                string summary = jsonObj["fields"]["summary"].ToString();
                string status;
                string assignee;

                if (summary[0] == '{')
                {
                    summary = jsonObj["fields"]["summary"]["value"].ToString();
                    status = jsonObj["fields"]["status"]["value"]["name"].ToString();
                    assignee = jsonObj["fields"]["assignee"]["value"]["displayName"].ToString();
                }
                else
                {
                    status = jsonObj["fields"]["status"]["name"].ToString();
                    assignee = jsonObj["fields"]["assignee"].HasValues
                                   ? jsonObj["fields"]["assignee"]["displayName"].ToString()
                                   : "Unassigned";
                }

                var sb = new StringBuilder();

                if (!message.StartsWith("http"))
                {
                    sb.AppendLine(this.url + "browse/" + key);
                }

                sb.AppendLine('"' + summary + '"');
                sb.AppendLine(string.Format("Status: {0}", status));
                sb.AppendLine(string.Format("Assignee: {0}", assignee));
                return sb.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// The is applicable.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool IsApplicable(string message)
        {
            Match match = this.Pattern.Match(message);
            return match.Success;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns credentials pair in base64 format.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetEncodedCredentials()
        {
            string mergedCredentials = string.Format("{0}:{1}", this.login, this.password);
            byte[] byteCredentials = Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }

        #endregion
    }
}
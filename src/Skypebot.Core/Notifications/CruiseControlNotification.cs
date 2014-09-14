namespace Skypebot.Core.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Skypebot.Core.Notifications.Abstract;

    using ThoughtWorks.CruiseControl.Remote;

    /// <summary>
    /// The cruise control notification.
    /// </summary>
    public class CruiseControlNotification : AbstractNotification
    {
        #region Constants and Fields

        /// <summary>
        /// The list of build names to be notified of.
        /// </summary>
        private readonly List<string> buildNames;

        /// <summary>
        /// The client.
        /// </summary>
        private readonly CruiseServerClientBase client;

        /// <summary>
        /// The current statuses.
        /// </summary>
        private ProjectStatus[] currentStatuses;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CruiseControlNotification"/> class.
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="targetServer">
        /// The target server.
        /// </param>
        /// <param name="buildNames">
        /// The list of build names to be notified of.
        /// </param>
        public CruiseControlNotification(string address, string targetServer, List<string> buildNames)
        {
            this.buildNames = buildNames;
            this.client =
                new CruiseServerClientFactory().GenerateHttpClient(address, targetServer) as CruiseServerClient;
        }

        #endregion

        #region Public Methods and Operators


        /// <summary>
        /// Returns whether status of resource/thing has changed from the previous call.
        /// </summary>
        /// <param name="message">The output message describing changes.</param>
        /// <returns>
        ///   <c>true</c>, if there are any changes; otherwise, <c>false</c>.
        /// </returns>
        public override bool HasChanged(out string message)
        {
            message = string.Empty;
            bool hasChanged = false;

            ProjectStatus[] statuses;
            try
            {
                statuses = this.client.GetProjectStatus();
            }
            catch (Exception)
            {
                return false;
            }

            if (this.currentStatuses != null && this.currentStatuses.Length > 0)
            {
                for (int i = 0; i < statuses.Length; i++)
                {
                    if (this.buildNames.Contains(statuses[i].Name)
                        && this.currentStatuses[i].BuildStatus != statuses[i].BuildStatus)
                    {
                        string statusChanges = this.GetStatusChanges(statuses[i]);

                        if (!string.IsNullOrWhiteSpace(statusChanges))
                        {
                            message = message + Environment.NewLine + statusChanges;
                        }

                        hasChanged = true;
                    }
                }
            }

            this.currentStatuses = statuses;
            return hasChanged;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the status changes string.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetStatusChanges(ProjectStatus status)
        {
            // Show first breaking task and the breaking users.
            // Search from the end, to get the most recent messages of the specified kind.
            bool breakerFound = false;
            bool breakingTaskFound = false;
            bool aborterFound = false;
            bool fixerFound = false;
            var message = new StringBuilder();

            for (int i = status.Messages.Length - 1; i >= 0; i--)
            {
                if (status.Messages[i].Kind == Message.MessageKind.Fixer)
                {
                    if (!fixerFound)
                    {
                        if (status.CurrentMessage != status.Messages[i].Text)
                        {
                            message.AppendFormat("Recent checkins have fixed the {0} build", status.Name);
                            message.Append(status.Messages[i].Text);
                        }

                        fixerFound = true;
                    }
                }

                if (status.Messages[i].Kind == Message.MessageKind.Breakers)
                {
                    if (!breakerFound)
                    {
                        if (status.CurrentMessage != status.Messages[i].Text)
                        {
                            message.AppendFormat("Recent checkins have broken the {0} build. Breaker(s): ", status.Name);
                            message.Append(status.Messages[i].Text);
                        }

                        breakerFound = true;
                    }
                }

                if (status.Messages[i].Kind == Message.MessageKind.FailingTasks)
                {
                    if (!breakingTaskFound)
                    {
                        if (status.CurrentMessage != status.Messages[i].Text)
                        {
                            message.Append(status.Messages[i].Text);
                        }

                        breakingTaskFound = true;
                    }
                }

                if (status.Messages[i].Kind == Message.MessageKind.BuildAbortedBy)
                {
                    if (!aborterFound)
                    {
                        if (status.CurrentMessage != status.Messages[i].Text)
                        {
                            message.Append("The build is aborted by: ");
                            message.Append(status.Messages[i].Text);
                        }

                        aborterFound = true;
                    }
                }
            }

            // TODO: For some reason, there may be no any messages when 
            // a build is fixed, so add the message explicitly.
            if (status.BuildStatus == IntegrationStatus.Success && !fixerFound)
            {
                message.AppendFormat("Recent checkins have fixed the {0} build", status.Name);
            }

            if (!string.IsNullOrEmpty(status.CurrentMessage))
            {
                message.Append(" - " + status.CurrentMessage);
            }

            if (message.Length > 0)
            {
                message.AppendLine();
                message.Append(status.WebURL);
            }

            return message.ToString();
        }

        #endregion
    }
}
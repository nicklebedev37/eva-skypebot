namespace Skypebot.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using global::Skypebot.Core.Commands.Abstract;

    using global::Skypebot.Core.ObjectModel;

    using ThoughtWorks.CruiseControl.Remote;

    /// <summary>
    /// The cruise control command.
    /// </summary>
    public class CruiseControlCommand : AbstractDirectCommand
    {
        #region Constants and Fields

        /// <summary>
        /// The build names.
        /// </summary>
        private readonly List<string> buildNames;

        /// <summary>
        /// The client.
        /// </summary>
        private readonly CruiseServerClientBase client;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CruiseControlCommand"/> class.
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="targetServer">
        /// The target server.
        /// </param>
        /// <param name="buildNames">
        /// The build names.
        /// </param>
        public CruiseControlCommand(string address, string targetServer, List<string> buildNames)
        {
            this.buildNames = buildNames;
            this.client =
                new CruiseServerClientFactory().GenerateHttpClient(address, targetServer) as CruiseServerClient;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the help (in short).
        /// </summary>
        /// <value>
        /// The help.
        /// </value>
        public override string Help
        {
            get
            {
                return "provides info about build statuses on Cruise Control server";
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "cc";
            }
        }

        /// <summary>
        /// Gets the usage.
        /// </summary>
        /// <value>
        /// The usage.
        /// </value>
        public override string Usage
        {
            get
            {
                return "Usage:\r\n"
                       + "cc - lists build names and their current statuses of projects the current chat is subscribed to\r\n"
                       + "cc 'build name' - shows status of a certain build\r\n" + "Examples:\r\n" + "cc\r\n"
                       + "cc TB5.7";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The dispose.
        /// </summary>
        public override void Dispose()
        {
            this.client.Dispose();
        }

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
            IEnumerable<ProjectStatus> statuses = this.client.GetProjectStatus();

            string args = this.ExtractCommandArgument(message, chat.Contacts.Count == 2);
            if (!string.IsNullOrEmpty(args))
            {
                statuses = statuses.Where(s => string.Equals(s.Name, args, StringComparison.CurrentCultureIgnoreCase));
            }

            var sb = new StringBuilder();
            foreach (ProjectStatus status in statuses)
            {
                if (this.buildNames.Contains(status.Name))
                {
                    sb.AppendLine(string.Format("{0}: {1}", status.Name, status.BuildStatus));
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
namespace Skypebot.Core.Commands
{
    using System;

    using Skypebot.Core.Commands.Abstract;
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// The sub command.
    /// </summary>
    public class SubCommand : AbstractDirectCommand
    {
        #region Constants and Fields

        /// <summary>
        /// The data manager.
        /// </summary>
        private readonly DataManager dataManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubCommand"/> class.
        /// </summary>
        /// <param name="dataManager">
        /// The data manager.
        /// </param>
        public SubCommand(DataManager dataManager)
        {
            this.dataManager = dataManager;
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
                return "subscribes the current chat to a project";
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "sub";
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
                       + "sub 'project name' - subscribes the current chat to notifications from Cruise Control and showing info about a bugtracker ticket for the selected project\r\n"
                       + "Examples:\r\n" + "sub CLASSROOM\r\n";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="chat">
        /// The chat.
        /// </param>
        /// <param name="message">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string Execute(SkypeContact sender, SkypeChat chat, string message)
        {
            string projectName = this.ExtractCommandArgument(message, chat.Contacts.Count == 2);
            int projectId = this.dataManager.GetProjectId(projectName);

            if (projectId == 0)
            {
                return "No project with such name was found";
            }

            try
            {
                this.dataManager.SubscribeChat(chat.Id, projectId);
            }
            catch (Exception)
            {
                return "Oops, something went wrong with subscribing";
            }

            return string.Format("You've successfully been subscribed to project {0}", projectName);
        }

        #endregion
    }
}
namespace Skypebot.Core.Commands
{
    using System;

    using Skypebot.Core.Commands.Abstract;
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// The unsub command.
    /// </summary>
    public class UnsubCommand : AbstractDirectCommand
    {
        #region Constants and Fields

        /// <summary>
        /// The data manager.
        /// </summary>
        private readonly DataManager dataManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsubCommand"/> class.
        /// </summary>
        /// <param name="dataManager">
        /// The data manager.
        /// </param>
        public UnsubCommand(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the help.
        /// </summary>
        public override string Help
        {
            get
            {
                return "unsubscribes current chat from projects";
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "unsub";
            }
        }

        /// <summary>
        /// Gets the usage.
        /// </summary>
        public override string Usage
        {
            get
            {
                return "Just type 'eva unsub' and you'll get unsubscribed";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The execute.
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
        /// The <see cref="string"/>.
        /// </returns>
        public override string Execute(SkypeContact contact, SkypeChat chat, string message)
        {
            try
            {
                this.dataManager.UnsubscribeChat(chat.Id);
            }
            catch (Exception)
            {
                return "Oops, something went wrong with unsubscribing";
            }

            return string.Format("You've successfully been unsubscribed from any project");
        }

        #endregion
    }
}
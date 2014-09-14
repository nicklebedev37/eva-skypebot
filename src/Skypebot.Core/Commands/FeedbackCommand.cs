namespace Skypebot.Core.Commands
{
    using System;

    using Skypebot.Core.Commands.Abstract;
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// The feedback command.
    /// </summary>
    public class FeedbackCommand : AbstractDirectCommand
    {
        #region Constants and Fields

        /// <summary>
        /// The maximum length of the feedback message.
        /// </summary>
        private const int MaxFeedbackLength = 500;

        /// <summary>
        /// The data manager.
        /// </summary>
        private readonly DataManager dataManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackCommand"/> class.
        /// </summary>
        /// <param name="dataManager">
        /// The data manager.
        /// </param>
        public FeedbackCommand(DataManager dataManager)
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
                return "Sends a message to eva developers";
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "feedback";
            }
        }

        /// <summary>
        /// Gets the usage.
        /// </summary>
        public override string Usage
        {
            get
            {
                return "Just type 'eva feedback some_message' to drop somemessage to eva developers";
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
            string argument = this.ExtractCommandArgument(message, chat.Contacts.Count == 2);

            try
            {
                if (!string.IsNullOrEmpty(argument))
                {
                    if (argument.Length > MaxFeedbackLength)
                    {
                        return
                            string.Format(
                                "The feedback message is too long. The maximum allowed length is 500. Your's is {0}", 
                                argument.Length);
                    }

                    this.dataManager.AddFeedback(contact.Id, argument);
                }
                else
                {
                    return "Please use feedback command with argument";
                }
            }
            catch (Exception)
            {
                return "Sorry, couldn't process your feedback";
            }

            return "Got your message. We will try to take a look at it asap. Thanks!";
        }

        #endregion
    }
}
namespace Skypebot.Core.Commands
{
    using Skypebot.Core.Commands.Abstract;
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// Executes the specified JavaScript code.
    /// </summary>
    public class OnCommand : AbstractDirectCommand
    {
        #region Constants and Fields

        /// <summary>
        /// The already listening message.
        /// </summary>
        private const string AlreadyListeningMessage = "I'm already listening to you :)";

        /// <summary>
        /// The hello message.
        /// </summary>
        private const string HelloMessage = "Hello!";

        /// <summary>
        /// The state manager.
        /// </summary>
        private readonly DataManager dataManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OnCommand"/> class.
        /// </summary>
        /// <param name="dataManager">
        /// The state manager.
        /// </param>
        public OnCommand(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public override CommandType CommandType
        {
            get
            {
                return CommandType.System;
            }
        }

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
                return "turns on eva for current chat.";
            }
        }

        /// <summary>
        /// Gets the name the command is invoked by.
        /// </summary>
        /// <value>
        /// The name the command is invoked by.
        /// </value>
        public override string Name
        {
            get
            {
                return "on";
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
                return "Usage:\r\n" + "just type 'eva on' in the chat which you would like eva work for.\r\n";
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
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The result string to send back as the answer.
        /// </returns>
        public override string Execute(SkypeContact contact, SkypeChat chat, string args)
        {
            if (this.dataManager.IsChatEnabled(chat.Id))
            {
                return AlreadyListeningMessage;
            }

            this.dataManager.EnabledChat(chat.Id);
            return HelloMessage;
        }

        #endregion
    }
}
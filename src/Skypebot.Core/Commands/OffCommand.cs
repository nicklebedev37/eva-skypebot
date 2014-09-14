namespace Skypebot.Core.Commands
{
    using global::Skypebot.Core.Commands.Abstract;
    using global::Skypebot.Core.ObjectModel;

    /// <summary>
    /// Executes the specified JavaScript code.
    /// </summary>
    public class OffCommand : AbstractDirectCommand
    {
        #region Constants and Fields

        /// <summary>
        /// The goodbye message.
        /// </summary>
        private const string GoodbyeMessage = "Goodbye!";

        /// <summary>
        /// The state manager.
        /// </summary>
        private readonly DataManager dataManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OffCommand"/> class. 
        /// </summary>
        /// <param name="dataManager">
        /// The data manager.
        /// </param>
        public OffCommand(DataManager dataManager)
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
                return "turns off eva for current chat.";
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
                return "off";
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
                return "Usage:\r\n" + "just type 'eva off' in the chat which you would like eva stop working for.\r\n";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the type of the command (whether it is system or application level command).
        /// </summary>
        public override CommandType CommandType
        {
            get
            {
                return CommandType.Application;
            }
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
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The result string to send back as the answer.
        /// </returns>
        public override string Execute(SkypeContact contact, SkypeChat chat, string args)
        {
            this.dataManager.DisableChat(chat.Id);
            return GoodbyeMessage;
        }

        #endregion
    }
}
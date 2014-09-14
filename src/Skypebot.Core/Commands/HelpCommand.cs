namespace Skypebot.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using global::Skypebot.Core.Commands.Abstract;

    using global::Skypebot.Core.ObjectModel;

    /// <summary>
    /// The help command.
    /// </summary>
    public class HelpCommand : AbstractDirectCommand
    {
        #region Constants and Fields

        /// <summary>
        /// The commands.
        /// </summary>
        private readonly List<AbstractDirectCommand> commands;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpCommand"/> class.
        /// </summary>
        /// <param name="commands">
        /// The commands.
        /// </param>
        public HelpCommand(List<AbstractDirectCommand> commands)
        {
            this.commands = commands;
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
        /// Gets the help.
        /// </summary>
        public override string Help
        {
            get
            {
                return "Displays the main help page";
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "help";
            }
        }

        /// <summary>
        /// Gets the usage.
        /// </summary>
        public override string Usage
        {
            get
            {
                return "just type 'eva help' in the chat";
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
            string args = this.ExtractCommandArgument(message, chat.Contacts.Count == 2);

            if (string.IsNullOrWhiteSpace(args))
            {
                var sb = new StringBuilder();
                sb.AppendLine("The commands supported by this bot:");
                sb.AppendLine("help - displays this help message");

                foreach (AbstractDirectCommand command in this.commands)
                {
                    sb.AppendLine(string.Format("{0} - {1}", command.Name, command.Help));
                }

                sb.AppendLine();
                sb.Append("To get more commands, you should be subscribed to some project.");
                sb.Append("To get detailed help for a certain command, type a command ");
                sb.AppendLine("name after the 'help' command. For example:");
                sb.AppendLine("help on");

                return sb.ToString();
            }

            if (string.Equals(args, "help", StringComparison.CurrentCultureIgnoreCase))
            {
                return "Displays the main help page";
            }

            AbstractDirectCommand directCommand =
                this.commands.FirstOrDefault(c => string.Equals(c.Name, args, StringComparison.OrdinalIgnoreCase));
            if (directCommand != null)
            {
                return directCommand.Usage;
            }

            return "The command is not found. Please type 'help' to see available commands";
        }

        #endregion
    }
}
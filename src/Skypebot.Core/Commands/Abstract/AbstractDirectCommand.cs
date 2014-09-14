namespace Skypebot.Core.Commands.Abstract
{
    using System;
    using System.Text.RegularExpressions;

    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// The command that is invoked explicitly by its name.
    /// </summary>
    public abstract class AbstractDirectCommand : ISkypeCommand
    {
        #region Public Properties

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public virtual CommandType CommandType
        {
            get
            {
                return CommandType.Application;
            }
        }

        /// <summary>
        /// Gets the help (in short).
        /// </summary>
        /// <value>
        /// The help.
        /// </value>
        public abstract string Help { get; }

        /// <summary>
        /// Gets the name the command is invoked by.
        /// </summary>
        /// <value>
        /// The name the command is invoked by.
        /// </value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets the usage (detailed help).
        /// </summary>
        /// <value>
        /// The usage.
        /// </value>
        public abstract string Usage { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Checks whether provided message is a direct command (starts with 'eva' word).
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// True if message is a direct command, false otherwise.
        /// </returns>
        public static bool IsDirectCommand(string message)
        {
            return CommandConsts.BotRegex.Match(message.Trim()).Success;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public virtual void Dispose()
        {
        }

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
        public abstract string Execute(SkypeContact contact, SkypeChat chat, string message);

        /// <summary>
        /// The is applicable.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="isPrivateChat">
        /// The is Private Chat.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsApplicable(string message, bool isPrivateChat)
        {
            return string.Compare(this.Name, this.ExtractCommandName(message, isPrivateChat), StringComparison.OrdinalIgnoreCase) == 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Extracts the arguemnt from the message body.
        ///     E.g. if initial message is 'eva cc tb-daily' then result will be tb-daily.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="isPrivateChat">
        /// The is Private Chat.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected string ExtractCommandArgument(string message, bool isPrivateChat)
        {
            message = message.Trim();

            Match botMatch = CommandConsts.BotRegex.Match(message);
            string messageWithoutEva;

            if (botMatch.Success)
            {
                messageWithoutEva = CommandConsts.BotRegex.Replace(message, string.Empty).Trim();
            }
            else if (isPrivateChat)
            {
                // It's ok to for personal chat not to have 'eva command' before the command body. 
                messageWithoutEva = message;
            }
            else
            {
                return string.Empty;
            }

            Match directCommandMatch = CommandConsts.FirstTokenRegex.Match(messageWithoutEva);
            return messageWithoutEva.Replace(directCommandMatch.Value, string.Empty).Trim();
        }

        /// <summary>
        /// Extracts the command name from the message body.
        ///     E.g. if initial message is 'eva cc tb-daily' then result will be cc.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="isPrivateChat">
        /// The is Private Chat.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected string ExtractCommandName(string message, bool isPrivateChat)
        {
            message = message.Trim();

            Match botMatch = CommandConsts.BotRegex.Match(message);
            string messageWithoutEva;

            if (botMatch.Success)
            {
                messageWithoutEva = CommandConsts.BotRegex.Replace(message, string.Empty).Trim();
            }
            else if (isPrivateChat)
            {
                // It's ok to for personal chat not to have 'eva command' before the command body. 
                messageWithoutEva = message;
            }
            else
            {
                return string.Empty;
            }

            Match directCommandMatch = CommandConsts.FirstTokenRegex.Match(messageWithoutEva);

            if (directCommandMatch.Success)
            {
                return directCommandMatch.Value;
            }

            return string.Empty;
        }

        #endregion
    }
}
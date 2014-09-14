namespace Skypebot.Core.ObjectModel
{
    using System.Collections.Generic;

    using Skypebot.Core.Commands.Abstract;

    /// <summary>
    /// Responsible for parsing incoming message and returning appropriate command.
    /// </summary>
    public class MessageParser
    {
        #region Public Methods and Operators

        /// <summary>
        /// Parsing given message and returns command that match the message.
        /// If there is no such message - returns null.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="directCommands">
        /// The direct Commands.
        /// </param>
        /// <param name="replaceCommands">
        /// The replace Commands.
        /// </param>
        /// <param name="isPrivateChat">
        /// The is Private Chat.
        /// </param>
        /// <returns>
        /// The <see cref="ISkypeCommand"/>.
        /// </returns>
        public ISkypeCommand ParseMessage(
            string message, 
            IEnumerable<AbstractDirectCommand> directCommands, 
            IEnumerable<AbstractReplaceCommand> replaceCommands, 
            bool isPrivateChat)
        {
            foreach (AbstractDirectCommand command in directCommands)
            {
                if (command.IsApplicable(message, isPrivateChat))
                {
                    return command;
                }
            }

            foreach (AbstractReplaceCommand command in replaceCommands)
            {
                if (command.IsApplicable(message))
                {
                    return command;
                }
            }

            return null;
        }

        #endregion
    }
}
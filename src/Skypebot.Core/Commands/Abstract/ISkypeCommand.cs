namespace Skypebot.Core.Commands.Abstract
{   
    using System;

    using global::Skypebot.Core.ObjectModel;

    /// <summary>
    /// The command type.
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// The system.
        /// </summary>
        System = 0, 

        /// <summary>
        /// The application.
        /// </summary>
        Application = 1
    }

    /// <summary>
    /// The base command for skype bot.
    /// </summary>
    public interface ISkypeCommand : IDisposable
    {
        /// <summary>
        /// Gets the projectId current command relates to.
        /// </summary>
        int ProjectId { get; }

        /// <summary>
        /// Gets the type of the current command (application level or system one).
        /// </summary>
        CommandType CommandType { get; }

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
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The result string to send back as the answer.
        /// </returns>
        string Execute(SkypeContact contact, SkypeChat chat, string message);

        #endregion
    }
}
namespace Skypebot.Core.Commands.Abstract
{
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// The ReplaceCommand interface.
    /// </summary>
    public abstract class AbstractReplaceCommand : ISkypeCommand
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
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        #endregion

        #region Public Methods and Operators

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
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract bool IsApplicable(string message);

        #endregion
    }
}
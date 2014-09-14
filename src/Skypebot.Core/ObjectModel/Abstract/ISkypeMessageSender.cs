namespace Skypebot.Core.ObjectModel.Abstract
{
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// The SkypeMessageSender interface.
    /// </summary>
    public interface ISkypeMessageSender
    {
        #region Public Methods and Operators

        /// <summary>
        /// The send message to specific contact.
        /// </summary>
        /// <param name="chat">
        /// The chat.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        void SendMessageToChat(SkypeChat chat, string message);

        /// <summary>
        /// The send message to specific skype chat.
        ///     Talk between 2 peoples is also a chat.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="chat">
        /// The chat.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        void SendMessageToContact(SkypeContact sender, SkypeChat chat, string message);

        #endregion
    }
}
namespace Skypebot.Core.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using SKYPE4COMLib;

    using Skypebot.Core.Commands.Abstract;
    using Skypebot.Core.ObjectModel.Abstract;

    /// <summary>
    /// The basic wrapper over the Skype API.
    /// </summary>
    public class SkypeClient : ISkypeMessageSender
    {
        #region Constants and Fields

        /// <summary>
        /// Manages the contacts and chats instances.
        /// </summary>
        private readonly ContactsManager contactsManager;

        /// <summary>
        /// Logging all happening stuff.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Parsed incoming messages and returns appripriate command instances.
        /// </summary>
        private readonly MessageParser messageParser;

        /// <summary>
        /// The API object that provides access to the skype application.
        /// </summary>
        private readonly Skype skype;

        /// <summary>
        /// Keeps state of the skype client.
        /// When client is starting it should restore the state from the database.
        /// </summary>
        private readonly DataManager dataManager;

        /// <summary>
        /// Responsible for providing proper set of commands for particular message/chat/user.
        /// </summary>
        private readonly CommandManager commandManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SkypeClient"/> class and immediatly starts it.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public SkypeClient(ILogger logger)
        {
            // Starting bot and attaching it to the skype app.
            this.skype = new Skype();
            this.skype.Attach(7, false);

            if (!this.skype.Client.IsRunning)
            {
                this.skype.Client.Start(true, true);
            }

            // Initialiazing skype bot.
            this.logger = logger;

            this.contactsManager = new ContactsManager(this.skype);
            this.dataManager = new DataManager();
            this.commandManager = new CommandManager(this.dataManager, this);
            this.messageParser = new MessageParser();

            this.skype.MessageStatus += this.OnMessageReceived;
            this.skype.UserAuthorizationRequestReceived += this.OnAuthorizationRequestReceived;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Receives 'add to friend' request and processes it.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        public void ReceiveAuthorizationRequest(User user)
        {
            this.logger.LogMessage(string.Format("Received 'add to friend' request from <{0}>", user.Handle));

            if (user.IsAuthorized)
            {
                return;
            }

            string replyMessage;
            bool shouldBeAuthirized;
            SkypeContact contact = this.contactsManager.GetSkypeContact(user.Handle);

            this.contactsManager.TryAuthorizeUser(contact, out replyMessage, out shouldBeAuthirized);

            if (shouldBeAuthirized)
            {
                user.IsAuthorized = true;
            }

            this.SendMessageToContact(contact, null, replyMessage);
        }

        /// <summary>
        /// The receive message.
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
        public void ReceiveMessage(SkypeContact contact, SkypeChat chat, string message)
        {
            this.logger.LogMessage(
                string.Format("Received message <{0}> from <{1}> in chat <{2}>", message, contact.Id, chat.Id));

            List<ISkypeCommand> commands = this.commandManager.GetCommandsForChat(chat);

            ISkypeCommand command = this.messageParser.ParseMessage(
                message,
                commands.OfType<AbstractDirectCommand>(),
                commands.OfType<AbstractReplaceCommand>(),
                chat.Contacts.Count == 2);

            if (command != null)
            {
                ThreadPool.QueueUserWorkItem(
                    data =>
                        {
                            string response;
                            try
                            {
                                response = command.Execute(contact, chat, message);
                            }
                            catch (Exception)
                            {
                                response =
                                    "Something bad happened with command execution. Please address it to developers or try later";
                            }

                            this.SendMessageToChat(chat, response);
                        });
            }
            else if (!this.dataManager.IsChatEnabled(chat.Id))
            {
                // for disabled chats no messages should be shown except the replies to system
                // commands (like on/help/off).
                return;
            }
            else if (AbstractDirectCommand.IsDirectCommand(message) && chat.Contacts.Count != 2)
            {
                // public chats ignore messages that don't match commands since they may be addressed
                // not to eva.
                this.SendMessageToChat(chat, "No such command found");
            }
            else if (chat.Contacts.Count == 2)
            {
                // For private chats any message is addressed to eva and if the message doesn't match
                // commands then it is considered to be an incorrect command (EVA-4 issue).
                this.SendMessageToChat(chat, "No such command found");
            }
        }

        /// <summary>
        /// The send message to chat.
        /// </summary>
        /// <param name="chat">
        /// The chat.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void SendMessageToChat(SkypeChat chat, string message)
        {
            if (!string.IsNullOrEmpty(message) && this.skype.Chat[chat.Id] != null)
            {
                this.logger.LogMessage(string.Format("Sending message <{0}> to chat <{1}>", message, chat.Id));

                try
                {
                    this.skype.Chat[chat.Id].SendMessage(message);
                }
                catch (Exception ex)
                {
                    this.logger.LogException(
                        string.Format("Failed on sending message <{0}> to chat <{1}>", message, chat.Id), 
                        ex);
                }
            }
        }

        /// <summary>
        /// The send message to contact.
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
        public void SendMessageToContact(SkypeContact contact, SkypeChat chat, string message)
        {
            string chatId = chat != null ? chat.Id : "'no chat'";

            this.logger.LogMessage(
                string.Format("Sending message <{0}> to <{1}> in chat <{2}>", message, contact.Id, chatId));

            try
            {
                this.skype.SendMessage(contact.Id, message);
            }
            catch (Exception ex)
            {
                this.logger.LogException(
                    string.Format(
                        "Failed on sending message <{0}> from <{1}> in chat <{2}>", 
                        message, 
                        contact.Id, 
                        chatId), 
                    ex);
            }
        }

        /// <summary>
        /// The send message to contact.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void SendMessageToContact(SkypeContact contact, string message)
        {
            this.SendMessageToContact(contact, null, message);
        }

        /// <summary>
        /// Updates info about projects.
        /// </summary>
        public void UpdateProjectsInfo()
        {
            this.commandManager.UpdateData();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles receiving authirization request.
        /// </summary>
        /// <param name="user">
        /// User, trying to add eva to friends.
        /// </param>
        private void OnAuthorizationRequestReceived(User user)
        {
            this.ReceiveAuthorizationRequest(user);
        }

        /// <summary>
        /// Handles incoming message.
        /// </summary>
        /// <param name="message">
        /// Received message.
        /// </param>
        /// <param name="status">
        /// Message status.
        /// </param>
        private void OnMessageReceived(ChatMessage message, TChatMessageStatus status)
        {
            if (status == TChatMessageStatus.cmsReceived)
            {
                if (!message.Sender.IsAuthorized || message.Sender.IsBlocked)
                {
                    this.logger.LogMessage(
                        string.Format("Receiving message from unauthorized/blocked user: <{0}>", message.Sender.Handle));

                    return;
                }

                SkypeContact contact = this.contactsManager.GetSkypeContact(message.Sender.Handle);
                SkypeChat chat = this.contactsManager.GetSkypeChat(message.ChatName);

                this.ReceiveMessage(contact, chat, message.Body);
            }
        }

        #endregion
    }
}
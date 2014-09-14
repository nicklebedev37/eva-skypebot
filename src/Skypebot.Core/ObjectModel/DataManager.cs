namespace Skypebot.Core.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Skypebot.DataAccessLayer;
    using Skypebot.DataAccessLayer.Repositories;

    /// <summary>
    /// The data manager.
    /// </summary>
    public class DataManager
    {
        #region Constants and Fields

        /// <summary>
        /// The chat project mapping.
        /// </summary>
        private readonly Dictionary<string, int> chatProjectMapping;

        /// <summary>
        /// The chat subscription repository.
        /// </summary>
        private readonly ChatSubscriptionRepository chatSubscriptionRepository = new ChatSubscriptionRepository();

        /// <summary>
        /// The disabled chat repository.
        /// </summary>
        private readonly DisabledChatRepository disabledChatRepository = new DisabledChatRepository();

        /// <summary>
        /// The disabled chats.
        /// </summary>
        private readonly HashSet<string> disabledChats;

        /// <summary>
        /// The feedback repository.
        /// </summary>
        private readonly FeedbackRepository feedbackRepository = new FeedbackRepository();

        /// <summary>
        /// The project repository.
        /// </summary>
        private readonly ProjectRepository projectRepository = new ProjectRepository();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager"/> class.
        /// </summary>
        public DataManager()
        {
            this.disabledChats = new HashSet<string>();
            this.chatProjectMapping = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            List<DisabledChat> disabledChatsInfo = this.disabledChatRepository.GetAll();
            foreach (DisabledChat disabledChat in disabledChatsInfo)
            {
                this.disabledChats.Add(disabledChat.Name);
            }

            List<ChatSubscription> chatSubscriptionsInfo = this.chatSubscriptionRepository.GetAll();
            foreach (ChatSubscription chatSub in chatSubscriptionsInfo)
            {
                this.chatProjectMapping.Add(chatSub.ChatName, chatSub.ProjectId);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a new feedback message.
        /// </summary>
        /// <param name="userSkypeId">
        /// The user skype id.
        /// </param>
        /// <param name="feedback">
        /// The feedback.
        /// </param>
        public void AddFeedback(string userSkypeId, string feedback)
        {
            this.feedbackRepository.Create(new Feedback { UsersSkypeId = userSkypeId, Text = feedback });
        }

        /// <summary>
        /// Disables the chat with given name.
        /// </summary>
        /// <param name="chatName">
        /// The chat name.
        /// </param>
        public void DisableChat(string chatName)
        {
            DisabledChat chat =
                this.disabledChatRepository.GetAll()
                    .FirstOrDefault(c => string.Compare(c.Name, chatName, StringComparison.OrdinalIgnoreCase) == 0);

            if (chat == null)
            {
                this.disabledChatRepository.Create(new DisabledChat { Name = chatName });
            }

            if (!this.disabledChats.Contains(chatName))
            {
                this.disabledChats.Add(chatName);
            }
        }

        /// <summary>
        ///  Enables the chat with given name.
        /// </summary>
        /// <param name="chatName">
        /// The chat name.
        /// </param>
        public void EnabledChat(string chatName)
        {
            DisabledChat chat =
                this.disabledChatRepository.GetAll()
                    .FirstOrDefault(c => string.Compare(c.Name, chatName, StringComparison.OrdinalIgnoreCase) == 0);

            if (chat != null)
            {
                this.disabledChatRepository.Delete(chat);
            }

            if (this.disabledChats.Contains(chatName))
            {
                this.disabledChats.Remove(chatName);
            }
        }

        /// <summary>
        /// Returns collection of names of chat subscribed to given project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public IEnumerable<string> GetChatsForProject(int projectId)
        {
            var chatNames = new List<string>();
            foreach (var item in this.chatProjectMapping)
            {
                if (item.Value == projectId)
                {
                    chatNames.Add(item.Key);
                }
            }

            return chatNames;
        }

        /// <summary>
        /// Returns project id for chat (if chat subscribed to this project).
        /// If there is no such chat - return 0.
        /// </summary>
        /// <param name="chatName">
        /// The chat name.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GetProjectForChat(string chatName)
        {
            if (this.chatProjectMapping.ContainsKey(chatName))
            {
                return this.chatProjectMapping[chatName];
            }

            return 0;
        }

        /// <summary>
        /// Returns a project id for given project alias/name.
        /// </summary>
        /// <param name="projectName">
        /// Project alias/name.
        /// </param>
        /// <returns>
        /// The project id.
        /// </returns>
        public int GetProjectId(string projectName)
        {
            Project project =
                this.projectRepository.GetAll()
                    .FirstOrDefault(p => string.Compare(p.Alias, projectName, StringComparison.OrdinalIgnoreCase) == 0);

            return project != null ? project.Id : 0;
        }

        /// <summary>
        /// Returns the whole list of projects.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public IEnumerable<Project> GetProjects()
        {
            return this.projectRepository.GetAll();
        }

        /// <summary>
        /// Returns boolean flag indicating whether chat is enabled.
        /// </summary>
        /// <param name="chatName">
        /// The chat name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsChatEnabled(string chatName)
        {
            return
                this.disabledChats.FirstOrDefault(
                    c => string.Compare(c, chatName, StringComparison.OrdinalIgnoreCase) == 0) == null;
        }

        /// <summary>
        /// Subscribed given chat to the given project.
        /// </summary>
        /// <param name="chatName">
        /// The chat name.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        public void SubscribeChat(string chatName, int projectId)
        {
            ChatSubscription chatSubscription =
                this.chatSubscriptionRepository.GetAll()
                    .FirstOrDefault(c => string.Compare(c.ChatName, chatName, StringComparison.OrdinalIgnoreCase) == 0);

            if (chatSubscription == null)
            {
                // chat wasn't subscribed to any project yet.
                this.chatSubscriptionRepository.Create(
                    new ChatSubscription { ChatName = chatName, ProjectId = projectId });
            }
            else
            {
                // chat was subscribed to smth already.
                this.chatSubscriptionRepository.Update(
                    new ChatSubscription { Id = chatSubscription.Id, ChatName = chatName, ProjectId = projectId });
            }

            if (!this.chatProjectMapping.ContainsKey(chatName))
            {
                this.chatProjectMapping.Add(chatName, projectId);
            }
            else
            {
                this.chatProjectMapping[chatName] = projectId;
            }
        }

        /// <summary>
        /// Unsubscribes chat from any project.
        /// </summary>
        /// <param name="chatName">
        /// The chat name.
        /// </param>
        public void UnsubscribeChat(string chatName)
        {
            ChatSubscription chatSubscription =
                this.chatSubscriptionRepository.GetAll()
                    .FirstOrDefault(c => string.Compare(c.ChatName, chatName, StringComparison.OrdinalIgnoreCase) == 0);

            this.chatSubscriptionRepository.Delete(chatSubscription);

            if (this.chatProjectMapping.ContainsKey(chatName))
            {
                this.chatProjectMapping.Remove(chatName);
            }
        }

        #endregion
    }
}
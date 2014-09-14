namespace Skypebot.Core.ObjectModel
{
    using SKYPE4COMLib;

    /// <summary>
    /// Responsible for managing user-authentication stuff.
    /// </summary>
    public class ContactsManager
    {
        #region Constants and Fields

        /// <summary>
        /// The skype.
        /// </summary>
        private readonly Skype skype;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsManager"/> class.
        /// </summary>
        /// <param name="skype">
        /// The skype.
        /// </param>
        public ContactsManager(Skype skype)
        {
            this.skype = skype;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrives chat instance by id.
        /// </summary>
        /// <param name="id">
        /// Id of the chat.
        /// </param>
        /// <returns>
        /// Found skype chat instance of null if wrong id was supplied.
        /// </returns>
        public SkypeChat GetSkypeChat(string id)
        {
            var chat = new SkypeChat { Id = id };
            foreach (User user in this.skype.Chat[id].Members)
            {
                chat.Contacts.Add(new SkypeContact { Id = user.Handle });
            }

            return chat;
        }

        /// <summary>
        /// The get skype contact.
        /// </summary>
        /// <param name="id">
        /// The handle.
        /// </param>
        /// <returns>
        /// The <see cref="SkypeContact"/>.
        /// </returns>
        public SkypeContact GetSkypeContact(string id)
        {
            foreach (User user in this.skype.Friends)
            {
                if (user.Handle == id)
                {
                    var contact = new SkypeContact { Id = id };

                    string[] nameTokens = user.DisplayName.Split(' ');
                    if (nameTokens.Length > 0)
                    {
                        contact.FirstName = nameTokens[0];
                    }

                    if (nameTokens.Length > 1)
                    {
                        contact.LastName = nameTokens[1];
                    }

                    return contact;
                }
            }

            return new SkypeContact { Id = id };
        }

        /// <summary>
        /// The try authorize user.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <param name="replyMessage">
        /// The reply message.
        /// </param>
        /// <param name="shouldBeAuthorized">
        /// The should be authorized.
        /// </param>
        public void TryAuthorizeUser(SkypeContact contact, out string replyMessage, out bool shouldBeAuthorized)
        {
            replyMessage = string.Empty;
            shouldBeAuthorized = false;

            replyMessage += "Hello " + contact.Id + "! I'm the Eva bot. ";

            this.skype.UsersWaitingAuthorization.RemoveAll();
        }

        #endregion
    }
}
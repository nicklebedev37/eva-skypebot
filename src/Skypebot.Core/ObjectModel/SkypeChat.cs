namespace Skypebot.Core.ObjectModel
{
    using System.Collections.Generic;

    /// <summary>
    /// The skype chat.
    /// </summary>
    public class SkypeChat
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SkypeChat"/> class.
        /// </summary>
        public SkypeChat()
        {
            this.Contacts = new List<SkypeContact>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        public List<SkypeContact> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// Id is supposed to be unique.
        /// </summary>
        public string Id { get; set; }

        #endregion
    }
}
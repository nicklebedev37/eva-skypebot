namespace Skypebot.Core.ObjectModel
{
    /// <summary>
    /// A property bag that keeps skype contact info.
    /// </summary>
    public class SkypeContact
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }
    }
}

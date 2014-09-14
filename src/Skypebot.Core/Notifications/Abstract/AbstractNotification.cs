namespace Skypebot.Core.Notifications.Abstract
{
    /// <summary>
    /// Provides basic functionality for getting notification info.
    /// </summary>
    public abstract class AbstractNotification
    {
        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Returns whether status of resource/thing has changed from the previous call.
        /// </summary>
        /// <param name="message">The output message describing changes.</param>
        /// <returns>
        ///   <c>true</c>, if there are any changes; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool HasChanged(out string message);
    }
}

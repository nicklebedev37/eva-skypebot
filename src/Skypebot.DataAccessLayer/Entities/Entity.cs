namespace Skypebot.DataAccessLayer.Entities
{
    /// <summary>
    /// The base class for all database entities.
    /// It was introduced mostly to get access to generic entity by id.
    /// </summary>
    public abstract class Entity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public virtual int Id { get; set; }

        #endregion
    }
}
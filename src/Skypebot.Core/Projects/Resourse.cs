namespace Skypebot.Core.Projects
{
    using global::Skypebot.Core.Commands.Interfaces;

    /// <summary>
    /// The i resourse.
    /// </summary>
    public abstract class Resourse
    {
        #region Constants and Fields

        /// <summary>
        /// The command.
        /// </summary>
        protected ISkypeCommand command;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the command.
        /// </summary>
        public ISkypeCommand Command
        {
            get
            {
                return this.command;
            }
        }

        #endregion
    }
}
namespace Skypebot.Core.ObjectModel.Abstract
{
    using System;

    /// <summary>
    /// The Logger interface.
    /// </summary>
    public interface ILogger
    {
        #region Public Methods and Operators

        /// <summary>
        /// The log exception.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="ex">
        /// The ex.
        /// </param>
        void LogException(string message, Exception ex);

        /// <summary>
        /// The log message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void LogMessage(string message);

        #endregion
    }
}
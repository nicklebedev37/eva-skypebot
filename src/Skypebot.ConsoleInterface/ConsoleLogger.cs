namespace Skypebot.ConsoleInterface
{
    using System;

    using Skypebot.Core.ObjectModel.Abstract;

    /// <summary>
    /// The console logger.
    /// </summary>
    internal class ConsoleLogger : ILogger
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
        public void LogException(string message, Exception ex)
        {
            Console.WriteLine("message: {0}, exception info: {1}", message, ex);
        }

        /// <summary>
        /// The log message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void LogMessage(string message)
        {
            Console.WriteLine("message: {0}", message);
        }

        #endregion
    }
}

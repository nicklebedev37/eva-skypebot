namespace Skypebot.Core.Commands
{
    using System;
    using System.Threading;

    using Jint;

    using Skypebot.Core.Commands.Abstract;
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// Executes the specified JavaScript code.
    /// </summary>
    public class JsCommand : AbstractDirectCommand
    {
        #region Constants and Fields

        /// <summary>
        /// The timeout of the command execution.
        /// </summary>
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the help (in short).
        /// </summary>
        /// <value>
        /// The help.
        /// </value>
        public override string Help
        {
            get
            {
                return "executes the javascript code like in a browser console";
            }
        }

        /// <summary>
        /// Gets the name the command is invoked by.
        /// </summary>
        /// <value>
        /// The name the command is invoked by.
        /// </value>
        public override string Name
        {
            get
            {
                return "js";
            }
        }

        /// <summary>
        /// Gets the usage.
        /// </summary>
        /// <value>
        /// The usage.
        /// </value>
        public override string Usage
        {
            get
            {
                return "Usage:\r\n"
                       + "js 'javascript code' - executes the expression and shows the result of its execution\r\n"
                       + "Examples:\r\n" + "js var x = 2; x = x * Math.PI + [1, 2, 3].slice(1, 2)\r\n";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Executes the command on answer to the specified chat name.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <param name="chat">
        /// The chat.
        /// </param>
        /// <param name="message">
        /// The args.
        /// </param>
        /// <returns>
        /// The result string to send back as the answer.
        /// </returns>
        public override string Execute(SkypeContact contact, SkypeChat chat, string message)
        {
            string argument = this.ExtractCommandArgument(message, chat.Contacts.Count == 2);

            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(argument))
            {
                Thread thread = new Thread(
                    () =>
                        {
                            try
                            {
                                result = new Engine().Execute(argument).GetCompletionValue().ToString();
                            }
                            catch (Exception)
                            {
                                result = "Sorry, couldn't parse.";
                            }
                        });

                thread.Start();

                if (!thread.Join(Timeout))
                {
                    thread.Abort();
                    result = "Sorry, your code took too long and has been aborted.";
                }
            }

            return result;
        }

        #endregion
    }
}

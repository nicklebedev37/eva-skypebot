namespace Skypebot.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Skypebot.Core.Commands.Abstract;
    using global::Skypebot.Core.ObjectModel;
    using System.Configuration;

    /// <summary>
    /// The random guy.
    /// </summary>
    public class RandomGuyCommand : AbstractDirectCommand
    {
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
                return "randomguy";
            }
        }

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
                return "randomly selects one of contacts in the current chat";
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
                return "Usage:\r\n" + "randomguy - uniformly selects and shows skype contact of the current chat\r\n"
                       + "Examples:\r\n" + "randomguy\r\n";
            }
        }

        /// <summary>
        /// Executes the command on answer to the specified chat name.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <param name="chat">
        /// The chat.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The result string to send back as the answer.
        /// </returns>
        public override string Execute(SkypeContact contact, SkypeChat chat, string args)
        {
            string botSkypeId = ConfigurationManager.AppSettings["botskypeid"];

            List<SkypeContact> contacts = chat.Contacts.Where(s => s.Id != botSkypeId).ToList();
            return contacts[(new Random()).Next(contacts.Count)].Id;
        }
    }
}

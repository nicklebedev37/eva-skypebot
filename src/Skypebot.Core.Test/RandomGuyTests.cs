namespace Skypebot.Core.Test
{
    using System.Collections.Generic;
    using System.Configuration;

    using NUnit.Framework;

    using Skypebot.Core.Commands;
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// Tests set for command 'random guy'.
    /// </summary>
    [TestFixture]
    public class RandomGuyTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// Positive test for the command.
        /// </summary>
        [TestCase]
        public void RandomGuyCommandExecute_Success()
        {
            string botSkypeId = ConfigurationManager.AppSettings["botskypeid"];

            var randomGuy = new RandomGuyCommand();
            string reply = randomGuy.Execute(
                null, 
                new SkypeChat
                    {
                        Contacts =
                            new List<SkypeContact>
                                {
                                    new SkypeContact { Id = "a" }, 
                                    new SkypeContact { Id = "b" }, 
                                    new SkypeContact { Id = "c" }, 
                                    new SkypeContact { Id = botSkypeId }
                                }
                    }, 
                "eva randomguy");

            Assert.IsTrue(reply == "a" || reply == "b" || reply == "c");
        }

        #endregion
    }
}
namespace Skypebot.Core.Test
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Skypebot.Core.Commands;
    using Skypebot.Core.Commands.Abstract;
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// The js command test.
    /// </summary>
    [TestFixture]
    public class CommandsTests
    {
        #region Constants and Fields

        /// <summary>
        /// The fake direct commands.
        /// </summary>
        private List<AbstractDirectCommand> fakeDirectCommands;

        /// <summary>
        /// The fake replace commands.
        /// </summary>
        private List<AbstractReplaceCommand> fakeReplaceCommands;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsTests"/> class.
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            this.fakeDirectCommands = new List<AbstractDirectCommand>
                                          {
                                              new JsCommand(), 
                                              new RandomGuyCommand(), 
                                              new OnCommand(null), 
                                              new OffCommand(null), 
                                              new SubCommand(null), 
                                              new UnsubCommand(null), 
                                              new FeedbackCommand(null)
                                          };

            AbstractDirectCommand helpCommand = new HelpCommand(this.fakeDirectCommands);
            this.fakeDirectCommands.Add(helpCommand);

            this.fakeReplaceCommands = new List<AbstractReplaceCommand>
                                           {
                                               new JiraCommand(
                                                   string.Empty, 
                                                   string.Empty, 
                                                   string.Empty, 
                                                   new List<string>
                                                       {
                                                           "INTERNAL", 
                                                           "COREBUILDS"
                                                       })
                                           };
        }

        /// <summary>
        /// The positive test for parsing feedback command.
        /// </summary>
        [TestCase]
        public void ParseFeedbackCommandTest()
        {
            Assert.IsTrue(this.RunParseTest("eva feedback your bot is awesome!!!") is FeedbackCommand);
        }

        /// <summary>
        /// The positive test for parsing help command.
        /// </summary>
        [TestCase]
        public void ParseHelpCommandTest()
        {
            Assert.IsTrue(this.RunParseTest("eva help") is HelpCommand);
        }

        /// <summary>
        /// The positive test for parsing jira command without caring of registry/case.
        /// </summary>
        [TestCase]
        public void ParseJiraCommandCaseInsensitiveTest()
        {
            Assert.IsTrue(this.RunParseTest("The coreBuildS-321 bug should be parsed too!") is JiraCommand);
        }

        /// <summary>
        /// The positive test for parsing jira command.
        /// </summary>
        [TestCase]
        public void ParseJiraCommandTest()
        {
            Assert.IsTrue(this.RunParseTest("Hi Jim, please take a look internal-123 bug!") is JiraCommand);
        }

        /// <summary>
        /// The positive test for parsing js command.
        /// </summary>
        [TestCase]
        public void ParseJsCommandTest()
        {
            Assert.IsTrue(this.RunParseTest("eva js var x=3;") is JsCommand);
        }

        /// <summary>
        /// The positive test for parsing off command.
        /// </summary>
        [TestCase]
        public void ParseOffCommandTest()
        {
            Assert.IsTrue(this.RunParseTest("eva off hi") is OffCommand);
        }

        /// <summary>
        /// The positive test for parsing on command.
        /// </summary>
        [TestCase]
        public void ParseOnCommandTest()
        {
            Assert.IsTrue(this.RunParseTest("eva on") is OnCommand);
        }

        /// <summary>
        /// The positive test for parsing random guy command.
        /// </summary>
        [TestCase]
        public void ParseRandomGuyCommandTest()
        {
            Assert.IsTrue(this.RunParseTest("eva randomguy") is RandomGuyCommand);
        }

        /// <summary>
        /// The positive test for parsing sub command.
        /// </summary>
        [TestCase]
        public void ParseSubCommandTest()
        {
            Assert.IsTrue(this.RunParseTest("eva sub projectA") is SubCommand);
        }

        /// <summary>
        /// The positive test for parsing unsub command.
        /// </summary>
        [TestCase]
        public void ParseUnsubCommandTest()
        {
            Assert.IsTrue(this.RunParseTest("eva unsub") is UnsubCommand);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The run parse test.
        /// </summary>
        /// <param name="inputMessage">
        /// The input message.
        /// </param>
        /// <returns>
        /// The <see cref="ISkypeCommand"/>.
        /// </returns>
        private ISkypeCommand RunParseTest(string inputMessage)
        {
            var parser = new MessageParser();
            return parser.ParseMessage(inputMessage, this.fakeDirectCommands, this.fakeReplaceCommands, false);
        }

        #endregion
    }
}
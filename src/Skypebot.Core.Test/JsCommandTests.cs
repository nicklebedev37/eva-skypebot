namespace Skypebot.Core.Test
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Skypebot.Core.Commands;
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// The js command tests.
    /// </summary>
    [TestFixture]
    public class JsCommandTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// The js command positive test but this test also check whether the command will do proper time.
        /// </summary>
        [TestCase]
        public void JsCommandExecuteWithTrim_Success()
        {
            var js = new JsCommand();
            Assert.IsTrue(js.Execute(null, this.GetFakeSkypeChat(), " eva js  var x = 1 * 2 * 3 + 4; x;") == "10");
        }

        /// <summary>
        /// The js command negative test.
        /// </summary>
        [TestCase]
        public void JsCommandExecute_Failure()
        {
            var js = new JsCommand();
            Assert.IsTrue(
                js.Execute(null, this.GetFakeSkypeChat(), "eva js you shall not parse!") == "Sorry, couldn't parse.");
        }

        /// <summary>
        /// The js command positive test.
        /// </summary>
        [TestCase]
        public void JsCommandExecute_Success()
        {
            var js = new JsCommand();
            Assert.IsTrue(js.Execute(null, this.GetFakeSkypeChat(), "eva js var x = 3, y = 6; x *= y; x;") == "18");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a fake instance of skype chat object.
        /// </summary>
        /// <returns>
        /// The skype chat.
        /// </returns>
        private SkypeChat GetFakeSkypeChat()
        {
            return new SkypeChat();
        }

        #endregion
    }
}
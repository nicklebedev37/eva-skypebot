namespace Skypebot.ConsoleInterface
{
    using System;
    using Skypebot.Core.ObjectModel;

    /// <summary>
    /// Program object.
    /// </summary>
    public class Program
    {
        #region Constants and Fields

        /// <summary>
        /// The line that stops the app after user inputs it.
        /// </summary>
        private const string TerminateLine = "exit";

        /// <summary>
        /// The line that cause updates the info about projects.
        /// </summary>
        private const string UpdateProjectsInfoLine = "update";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Entry point of the console app.
        /// </summary>
        public static void Main()
        {
            var program = new Program();
            program.Run();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Stars the command line logic.
        /// </summary>
        private void Run()
        {
            var skypeClient = new SkypeClient(new ConsoleLogger());

            string readLine = string.Empty;
            while (readLine != TerminateLine)
            {
                readLine = Console.ReadLine();

                if (string.Compare(readLine, UpdateProjectsInfoLine, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    skypeClient.UpdateProjectsInfo();
                }
            }
        }

        #endregion
    }
}
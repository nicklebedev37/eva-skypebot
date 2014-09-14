namespace Skypebot.Core.Commands
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// The command consts.
    /// </summary>
    public static class CommandConsts
    {
        #region Constants and Fields

        /// <summary>
        /// The main regex to recognize the bot command.
        /// </summary>
        public static readonly Regex BotRegex = new Regex(@"^eva\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Extracts first token from the command.
        ///     It is used to identify direct command name.
        /// </summary>
        public static readonly Regex FirstTokenRegex = new Regex(@"^\s*\w+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion
    }
}
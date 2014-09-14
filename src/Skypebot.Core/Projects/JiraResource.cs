namespace Skypebot.Core.Projects
{
    using System.Collections.Generic;
    using global::Skypebot.Core.Commands;
    using global::Skypebot.Core.Commands.Interfaces;

    public class JiraResource : Resourse
    {
        public JiraResource(string url, string login, string password, List<string> projectNames)
        {
            command = new JiraCommand(url, login, password, projectNames);
        }

        public ISkypeCommand Command
        {
            get
            {
                return command;
            }
        }
    }
}

namespace Skypebot.Core.Projects
{
    using System.Collections.Generic;

    using global::Skypebot.Core.Commands;

    public class CruiseControlResource : Resourse
    {
        public CruiseControlResource(string url, string targetServer, List<string> buildNames, Project project)
        {
            command = new CruiseControlCommand(url, targetServer, buildNames, project);
        }
    }
}

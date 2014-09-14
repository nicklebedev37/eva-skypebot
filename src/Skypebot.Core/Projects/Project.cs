namespace Skypebot.Core.Projects
{
    using System.Collections.Generic;

    public class Project
    {
        public string Name { get; set; }

        public Project()
        {
            ResourceList = new List<Resourse>();
        }

        public List<Resourse> ResourceList { get; set; }
    }
}

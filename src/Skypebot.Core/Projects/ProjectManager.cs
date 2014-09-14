namespace Skypebot.Core.Projects
{
    using System.Collections.Generic;
    using System.Linq;

    using global::Skypebot.Core.ObjectModel;

    /// <summary>
    /// The project manager.
    /// </summary>
    public class ProjectManager
    {
        public List<Project> projects { get; set; }

        public List<SkypeChat> skypeChats { get; set; }

        // Mapping of chatName to the appropriate Project.
        public Dictionary<string, Project> chatProjectMapping; 

        public ProjectManager()
        {
            projects = new List<Project>();
            skypeChats = new List<SkypeChat>();
            chatProjectMapping = new Dictionary<string, Project>();
        }

        public void AddProject(Project project)
        {
            projects.Add(project);
        }

        public void AddChat(SkypeChat chat, Project project)
        {
            this.RemoveChat(chat);

            skypeChats.Add(chat);
            chatProjectMapping[chat.Id] = project;
        }

        public void RemoveChat(SkypeChat chat)
        {
            SkypeChat foundChat = skypeChats.Where(c => c.Id == chat.Id).FirstOrDefault();

            if (foundChat != null)
            {
                skypeChats.Remove(foundChat);
            }
        }
    }
}

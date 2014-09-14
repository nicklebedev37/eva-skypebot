namespace Skypebot.Core.ObjectModel
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Xml.Linq;

    using Skypebot.Core.Commands;
    using Skypebot.Core.Commands.Abstract;
    using Skypebot.Core.Notifications;
    using Skypebot.Core.Notifications.Abstract;
    using Skypebot.Core.ObjectModel.Abstract;
    using Skypebot.DataAccessLayer;

    /// <summary>
    /// The command manager.
    /// </summary>
    public class CommandManager
    {
        #region Constants and Fields

        /// <summary>
        /// The notify timer period.
        /// </summary>
        private const int NotifyTimerPeriod = 5000;

        /// <summary>
        /// The common commands.
        /// </summary>
        private readonly List<AbstractDirectCommand> commonCommands;

        /// <summary>
        /// The message sender.
        /// </summary>
        private readonly ISkypeMessageSender messageSender;

        /// <summary>
        /// The state manager.
        /// </summary>
        private readonly DataManager dataManager;

        /// <summary>
        /// The notifications.
        /// </summary>
        private List<AbstractNotification> notifications;

        /// <summary>
        /// The projects commands.
        /// </summary>
        private List<ISkypeCommand> projectsCommands;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandManager"/> class.
        /// </summary>
        /// <param name="dataManager">
        /// The state manager.
        /// </param>
        /// <param name="messageSender">
        /// The message sender.
        /// </param>
        public CommandManager(DataManager dataManager, ISkypeMessageSender messageSender)
        {
            this.messageSender = messageSender;
            this.dataManager = dataManager;

            // Initializing commands.
            this.commonCommands = new List<AbstractDirectCommand>
                                      {
                                          new OnCommand(dataManager), 
                                          new OffCommand(dataManager), 
                                          new JsCommand(), 
                                          new RandomGuyCommand(), 
                                          new SubCommand(dataManager),
                                          new UnsubCommand(dataManager),
                                          new FeedbackCommand(dataManager)
                                      };

            this.UpdateData();

            var timer = new Timer(this.OnNotifyTimerElapsed);
            timer.Change(0, NotifyTimerPeriod);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get commands for chat.
        /// </summary>
        /// <param name="chat">
        /// The chat.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ISkypeCommand> GetCommandsForChat(SkypeChat chat)
        {
            List<ISkypeCommand> commands = new List<ISkypeCommand>(this.commonCommands);

            int projectId = this.dataManager.GetProjectForChat(chat.Id);
            if (projectId != 0)
            {
                commands.AddRange(this.projectsCommands.Where(c => c.ProjectId == projectId));
            }

            // For disabled chats eva accepts only system commands.
            // Usually they are on and off commands.
            commands = commands
                .Where(c => this.dataManager.IsChatEnabled(chat.Id) || c.CommandType == CommandType.System)
                .ToList();
            
            commands.Add(new HelpCommand(commands.OfType<AbstractDirectCommand>().ToList()));
            return commands;
        }

        /// <summary>
        /// The update data.
        /// </summary>
        public void UpdateData()
        {
            IEnumerable<Project> projects = this.dataManager.GetProjects();

            this.projectsCommands = new List<ISkypeCommand>();
            this.notifications = new List<AbstractNotification>();

            foreach (Project project in projects)
            {
                foreach (Resource resource in project.Resources)
                {
                    if (resource.Type == "jira")
                    {
                        List<string> issueTypes =
                            XElement.Parse(resource.AdditionalFields)
                                .Elements("issuetype")
                                .Select(element => element.Value)
                                .ToList();

                        this.projectsCommands.Add(new JiraCommand(resource.Url, resource.Username, resource.Password, issueTypes)
                                                      {
                                                          ProjectId = project.Id
                                                      });
                    }
                    else if (resource.Type == "cc")
                    {
                        XElement root = XElement.Parse(resource.AdditionalFields);
                        List<string> buildNames = root.Elements("build").Select(b => b.Value).ToList();
                        string targetServer = root.Element("targetserver").Value;

                        this.projectsCommands.Add(new CruiseControlCommand(resource.Url, targetServer, buildNames)
                                                      {
                                                          ProjectId = project.Id
                                                      });
                        this.notifications.Add(new CruiseControlNotification(resource.Url, targetServer, buildNames)
                                                       {
                                                           ProjectId = project.Id
                                                       });
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is constantly invoked to check whether some projects state changed
        /// and whether we need to notify about it.
        /// </summary>
        /// <param name="state">
        /// Timer's state.
        /// </param>
        private void OnNotifyTimerElapsed(object state)
        {
            if (this.notifications == null)
            {
                return;
            }

            foreach (AbstractNotification notification in this.notifications)
            {
                string message;
                if (notification.HasChanged(out message))
                {
                    if (notification.ProjectId.HasValue)
                    {
                        IEnumerable<string> chatNames = this.dataManager.GetChatsForProject(notification.ProjectId.Value);

                        foreach (var chatName in chatNames)
                        {
                            this.messageSender.SendMessageToChat(new SkypeChat { Id = chatName }, message);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
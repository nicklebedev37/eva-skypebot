namespace Skypebot.Core.Notifications
{
    using Skypebot.Core.Notifications.Abstract;

    /// <summary>
    /// The TeamCity builds notificator.
    /// This feature is quite raw and in the protorype stage.
    /// </summary>
    public class TeamcityNotification : AbstractNotification
    {
        private string state;

        public TeamcityNotification()
        {
            this.state = string.Empty;
        }

        public override bool HasChanged(out string message)
        {
            
        }
    }
}

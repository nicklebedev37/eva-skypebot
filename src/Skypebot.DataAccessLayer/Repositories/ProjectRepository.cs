namespace Skypebot.DataAccessLayer.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// The project repository.
    /// </summary>
    public class ProjectRepository : AbstractRepository<Project>
    {
        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public override List<Project> GetAll()
        {
            using (var dbcontext = new evadb())
            {
                return dbcontext.Projects
                    .Select(e => e)
                    .Include("Resources")
                    .Include("ChatSubscriptions")
                    .ToList();
            }
        }
    }
}

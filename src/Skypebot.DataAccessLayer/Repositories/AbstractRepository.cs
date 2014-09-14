namespace Skypebot.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Skypebot.DataAccessLayer.Entities;

    /// <summary>
    /// The abstract repository.
    /// </summary>
    /// <typeparam name="T">
    /// The database entity.
    /// </typeparam>
    public class AbstractRepository<T>
        where T : Entity
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public virtual T Create(T entity)
        {
            using (var dbcontext = new evadb())
            {
                T createdEntity = dbcontext.Set<T>().Add(entity);
                dbcontext.SaveChanges();
                return createdEntity;
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool Delete(T entity)
        {
            try
            {
                using (var dbcontext = new evadb())
                {
                    dbcontext.Set<T>().Attach(entity);
                    dbcontext.Set<T>().Remove(entity);
                    dbcontext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public virtual List<T> GetAll()
        {
            using (var dbcontext = new evadb())
            {
                return dbcontext.Set<T>().ToList();
            }
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public virtual T GetById(int id)
        {
            using (var dbcontext = new evadb())
            {
                return dbcontext.Set<T>().FirstOrDefault(p => p.Id == 1);
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool Update(T entity)
        {
            try
            {
                using (var dbcontext = new evadb())
                {
                    dbcontext.Set<T>().AddOrUpdate(entity);
                    dbcontext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
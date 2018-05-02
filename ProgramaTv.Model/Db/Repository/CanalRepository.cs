using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using ProgramaTv.Model.Db.Model;

namespace ProgramaTv.Model.Db.Repository
{
    public class CanalRepository : RepositoryBase<Canal>
    {
        public CanalRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }
    }
}

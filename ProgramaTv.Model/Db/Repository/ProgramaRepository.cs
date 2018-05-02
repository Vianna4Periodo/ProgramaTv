using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using ProgramaTv.Model.Db.Model;
using NHibernate.Linq;

namespace ProgramaTv.Model.Db.Repository
{
    public class ProgramaRepository : RepositoryBase<Programa>
    {
        public ProgramaRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

        public IList<Programa> ProgramasByCanal(int idCanal)
        {
            try
            {
                return this.Session.Query<Programa>().Where(x => x.Canal.Id == idCanal).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public IList<Programa> ProgramasByDate(DateTime date)
        {
            try
            {
                return this.Session.Query<Programa>().Where(x => x.DataHora == date).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }
    }
}

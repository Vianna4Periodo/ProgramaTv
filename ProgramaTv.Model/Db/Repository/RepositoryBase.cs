using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Context;
using NHibernate.Linq;

namespace ProgramaTv.Model.Db.Repository
{
    public class RepositoryBase<T> where T : class
    {
        private ISessionFactory _sessionFactory;

        protected ISession Session
        {
            get
            {
                try
                {
                    if (CurrentSessionContext.HasBind(_sessionFactory))
                    {
                        if (_sessionFactory.GetCurrentSession().IsOpen)
                        {
                            return _sessionFactory.GetCurrentSession();
                        }
                    }

                    var session = _sessionFactory.OpenSession();
                    session.FlushMode = FlushMode.Commit;

                    CurrentSessionContext.Bind(session);

                    return session;
                }
                catch (Exception ex)
                {
                    throw new Exception("Não foi possível criar a Sessão.", ex);
                }
            }
        }

        public RepositoryBase(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public IList<T> FindAll()
        {
            try
            {
                return Session.CreateCriteria(typeof(T)).List<T>();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public T FirstById(int id)
        {
            try
            {
                return Session.Get<T>(id);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public T FirstOrDefault()
        {
            try
            {
                return this.Session.Query<T>().FirstOrDefault();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public virtual T SaveOrUpdate(T entity)
        {
            try
            {
                var transacao = Session.BeginTransaction();

                Session.SaveOrUpdate(entity);

                transacao.Commit();

                return entity;

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível salvar " + typeof(T) + "\nErro:" + ex.Message);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public virtual T Save(T entity)
        {
            try
            {
                var transacao = Session.BeginTransaction();

                Session.Save(entity);

                transacao.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível salvar " + typeof(T) + "\nErro:" + ex.Message);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public virtual T Update(T entity)
        {
            try
            {
                var transacao = Session.BeginTransaction();

                Session.Update(entity);

                transacao.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível editar " + typeof(T) + "\nErro:" + ex.Message);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public void Delete(T entity)
        {
            try
            {
                var transacao = Session.BeginTransaction();

                Session.Delete(entity);

                transacao.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(T) + "\nErro:" + ex.Message);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public void DeleteAll(List<T> entity)
        {
            try
            {
                var transacao = Session.BeginTransaction();

                Session.Delete(entity);

                transacao.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(T) + "\nErro:" + ex.Message);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public void Clear()
        {
            Session?.Clear();
        }
    }
}

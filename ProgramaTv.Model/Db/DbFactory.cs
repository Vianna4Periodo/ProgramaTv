using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using NHibernate.Mapping.ByCode;
using ProgramaTv.Model.Db.Model;
using ProgramaTv.Model.Db.Repository;

namespace ProgramaTv.Model.Db
{
    public class DbFactory
    {
        private static DbFactory _instance = null;

        private ISessionFactory _sessionFactory;

        public ProgramaRepository ProgramaRepository { get; set; }
        public CanalRepository CanalRepository { get; set; }

        private DbFactory()
        {
            Conexao();

            ProgramaRepository = new ProgramaRepository(_sessionFactory);
            CanalRepository = new CanalRepository(_sessionFactory);
        }

        public static DbFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DbFactory();
                }

                return _instance;
            }
        }

        private void Conexao()
        {
            try
            {
                var server = "localhost";
                var port = "3306";
                var dbName = "db_guia_tv";
                var user = "root";
                var psw = "root";

                var stringConexao = "Persist Security Info=False;server=" + server + ";port=" + port + ";database=" +
                                    dbName + ";uid=" + user + ";pwd=" + psw;

                var mySql = new MySqlConnection(stringConexao);
                try
                {
                    mySql.Open();
                }
                catch
                {
                    CriarSchemaBanco(server, port, dbName, psw, user);
                }
                finally
                {
                    if (mySql.State == ConnectionState.Open)
                    {
                        mySql.Close();
                    }
                }

                ConfigurarNHibernate(stringConexao);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel conectar", ex);
            }
        }

        private void CriarSchemaBanco(string server, string port, string dbName, string psw, string user)
        {
            try
            {
                var stringConexao = "server=" + server + ";user=" + user + ";port=" + port + ";password=" + psw + ";";
                var mySql = new MySqlConnection(stringConexao);
                var cmd = mySql.CreateCommand();

                mySql.Open();
                cmd.CommandText = "CREATE DATABASE IF NOT EXISTS `" + dbName + "`;";
                cmd.ExecuteNonQuery();
                mySql.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi criar o banco de dados.", ex);
            }
        }

        private void ConfigurarNHibernate(String stringConexao)
        {
            try
            {
                var config = new Configuration();

                ConfigureLog4Net();

                //Configuração do NH com o MySQL
                config.DataBaseIntegration(i =>
                {
                    //Dialeto do Banco
                    i.Dialect<NHibernate.Dialect.MySQLDialect>();
                    //Conexao string
                    i.ConnectionString = stringConexao;
                    //Drive de conexão com o banco
                    i.Driver<NHibernate.Driver.MySqlDataDriver>();
                    // Provedor de conexão do MySQL 
                    i.ConnectionProvider<NHibernate.Connection.DriverConnectionProvider>();
                    // GERA LOG DOS SQL EXECUTADOS NO CONSOLE
                    i.LogSqlInConsole = true;
                    // DESCOMENTAR CASO QUEIRA VISUALIZAR O LOG DE SQL FORMATADO NO CONSOLE
                    i.LogFormattedSql = true;
                    // CRIA O SCHEMA DO BANCO DE DADOS SEMPRE QUE A CONFIGURATION FOR UTILIZADA
                    i.SchemaAction = SchemaAutoAction.Update;
                });

                //Realiza o mapeamento das classes
                var maps = this.Mapeamento();
                config.AddMapping(maps);

                //Verifico se a aplicação é Desktop ou Web
                if (HttpContext.Current == null)
                {
                    config.CurrentSessionContext<ThreadStaticSessionContext>();
                }
                else
                {
                    config.CurrentSessionContext<WebSessionContext>();
                }

                this._sessionFactory = config.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível configurar NH", ex);
            }
        }

        private HbmMapping Mapeamento()
        {
            try
            {
                var mapper = new ModelMapper();

                mapper.AddMappings(
                    Assembly.GetAssembly(typeof(CanalMap)).GetTypes()
                );

                return mapper.CompileMappingForAllExplicitlyAddedEntities();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível realizar o mapeamento do modelo.", ex);
            }
        }

        public static void ConfigureLog4Net()
        {
            //log4net.Config.XmlConfigurator.Configure();

            /***
            Exemplo de configuração do log para nhibernate no arquivo app.config
            -------------------------------------------------------------------
              <configSections>
                <section name="log4net"
                  type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
              </configSections>
              <log4net>
                <appender name="NHLog" type="log4net.Appender.RollingFileAppender, log4net" >
                  <param name="File" value="NHLog.txt" />
                  <param name="AppendToFile" value="true" />
                  <param name="maximumFileSize" value="200KB" />
                  <param name="maxSizeRollBackups" value="1" />
                  <layout type="log4net.Layout.PatternLayout, log4net">
                    <conversionPattern
                    value="%date{yyyy.MM.dd hh:mm:ss} %-5level [%thread] - %message%newline" />
                  </layout>
                </appender>
                <!-- levels: ALL, DEBUG, INFO, WARN, ERROR, FATAL, OFF -->
                <root>
                  <level value="INFO" />
                  <appender-ref ref="NHLog" />
                </root>
                <logger name="NHBase.SQL">
                  <level value="ALL" />
                  <appender-ref ref="NHLog" />
                </logger>
              </log4net>
            ***/
        }
    }
}

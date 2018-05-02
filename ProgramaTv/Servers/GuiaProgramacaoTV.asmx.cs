using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ProgramaTv.Model.Db;
using ProgramaTv.Model.Db.Model;

namespace ProgramaTv.Servers
{
    /// <summary>
    /// Summary description for GuiaProgramacaoTV
    /// </summary>
    [WebService(Namespace = "http://guiaTv.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GuiaProgramacaoTV : System.Web.Services.WebService
    {


        [WebMethod]
        public Canal CadastrarCanal(Canal canal)
        {
            return DbFactory.Instance.CanalRepository.Save(canal);
        }

        [WebMethod]
        public Programa CadastrarPrograma(Programa programa, int IdCanal)
        {
            var canal = DbFactory.Instance.CanalRepository.FirstById(IdCanal);
            if (canal == null)
            {
                throw new Exception("Canal não encontrado!");
            }

            programa.Canal = canal;

            return DbFactory.Instance.ProgramaRepository.Save(programa);
        }

        [WebMethod]
        public List<Canal> TodosCanais()
        {
            return DbFactory.Instance.CanalRepository.FindAll().ToList();
        }

        [WebMethod]
        public List<Programa> TodaProgramacaoIdCanal(int IdCanal)
        {
            var programas = DbFactory.Instance.ProgramaRepository.FindAll().ToList();
            programas = programas.Where(w => w.Canal.Id == IdCanal).ToList();

            return programas;
        }

        [WebMethod]
        public List<Programa> TodaProgramacaoPorData(DateTime data)
        {
            var programas = DbFactory.Instance.ProgramaRepository.FindAll().ToList();
            programas = programas.Where(w => w.DataHora.Date == data.Date).ToList();

            return programas;
        }

        [WebMethod]
        public List<Programa> TodaProgramacaoIdCanalData(int IdCanal, DateTime data)
        {
            var programas = DbFactory.Instance.ProgramaRepository.FindAll().ToList();
            programas = programas.Where(w => w.Canal.Id == IdCanal &&
                                             w.DataHora.Date == data.Date).ToList();

            return programas;
        }

        [WebMethod]
        public void RemoverCanal(int IdCanal)
        {
            var canal = DbFactory.Instance.CanalRepository.FirstById(IdCanal);
            if (canal == null)
            {
                throw new Exception("Canal não encontrado!");
            }

            DbFactory.Instance.CanalRepository.Delete(canal);
        }

        [WebMethod]
        public void RemoverPrograma(int IdPrograma)
        {
            var programa = DbFactory.Instance.ProgramaRepository.FirstById(IdPrograma);
            if (programa == null)
            {
                throw new Exception("Progarama não encontrado!");
            }

            DbFactory.Instance.ProgramaRepository.Delete(programa);
        }
    }
}

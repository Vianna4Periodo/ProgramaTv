using ProgramaTv.Model.Db;
using ProgramaTv.Model.Db.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProgramaTv.Rest.Controllers
{
    [RoutePrefix("programaTv")]
    public class ValuesController : ApiController
    {
        public IEnumerable<Canal> Get()
        {
            var canais = DbFactory.Instance.CanalRepository.FindAll();

            return canais;

        }

        public Canal Get(int id)
        {
            var canal = DbFactory.Instance.CanalRepository.FirstById(id);
            return canal;
        }

        [Route("api/buscarcanal/{id}")]
        [HttpPost]
        public Canal BuscarCanal(int id)
        {
            var canal = DbFactory.Instance.CanalRepository.FirstById(id);
            return canal;
        }

        public void Post([FromBody]Canal value)
        {
            DbFactory.Instance.CanalRepository.SaveOrUpdate(value);
        }

        public void Put(int id, [FromBody]Canal value)
        {
            var canal = DbFactory.Instance.CanalRepository.FirstById(id);
            if (canal != null)
            {
                canal.Nome = value.Nome;
                DbFactory.Instance.CanalRepository.SaveOrUpdate(canal);
            }
        }

        public void Delete(int id)
        {
            var canal = DbFactory.Instance.CanalRepository.FirstById(id);
            if (canal != null)
                DbFactory.Instance.CanalRepository.Delete(canal);
        }

        // -----
        // Programa API
        // -----

        [Route("api/programas")]
        [HttpGet]
        public IEnumerable<Programa> GetProgramas()
        {
            return DbFactory.Instance.ProgramaRepository.FindAll();
        }

        [Route("api/programas/{id}")]
        [HttpGet]
        public Programa GetPrograma(int id)
        {
            return DbFactory.Instance.ProgramaRepository.FirstById(id);
        }

        [Route("api/programas/canal/{id}")]
        [HttpGet]
        public IList<Programa> GetProgramaPorCanal(int id)
        {
            return DbFactory.Instance.ProgramaRepository.ProgramasByCanal(id);
        }

        [Route("api/programas/date/{date}")]
        [HttpGet]
        public IList<Programa> GetProgramaPorData(DateTime date)
        {
            return DbFactory.Instance.ProgramaRepository.ProgramasByDate(date);
        }

        [Route("api/programas")]
        [HttpPost]
        public Programa SalvarPrograma([FromBody]Programa value)
        {
            return DbFactory.Instance.ProgramaRepository.SaveOrUpdate(value);
        }

        [Route("api/programas/editar/{id}")]
        [HttpPut]
        public void EditarPrograma(int id, [FromBody]Programa value)
        {
            var programa = DbFactory.Instance.ProgramaRepository.FirstById(id);
            if (programa != null)
            {
                programa.Titulo = value.Titulo;
                programa.Descricao = value.Descricao;
                programa.DataHora = value.DataHora;

                DbFactory.Instance.ProgramaRepository.SaveOrUpdate(programa);
            }
        }

        [Route("api/programas/apagar/{id}")]
        [HttpDelete]
        public void ApagarPrograma(int id)
        {
            var programa = DbFactory.Instance.ProgramaRepository.FirstById(id);
            if (programa != null)
                DbFactory.Instance.ProgramaRepository.Delete(programa);
        }

    }
}

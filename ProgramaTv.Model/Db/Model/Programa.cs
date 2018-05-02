using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ProgramaTv.Model.Db.Model
{
    public class Programa
    {
        public virtual int Id { get; set; }
        public virtual String Titulo { get; set; }
        public virtual String Descricao { get; set; }
        public virtual DateTime DataHora { get; set; }
        [XmlIgnore]
        public virtual Canal Canal { get; set; }
    }

    public class ProgramaMap : ClassMapping<Programa>
    {
        public ProgramaMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));

            Property(x => x.Titulo);
            Property(x => x.Descricao);
            Property(x => x.DataHora);

            ManyToOne(x => x.Canal, m =>
            {
                m.Lazy(LazyRelation.NoLazy);
                m.Column("idCanal");
            });
        }
    }
}

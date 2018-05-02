using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ProgramaTv.Model.Db.Model
{
    public class Canal
    {
        public virtual int Id { get; set; }
        public virtual String Nome { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public virtual IList<Programa> Programacao { get; set; }

        public Canal()
        {
            Programacao = new List<Programa>();
        }
    }

    public class CanalMap : ClassMapping<Canal>
    {
        public CanalMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));

            Property(x => x.Nome);

            Bag(x => x.Programacao, m =>
            {
                m.Inverse(true);
                m.Cascade(Cascade.DeleteOrphans);
                m.Lazy(CollectionLazy.Lazy);
                m.Key(k => k.Column("idCanal"));
            }, r => r.OneToMany());
        }
    }
}

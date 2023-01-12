using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Business
{
    public class produto : Base
    {
        [Opcoesbase(UsaBD = true, UsaBusca = true, ChavePrimaria = true)]
        public int? Id { get; set; }

        [Opcoesbase(UsaBD = true, UsaBusca = true)]
        public string descricao { get; set; }

        [Opcoesbase(UsaBD = true, UsaBusca = true)]
        public string preco { get; set; }

        [Opcoesbase(UsaBD = true, UsaBusca = true)]
        public string qtdd { get; set; }

        public new List<produto> Todos()
        {
            List<produto> pro = new List<produto>();
            foreach (var ibase in base.Todos())
            {
                pro.Add((produto)ibase);
            }
            return pro;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Business
{
    public class comissao : Base
    {
        [Opcoesbase(UsaBD = true, UsaBusca = true, ChavePrimaria = true)]
        public int? Id { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string mecanico { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string valor { get; set; }

        public new List<comissao> Todos()
        {
            List<comissao> com = new List<comissao>();
            foreach (var ibase in base.Todos())
            {
                com.Add((comissao)ibase);
            }
            return com;
        }
    }
}

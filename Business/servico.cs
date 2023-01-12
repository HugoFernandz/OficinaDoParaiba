using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using System.Data.SqlClient;

namespace Business
{
    public class servico : Base
    {
        [Opcoesbase(UsaBD = true, UsaBusca = true, ChavePrimaria = true)]
        public int? Id { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string placa { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string maodeobra { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string mecanico { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string descricao { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string total { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string status { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string data { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string ordem { get; set; }

        public new List<servico> Todos()
        {
            List<servico> com = new List<servico>();
            foreach (var ibase in base.Todos())
            {
                com.Add((servico)ibase);
            }
            return com;
        }
    }
}
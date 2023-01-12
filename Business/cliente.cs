using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Business
{
    public class cliente : Base
    {
        [Opcoesbase(UsaBD = true, UsaBusca = true, ChavePrimaria = true)]
        public int? Id { get; set; }

        [Opcoesbase(UsaBD = true, UsaBusca = true)]
        public string nome { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string cpf { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string rg { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string endereco { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string celular { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string bairro { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string cidade { get; set; }

        public new List<cliente> Todos()
        {
            List<cliente> cli = new List<cliente>();
            foreach (var ibase in base.Todos())
            {
                cli.Add((cliente)ibase);
            }
            return cli;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Business
{
    public class veiculo : Base
    {
        [Opcoesbase(UsaBD = true, UsaBusca = true, ChavePrimaria = true)]
        public int? Id { get; set; }

        [Opcoesbase(UsaBD = true, UsaBusca = true)]
        public string placa { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string marca { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string quilometragem { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string ano { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string modelo { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string cor { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string cambio { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string motor { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string dono { get; set; }

        public new List<veiculo> Todos()
        {
            List<veiculo> vei = new List<veiculo>();
            foreach (var ibase in base.Todos())
            {
                vei.Add((veiculo)ibase);
            }
            return vei;
        }
    }
}

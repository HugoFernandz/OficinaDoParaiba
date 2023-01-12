using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Opcoesbase : Attribute
    {
        public bool UsaBD { get; set; }
        public bool UsaBusca { get; set; }
        public bool UsaAtualizar { get; set; }
        public bool UsaExcluir { get; set; }
        public bool ChavePrimaria { get; set; }
    }
}

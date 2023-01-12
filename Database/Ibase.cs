using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public interface Ibase
    {                  
        int Key { get; }
        void Salvar();
        List<Ibase> Excluir();
        List<Ibase> Busca();
        List<Ibase> Todos();
    }
}

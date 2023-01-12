using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Business
{
    public class ordems : Base
    {
        [Opcoesbase(UsaBD = true, UsaBusca = true, ChavePrimaria = true)]
        public int? Id { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string login { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string senha { get; set; }

        [Opcoesbase(UsaBD = true, UsaBusca = true)]
        public string ordem { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string produto { get; set; }

        [Opcoesbase(UsaBD = true)]
        public string quantidade { get; set; }

        public new List<ordems> Todos()
        {
            List<ordems> pro = new List<ordems>();
            foreach (var ibase in base.Todos())
            {
                pro.Add((ordems)ibase);
            }
            return pro;
        }
        //public new List<ordems> BusPro()
        //{
        //    List<ordems> pro = new List<ordems>();
        //    foreach (var ibase in base.BuscaProd())
        //    {
        //        pro.Add((ordems)ibase);
        //    }
        //    return pro;
        //}
    }
}


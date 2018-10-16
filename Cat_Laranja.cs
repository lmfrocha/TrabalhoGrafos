using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_2._0
{
    public class Cat_Laranja : Categoria
    {
        public Cat_Laranja()
            : base()
        {
            Id = "Laranja";
            Bonificacao = 1; //bonus multiplicador em pontos de novos bilhetes
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_2._0
{
    public class Cat_Abacate : Categoria
    {
        public Cat_Abacate()
            : base()
        {
            Id = "Abacate";
            Bonificacao = 1.2; //bonus multiplicador em pontos de novos bilhetes
        }
    }
}

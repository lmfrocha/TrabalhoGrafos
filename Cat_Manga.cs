using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_2._0
{
    public class Cat_Manga : Categoria
    {
        public Cat_Manga()
            : base()
        {
            Id = "Manga";
            Bonificacao = 1.5; //bonus multiplicador em pontos de novos bilhetes
        }
    }
}

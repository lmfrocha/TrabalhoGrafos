using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_2._0
{
    public abstract class Categoria
    {
        private string id;
        private double bonificacao;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public double Bonificacao
        {
            get
            {
                return bonificacao;
            }

            set
            {
                bonificacao = value;
            }
        }
    }
}

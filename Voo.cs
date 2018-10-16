using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_2._0
{
    public abstract class Voo
    {
        private string id;
        private int codigo;
        private Data dataVoo;
        private string cidOrigem;
        private string cidDestino;
        private int pontos;

        public string CidDestino
        {
            get { return cidDestino; }
            set { cidDestino = value; }
        }

        public string CidOrigem
        {
            get { return cidOrigem; }
            set { cidOrigem = value; }
        }

        public Data DataVoo
        {
            get { return dataVoo; }
            set { dataVoo = value; }
        }

        public int Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public int Pontos
        {
            get
            {
                return pontos;
            }

            set
            {
                pontos = value;
            }
        }
    }
}

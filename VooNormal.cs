using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_2._0
{
    public class VooNormal: Voo
    {
        /// <summary>
        /// Contrutor da classe VooNormal
        /// </summary>
        /// <param name="cod"></param>
        /// <param name="cO"></param>
        /// <param name="cD"></param>
        /// <param name="dt"></param>
        public VooNormal(int cod, string cO, string cD, Data dt)
        {
            this.Id = "Normal";
            this.Codigo = cod;
            this.CidOrigem = cO;
            this.CidDestino = cD;
            this.DataVoo = dt;
            this.Pontos = 1000;
        }

    }
}

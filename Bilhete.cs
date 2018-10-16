using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_2._0
{
    public class Bilhete
    {
        private double codCliente;
        private double codVoo;
        private double pontosGerados;
        private Data dt;

        public double CodCliente
        {
            get
            {
                return codCliente;
            }

            set
            {
                codCliente = value;
            }
        }

        public double CodVoo
        {
            get
            {
                return codVoo;
            }

            set
            {
                codVoo = value;
            }
        }

        public double PontosGerados
        {
            get
            {
                return pontosGerados;
            }

            set
            {
                pontosGerados = value;
            }
        }

        public Data Dt
        {
            get
            {
                return dt;
            }

            set
            {
                dt = value;
            }
        }

        /// <summary>
        /// Construtor da classe Bilhete
        /// </summary>
        /// <param name="cC"></param>
        /// <param name="cV"></param>
        /// <param name="pts"></param>
        /// <param name="dta"></param>
        public Bilhete(double cC, double cV, double pts, Data dta)
        {
            this.CodCliente = cC;
            this.CodVoo = cV;
            this.pontosGerados = pts;
            this.dt = dta;
        }

        /// <summary>
        /// Construtor da classe Bilhete
        /// </summary>
        /// <param name="cC"></param>
        /// <param name="cV"></param>
        /// <param name="pts"></param>
        /// <param name="dta"></param>
        public Bilhete(double cC, double cV)
        {
            this.CodCliente = cC;
            this.CodVoo = cV;
        }
    }
}

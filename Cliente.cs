using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_2._0
{
    public class Cliente
    {
        private double cod;
        private string nome;
        private string cidadeOrigem;
        private double pontosAcumulados; //total de pontos acumulados por bilhetes emitidos
        private double pontos12; //total de pontos acumulados nos últimos 365 dias
        private Categoria categ;
        private int bilComPontos;

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        public string CidadeOrigem
        {
            get { return cidadeOrigem; }
            set { cidadeOrigem = value; }
        }

        public double PontosAcumulados
        {
            get { return pontosAcumulados; }
            set { pontosAcumulados = value; }
        }

        public double Pontos12
        {
            get { return pontos12; }
            set { pontos12 = value; }
        }

        public Categoria Categ
        {
            get { return categ; }
            set { categ = value; }
        }

        public double Cod
        {
            get
            {
                return cod;
            }

            set
            {
                cod = value;
            }
        }

        public int BilComPontos
        {
            get
            {
                return bilComPontos;
            }

            set
            {
                bilComPontos = value;
            }
        }

        /// <summary>
        /// Construtor da classe Cliente
        /// </summary>
        /// <param name="_cod"></param>
        /// <param name="_nome"></param>
        /// <param name="_cidadeOrigem"></param>
        public Cliente(double _cod, string _nome, string _cidadeOrigem)
        {
            this.Cod = _cod;
            this.nome = _nome;
            this.cidadeOrigem = _cidadeOrigem;

            this.pontosAcumulados = 0;
            this.pontos12 = 0;
            this.bilComPontos = 0;

            this.categ = new Cat_Laranja();
        }

        /// <summary>
        /// Incrementa pontos a cada novo bilhete para o cliente
        /// </summary>
        /// <param name="pts"></param>
        public void addPontos (double pts)
        {
            this.PontosAcumulados += pts;
        }

        /// <summary>
        /// Decrementa pontos a cada novo bilhete emitido com utilização de pontos
        /// </summary>
        /// <param name="pts"></param>
        public void subPontos(double pts)
        {
            this.PontosAcumulados -= pts;
        }

        /// <summary>
        /// Incrementa pontos caso o bilhete tenha sido adquirido nos últimos 365 dias
        /// </summary>
        /// <param name="pts"></param>
        public void addPontos12(double pts)
        {
            this.pontos12 += pts;
            setCategoria();
        }

        /// <summary>
        /// Set para o atributo Categoria
        /// Se altera de acordo com a quantidade de pontos acumulados nos últimos 365 dias
        /// </summary>
        public void setCategoria()
        {
            if (this.pontos12 < 10000) this.categ = new Cat_Laranja();
            else if (this.pontos12 < 15000) this.categ = new Cat_Abacate();
            else this.categ = new Cat_Manga();
        }

        /// <summary>
        /// Verifica se a diferença entre datas atual e do bilhete é maior ou menor que 365 dias
        /// Incrementa os pontos adquiridos pelo bilhete para o cliente caso seja menor que 365 dias
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pontos"></param>
        /// <returns></returns>
        public void verifPontos12(Data data, double pontos)
        {
            int dias = 0;
            string now = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            Data atual = new Data(now);
            double result = 0;

            dias = atual.QuantosDias(data);

            if (dias < 366)
            {
                result += pontos;
            }

            this.addPontos12(result);
        }
    }
}

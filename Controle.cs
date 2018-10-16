using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_2._0
{
    // Classe utilizada para controle do programa
    public class Controle 
    {
        private Cliente[] clientes;
        private Voo[] voos;
        private Cidade[] cidades;
        private Bilhete[] bilhetes;

        public Voo[] Voos
        {
            get
            {
                return voos;
            }

            set
            {
                voos = value;
            }
        }

        public Cliente[] Clientes
        {
            get
            {
                return clientes;
            }

            set
            {
                clientes = value;
            }
        }

        public Cidade[] Cidades
        {
            get
            {
                return cidades;
            }

            set
            {
                cidades = value;
            }
        }

        public Bilhete[] Bilhetes
        {
            get
            {
                return bilhetes;
            }

            set
            {
                bilhetes = value;
            }
        }

        /// <summary>
        /// Construtor da classe Controle
        /// </summary>
        public Controle()
        {
            this.Clientes = new Cliente[100000];
            this.Voos = new Voo[100000];
            this.Cidades = new Cidade[10000];
            this.Bilhetes = new Bilhete[100000];
        }
    }
}

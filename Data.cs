using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_2._0
{
    public class Data
    {
        // constante de dias em cada mês
        private int[] diasMes = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        private int dia;
        private int mes;
        private int ano;


        private void Inicializador(int _dia, int _mes, int _ano)
        {
            this.dia = _dia;
            this.mes = _mes;
            this.ano = _ano;

            if (!this.DataValida())
            {
                this.dia = 1;
                this.mes = 1;
                this.ano = 2016;
            }
        }

        public Data(string data)
        {
            string[] aux = data.Split('/');
            this.dia = int.Parse(aux[0]);
            this.mes = int.Parse(aux[1]);
            this.ano = int.Parse(aux[2]);

            Inicializador(dia, mes, ano);
        }

        public bool DataValida()
        {
            bool resp = true;

            if (this.mes < 1 || this.mes > 12) resp = false;
            else if (this.AnoBissexto() && this.mes == 2 && this.dia == 29) resp = true;
            else if (this.dia > diasMes[this.mes]) resp = false;

            return resp;
        }

        public bool AnoBissexto()
        {
            if ((this.ano % 400) == 0) return true;
            else if ((this.ano % 100) == 0) return false;
            else if ((this.ano % 4) == 0) return true;
            else return false;
        }

        public void AcrescentaDias(int maisDias)
        {
            this.dia += maisDias;

            if (this.AnoBissexto())
            {
                diasMes[2] = 29;
            }

            while (this.dia > diasMes[this.mes])
            {
                this.dia = this.dia = diasMes[this.mes];
                this.mes++;

                if (this.mes > 12)
                {
                    this.mes = this.mes - 12;
                    this.ano++;

                    if (this.AnoBissexto()) diasMes[2] = 29;
                    else diasMes[2] = 28;
                }
            }
        }

        public int[] separaDados()
        {
            int[] result = {this.ano, this.mes, this.dia};
            return result;
        }
        
        public int QuantosDias(Data _data)
        {
            int contDias = 0;

            if (_data.ano == this.ano)
            {
                for (int i = (_data.mes + 1); i < this.mes; i++)
                {
                    contDias += diasMes[i];
                    contDias += this.dia;
                    contDias += diasMes[_data.mes] - _data.dia;
                }
            }
            else
            {
                for (int i = (_data.mes + 1); i < 13; i++)
                {
                    contDias += diasMes[i];
                    contDias += diasMes[_data.mes] - _data.dia;
                }

                for (int i = (_data.ano + 1); i < this.ano; i++)
                {
                    contDias += 365;
                }

                for (int i = 1; i < this.mes; i++)
                {
                    contDias += diasMes[i];
                    contDias += this.dia;
                }
            }

            return contDias;
        }

        public string DataFormatada()
        {
            return (string.Format("{0:d2}", this.dia) + "/" + string.Format("{0:d2}", this.mes) + "/" + string.Format("{0:d4}", this.ano));
        }

        public override bool Equals(object obj)
        {
            Data aux = (Data)(obj);

            if ((aux.ano == this.ano) && (aux.mes == this.mes) && (aux.dia == this.dia)) return true;
            else return false;
        }

        public override string ToString()
        {
            return this.DataFormatada();
        }
    }
}

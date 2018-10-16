using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_2._0
{
    public class Cidade
    {
        private string id;

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

        public Cidade(string _id)
        {
            this.Id = _id; //nome da cidade
        }
    }
}

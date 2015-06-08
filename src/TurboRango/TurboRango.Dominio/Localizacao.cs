using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboRango.Dominio
{
    public class Localizacao : Entidade
    {
        public string Bairro { get; set; }
        public double Latitude { get; set; }
        public string Logradouro { get; set; }
        public double Longitude { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboRango.Dominio
{
    public class Restaurante
    {
        internal string Nome { get; set; }
        internal int Capacidade { get; set; }
        internal Categoria Categoria { get; set; }
        internal Localizacao Localizacao { get; set; }
        internal Contato Contato { get; set; }
    }
}

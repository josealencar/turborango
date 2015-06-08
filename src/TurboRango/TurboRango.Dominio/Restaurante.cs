using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboRango.Dominio
{
    public class Restaurante : Entidade
    {
        public string Nome { get; set; }
        public int? Capacidade { get; set; }
        public Categoria Categoria { get; set; }
        public virtual Localizacao Localizacao { get; set; }
        public virtual Contato Contato { get; set; }
    }
}

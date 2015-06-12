using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboRango.Dominio
{
    public class FeedBack : Entidade
    {
        public int IdRestaurante { get; set; }
        public double Nota { get; set; }
        public DateTime? DataFeedBack { get; set; }
        public string Comentario { get; set; }
        public string Usuario { get; set; }
    }
}

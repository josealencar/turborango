using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboRango.Dominio;

namespace TurboRango.ImportadorXML
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Exemplo
            //Restaurante restaurante = new Restaurante();

            //restaurante.Nome = string.Empty;

            //Console.WriteLine(!string.IsNullOrEmpty(restaurante.Nome) ? "Tem valor" : "Não tem valor");
            #endregion

            #region Importação XML
            const string nomeArquivo = "restaurantes.xml";

            var restaurantesXML = new RestaurantesXML(nomeArquivo);

            var nomes = restaurantesXML.ObterNomes();

            var nomesAsc = restaurantesXML.OrdenarPorNomeAsc();

            var sites = restaurantesXML.ObterSites();

            Console.ReadKey();
            #endregion
        }
    }
}

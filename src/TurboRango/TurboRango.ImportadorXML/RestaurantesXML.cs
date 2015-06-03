using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TurboRango.Dominio;

namespace TurboRango.ImportadorXML
{
    public class RestaurantesXML
    {
        public string NomeArquivo { get; private set; }

        IEnumerable<XElement> restaurantes;

        /// <summary>
        /// Constrói RestaurantesXML a partir do nome do arquivo
        /// </summary>
        /// <param name="nomeArquivo">Nome do Arquivo XML</param>
        public RestaurantesXML(string nomeArquivo)
        {
            this.NomeArquivo = nomeArquivo;
            restaurantes = XDocument.Load(NomeArquivo).Descendants("restaurante");
        }

        public IList<string> ObterNomes()
        {
            #region modo hard
            //var resultado = new List<string>();

            //var nodos = XDocument.Load(NomeArquivo).Descendants("restaurante");

            //foreach (var item in nodos)
            //{
            //    resultado.Add(item.Attribute("nome").Value);
            //}

            //return resultado;
            #endregion

            #region modo query
            //return (
            //    from n in XDocument.Load(NomeArquivo).Descendants("restaurante")
            //    orderby n.Attribute("nome").Value
            //    select n.Attribute("nome").Value
            //    ).ToList();
            #endregion

            return restaurantes
                .Select(n => n.Attribute("nome").Value).OrderBy(n => n).ToList();
        }

        public IList<string> OrdenarPorNomeAsc()
        {
            return restaurantes
                .Select(n => n.Attribute("nome").Value).OrderBy(n => n).ToList();
        }

        public IList<string> ObterSites()
        {
            #region Query
            return (
                from n in restaurantes.Descendants("contato").Descendants("site")
                where n.Value != null
                select n.Value
                ).ToList();
            #endregion
        }

        public double CapacidadeMedia()//Já feito em aula
        {
            return (from n in XDocument.Load(NomeArquivo).Descendants("restaurante")
                        select Convert.ToInt32(n.Attribute("capacidade").Value))
                        .Average();
        }

        public object AgruparPorCategoria()
        {
            var res = from n in restaurantes
                      group n by n.Attribute("categoria").Value into g
                      where g != null
                      select new { 
                          Categoria = g.Key,
                          Restaurantes = g.ToList(),
                          SomatorioCapacidades = g.Sum(x => Convert.ToInt32(x.Attribute("capacidade").Value))
                      };

            return res;
        }

        public IList<string> ApenasComUmRestaurante()
        {
            var res = from n in restaurantes
                      group n by n.Attribute("categoria").Value into g
                      where g != null && g.Count() > 1
                      select new { 
                          Categoria = g.Key
                      };

            //IList<Categoria> resultado = null;

            //foreach (var i in res)
            //{
            //    Categoria convertida = (Categoria) Enum.Parse(typeof(Categoria), i);
            //    resultado.Add(convertida);
            //}

            return res.Select(r => r.Categoria).ToList();
        }

        public double CapacidadeMaxima()
        {
            return (from n in restaurantes
                    select Convert.ToInt32(n.Attribute("capacidade").Value))
                        .Max();
        }
    }
}

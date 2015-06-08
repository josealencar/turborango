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

        public IList<Categoria> ApenasComUmRestaurante()
        {
            var res = from n in restaurantes
                      group n by n.Attribute("categoria").Value into g
                      where g != null && g.Count() == 1
                      select (Categoria)Enum.Parse(typeof(Categoria), g.Key, ignoreCase: true);

            return res.ToList();
        }

        public IList<Categoria> ApenasMaisPopulares()
        {
            var res = from n in restaurantes
                      group n by n.Attribute("categoria").Value into g
                      where g != null
                      orderby g.Count() descending
                      select (Categoria)Enum.Parse(typeof(Categoria), g.Key, ignoreCase: true);
            return res.Take(2).ToList();
        }

        public IList<string> BairrosComMenosPizzarias()
        {
            var res = from n in restaurantes
                      orderby n.Element("localizacao").Element("bairro").Value
                      where n.Element("localizacao").Element("bairro") != null
                      select n.Element("");

            //return res.Select(r => r.ToString()).OrderBy(r => r).Take(8).ToList();
            return null;
        }

        public object AgrupadosPorBairroPercentual()
        {
            return null;
        }

        public double CapacidadeMaxima()
        {
            return (from n in restaurantes
                    select Convert.ToInt32(n.Attribute("capacidade").Value))
                        .Max();
        }

        //Exercício 2A
        public IEnumerable<Restaurante> TodosRestaurantes()
        {
            var res = from n in restaurantes
                      let loc = n.Element("localizacao")
                      let cont = n.Element("contato")
                      let site = cont != null && cont.Element("site") != null ? cont.Element("site").Value : null
                      let telef = cont != null && cont.Element("telefone") != null ? cont.Element("telefone").Value : null
                      where n != null
                      select new Restaurante
                      {
                          Nome = n.Attribute("nome").Value,
                          Categoria = (Categoria) Enum.Parse(typeof(Categoria), n.Attribute("categoria").Value, ignoreCase: true),
                          Capacidade = int.Parse(n.Attribute("capacidade").Value),
                          Localizacao = new Localizacao 
                          {
                              Bairro = loc.Element("bairro").Value,
                              Latitude = Convert.ToDouble(loc.Element("latitude").Value),
                              Longitude = Convert.ToDouble(loc.Element("longitude").Value),
                              Logradouro = loc.Element("logradouro").Value
                          },
                          Contato = new Contato
                          {
                              Site = site,
                              Telefone = telef
                          }
                      };
            return res;
        }
    }
}

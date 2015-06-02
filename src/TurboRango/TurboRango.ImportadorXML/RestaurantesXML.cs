using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TurboRango.ImportadorXML
{
    public class RestaurantesXML
    {
        public string NomeArquivo { get; private set; }


        /// <summary>
        /// Constrói RestaurantesXML a partir do nome do arquivo
        /// </summary>
        /// <param name="nomeArquivo">Nome do Arquivo XML</param>
        public RestaurantesXML(string nomeArquivo)
        {
            this.NomeArquivo = nomeArquivo;
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

            return XDocument.Load(NomeArquivo).Descendants("restaurante")
                .Select(n => n.Attribute("nome").Value).OrderBy(n => n).ToList();
        }

        public double CapacidadeMedia()
        {
            return (from n in XDocument.Load(NomeArquivo).Descendants("restaurante")
                        select Convert.ToInt32(n.Attribute("capacidade").Value))
                        .Average();
        }

        public double CapacidadeMaxima()
        {
            return (from n in XDocument.Load(NomeArquivo).Descendants("restaurante")
                    select Convert.ToInt32(n.Attribute("capacidade").Value))
                        .Max();
        }
    }
}

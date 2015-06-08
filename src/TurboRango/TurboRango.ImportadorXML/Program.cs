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

            var agrupadoPorCategoria = restaurantesXML.AgruparPorCategoria();

            var oportunidade = restaurantesXML.ApenasComUmRestaurante();

            var populares = restaurantesXML.ApenasMaisPopulares();

            var bairros = restaurantesXML.BairrosComMenosPizzarias();

            var criaRestaurantes = restaurantesXML.TodosRestaurantes();

            //Console.ReadKey();
            #endregion

            #region ADO.NET
            var connString = @"Data Source=.;Initial Catalog=TurboRango_DEV;UID=sa;PWD=sqlserver";
            //Integrated Security=true -- Quem usa autenticação do windows

            var acessoAoBanco = new CarinhaQueManipulaOBanco(connString);

            #region testeInserirContato
            //acessoAoBanco.InserirContato(new Contato
            //{
            //    Site = "www.dogao.gif",
            //    Telefone = "555555555"
            //});
            #endregion

            IEnumerable<Contato> contatos = acessoAoBanco.GetContatos();

            #region testeInserirLocalizacao
            //acessoAoBanco.InserirLocalizacao(new Localizacao
            //{
            //    Bairro = "teste",
            //    Logradouro = "teste",
            //    Latitude = 14.25,
            //    Longitude = 50.1458
            //});
            #endregion

            IEnumerable<Localizacao> localizacoes = acessoAoBanco.GetLocalizacoes();

            #region testeInserirRestaurante
            //acessoAoBanco.InserirRestaurante(new Restaurante
            //{
            //    Nome = "TesteRestaurante",
            //    Capacidade = 10,
            //    Categoria = Categoria.Comum,
            //    Localizacao = new Localizacao
            //    {
            //        Bairro = "qualquer",
            //        Logradouro = "qualquer",
            //        Latitude = 441.000,
            //        Longitude = 124.865
            //    },
            //    Contato = new Contato
            //    {
            //        Site = "www.testando.com",
            //        Telefone = "555471"
            //    }
            //});
            #endregion

            foreach (var r in criaRestaurantes)
            {
                acessoAoBanco.InserirRestaurante(r);
            }

            acessoAoBanco.Remover(50);



            Console.ReadKey();
            #endregion
        }
    }
}

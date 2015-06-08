using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboRango.Dominio;

namespace TurboRango.ImportadorXML
{
    public class CarinhaQueManipulaOBanco
    {
        private string connectionString;

        public CarinhaQueManipulaOBanco(string connectionString)
        {
            this.connectionString = connectionString;
        }

        internal int? InserirContato(Contato contato)
        {
            if (contato.Site == null && contato.Telefone == null) return null;
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "INSERT INTO [dbo].[Contato]([Site], [Telefone]) VALUES (@Site, @Telefone); SELECT @@IDENTITY";
                using (SqlCommand inserirContato = new SqlCommand(comandSQL, conn))
                {
                    inserirContato.Parameters.Add("@Site", SqlDbType.VarChar).Value = contato.Site != null ? contato.Site : (object)DBNull.Value;
                    inserirContato.Parameters.Add("@Telefone", SqlDbType.VarChar).Value = contato.Telefone != null ? contato.Telefone : (object)DBNull.Value;
                    conn.Open();
                    int? resultado = Convert.ToInt32(inserirContato.ExecuteScalar());
                    return resultado;
                }
            }
        }

        internal IEnumerable<Contato> GetContatos()
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "SELECT Site, Telefone FROM [dbo].[Contato](nolock)";
                using (SqlCommand selecionarContatos = new SqlCommand(comandSQL, conn))
                {
                    conn.Open();
                    using (SqlDataReader data = selecionarContatos.ExecuteReader())
                    {
                        List<Contato> contatos = new List<Contato>();
                        while (data.Read())
                        {
                            string site = data.IsDBNull(0) ? string.Empty : data.GetString(0);
                            string telefone = data.IsDBNull(1) ? string.Empty : data.GetString(1);
                            Contato novo = new Contato
                            {
                                Site = site,
                                Telefone = telefone
                            };
                            contatos.Add(novo);
                        }
                        return contatos;
                    }
                }
            }
        }

        internal int InserirLocalizacao(Localizacao localizacao)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "INSERT INTO [dbo].[Localizacao]([Bairro], [Logradouro], [Latitude], [Longitude]) VALUES (@Bairro, @Logradouro, @Latitude, @Longitude); SELECT @@IDENTITY";
                using (SqlCommand inserirLocalizacao = new SqlCommand(comandSQL, conn))
                {
                    inserirLocalizacao.Parameters.Add("@Bairro", SqlDbType.VarChar).Value = localizacao.Bairro;
                    inserirLocalizacao.Parameters.Add("@Logradouro", SqlDbType.VarChar).Value = localizacao.Logradouro;
                    inserirLocalizacao.Parameters.Add("@Latitude", SqlDbType.VarChar).Value = localizacao.Latitude;
                    inserirLocalizacao.Parameters.Add("@Longitude", SqlDbType.VarChar).Value = localizacao.Longitude;
                    conn.Open();
                    int resultado = Convert.ToInt32(inserirLocalizacao.ExecuteScalar());
                    return resultado;
                }
            }
        }

        internal IEnumerable<Localizacao> GetLocalizacoes()
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "SELECT Bairro, Logradouro, Latitude, Longitude FROM [dbo].[Localizacao](nolock)";
                using (SqlCommand selecionarContatos = new SqlCommand(comandSQL, conn))
                {
                    conn.Open();
                    using (SqlDataReader data = selecionarContatos.ExecuteReader())
                    {
                        List<Localizacao> localizacoes = new List<Localizacao>();
                        while (data.Read())
                        {
                            string bairro = data.IsDBNull(0) ? string.Empty : data.GetString(0);
                            string logradouro = data.IsDBNull(1) ? string.Empty : data.GetString(1);
                            double latitude = data.IsDBNull(2) ? 0 : data.GetDouble(2);
                            double longitude = data.IsDBNull(2) ? 0 : data.GetDouble(3);
                            localizacoes.Add(new Localizacao
                            {
                                Bairro = bairro,
                                Logradouro = logradouro,
                                Latitude = latitude,
                                Longitude = longitude
                            });
                        }
                        return localizacoes;
                    }
                }
            }
        }

        internal void InserirRestaurante(Restaurante restaurante)
        {
            int? idContato = null;
            int? idLocalizacao = null;
            if (restaurante.Contato != null)
            {
                Contato contato = restaurante.Contato;
                idContato = InserirContato(contato);
            }
            if (restaurante.Localizacao != null)
            {
                Localizacao localizacao = restaurante.Localizacao;
                idLocalizacao = InserirLocalizacao(localizacao);
            }
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "INSERT INTO [dbo].[Restaurante]([Nome], [Capacidade], [Categoria], [LocalizacaoId], [ContatoId]) VALUES (@Nome, @Capacidade, @Categoria, @Localizacao, @Contato)";
                using (SqlCommand inserirRestaurante = new SqlCommand(comandSQL, conn))
                {
                    inserirRestaurante.Parameters.Add("@Nome", SqlDbType.VarChar).Value = restaurante.Nome;
                    inserirRestaurante.Parameters.Add("@Capacidade", SqlDbType.VarChar).Value = restaurante.Capacidade;
                    inserirRestaurante.Parameters.Add("@Categoria", SqlDbType.VarChar).Value = restaurante.Categoria;
                    inserirRestaurante.Parameters.Add("@Localizacao", SqlDbType.VarChar).Value = idLocalizacao;
                    inserirRestaurante.Parameters.Add("@Contato", SqlDbType.VarChar).Value = idContato != null ? idContato : (object)DBNull.Value;
                    conn.Open();
                    int resultado = inserirRestaurante.ExecuteNonQuery();
                }
            }
        }
    }
}

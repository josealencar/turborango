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
        readonly private string DELETAR = "DELETAR";
        readonly private string ATUALIZAR = "ATUALIZAR";
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

        internal Contato GetContato(int id)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "SELECT Site, Telefone FROM [dbo].[Contato](nolock) WHERE Id=@Id";
                using (SqlCommand selecionarContatos = new SqlCommand(comandSQL, conn))
                {
                    selecionarContatos.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
                    conn.Open();
                    using (SqlDataReader data = selecionarContatos.ExecuteReader())
                    {
                        Contato contato = new Contato();
                        while (data.Read())
                        {
                            contato.Site = data.IsDBNull(0) ? string.Empty : data.GetString(0);
                            contato.Telefone = data.IsDBNull(1) ? string.Empty : data.GetString(1);
                        }
                        return contato;
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
                    inserirLocalizacao.Parameters.Add("@Latitude", SqlDbType.Decimal).Value = localizacao.Latitude;
                    inserirLocalizacao.Parameters.Add("@Longitude", SqlDbType.Decimal).Value = localizacao.Longitude;
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
                            double longitude = data.IsDBNull(3) ? 0 : data.GetDouble(3);
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

        internal Localizacao GetLocalizacao(int id)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "SELECT Bairro, Logradouro, Latitude, Longitude FROM [dbo].[Localizacao](nolock) WHERE Id=@Id";
                using (SqlCommand selecionarContatos = new SqlCommand(comandSQL, conn))
                {
                    selecionarContatos.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
                    conn.Open();
                    using (SqlDataReader data = selecionarContatos.ExecuteReader())
                    {
                        Localizacao localizacao = new Localizacao();
                        while (data.Read())
                        {
                            localizacao.Bairro = data.IsDBNull(0) ? string.Empty : data.GetString(0);
                            localizacao.Logradouro = data.IsDBNull(1) ? string.Empty : data.GetString(1);
                            localizacao.Latitude = data.IsDBNull(2) ? 0 : data.GetDouble(2);
                            localizacao.Longitude = data.IsDBNull(3) ? 0 : data.GetDouble(3);
                        }
                        return localizacao;
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

        internal void Remover(int id)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "DELETE FROM [dbo].[Restaurante] WHERE Id=@Id";
                using (SqlCommand inserirContato = new SqlCommand(comandSQL, conn))
                {
                    inserirContato.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
                    conn.Open();
                    int resultado = inserirContato.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Restaurante> Todos()
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "SELECT Nome, Capacidade, Categoria, LocalizacaoId, ContatoId FROM [dbo].[Restaurante](nolock)";
                using (SqlCommand selecionarRestaurantes = new SqlCommand(comandSQL, conn))
                {
                    conn.Open();
                    using (SqlDataReader data = selecionarRestaurantes.ExecuteReader())
                    {
                        List<Restaurante> restaurantes = new List<Restaurante>();
                        while (data.Read())
                        {
                            string nome = data.IsDBNull(0) ? string.Empty : data.GetString(0);
                            int? capacidade = data.IsDBNull(1) ? Convert.ToInt32(null) : Convert.ToInt32(data.GetInt32(1));
                            Categoria categoria = (Categoria) Enum.Parse(typeof(Categoria), data.GetString(2), ignoreCase : true);
                            int idLocalizacao = data.IsDBNull(3) ? 0 : Convert.ToInt32(data.GetInt32(3));
                            int idContato = data.IsDBNull(4) ? 0 : Convert.ToInt32(data.GetInt32(4));
                            Localizacao localizacao = new Localizacao();
                            if (idLocalizacao != 0) localizacao = GetLocalizacao(idLocalizacao);
                            Contato contato = new Contato();
                            if (idContato != 0) contato = GetContato(idContato);

                            Restaurante novo = new Restaurante
                            {
                                Nome = nome,
                                Capacidade = capacidade,
                                Categoria = categoria,
                                Localizacao = localizacao,
                                Contato = contato
                            };

                            restaurantes.Add(novo);
                        }
                        return restaurantes;
                    }
                }
            }
        }

        public void Atualizar(int id, Restaurante restaurante)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "SELECT LocalizacaoId, ContatoId FROM [dbo].[Restaurante](nolock) WHERE Id=@Id";
                using (SqlCommand selecionarRestaurantes = new SqlCommand(comandSQL, conn))
                {
                    selecionarRestaurantes.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
                    conn.Open();
                    using (SqlDataReader data = selecionarRestaurantes.ExecuteReader())
                    {
                        int idLocalizacao = data.IsDBNull(0) ? 0 : Convert.ToInt32(data.GetInt32(3));
                        int idContato = data.IsDBNull(1) ? 0 : Convert.ToInt32(data.GetInt32(4));
                        Localizacao localizacao = new Localizacao();
                        if (idLocalizacao != 0) localizacao = GetLocalizacao(idLocalizacao);
                        Contato contato = new Contato();
                        if (idContato != 0) contato = GetContato(idContato);
                        AlterarRestaurante(id, restaurante);
                        if (idLocalizacao != 0) AlterarLocalizacao(idLocalizacao, localizacao);
                        if (idContato != 0) AlterarContato(idContato, contato);
                    }
                }
            }
        }

        private void AlterarContato(int idContato, Contato contato)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "UPDATE [dbo].[Contato] SET Site=@Site, Telefone=@Telefone WHERE Id=@Id";
                using (SqlCommand alterarContato = new SqlCommand(comandSQL, conn))
                {
                    alterarContato.Parameters.Add("@Site", SqlDbType.VarChar).Value = contato.Site;
                    alterarContato.Parameters.Add("@Telefone", SqlDbType.VarChar).Value = contato.Telefone;
                    alterarContato.Parameters.Add("@Id", SqlDbType.VarChar).Value = idContato;
                    conn.Open();
                    int resultado = alterarContato.ExecuteNonQuery();
                }
            }
        }

        private void AlterarLocalizacao(int idLocalizacao, Localizacao localizacao)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "UPDATE [dbo].[Localizacao] SET Bairro=@Bairro, Logradouro=@Logradouro, Latitude=@Latitude, Longitude=@Longitude WHERE Id=@Id";
                using (SqlCommand alterarLocalizacao = new SqlCommand(comandSQL, conn))
                {
                    alterarLocalizacao.Parameters.Add("@Bairro", SqlDbType.VarChar).Value = localizacao.Bairro;
                    alterarLocalizacao.Parameters.Add("@Logradouro", SqlDbType.VarChar).Value = localizacao.Logradouro;
                    alterarLocalizacao.Parameters.Add("@Latitude", SqlDbType.VarChar).Value = localizacao.Latitude;
                    alterarLocalizacao.Parameters.Add("@Longitude", SqlDbType.VarChar).Value = localizacao.Longitude;
                    alterarLocalizacao.Parameters.Add("@Id", SqlDbType.VarChar).Value = idLocalizacao;
                    conn.Open();
                    int resultado = alterarLocalizacao.ExecuteNonQuery();
                }
            }
        }

        private void AlterarRestaurante(int id, Restaurante restaurante)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "UPDATE [dbo].[Restaurante] SET Nome=@Nome, Capacidade=@Capacidade, Categoria=@Categoria WHERE Id=@Id";
                using (SqlCommand alterarRestaurante = new SqlCommand(comandSQL, conn))
                {
                    alterarRestaurante.Parameters.Add("@Nome", SqlDbType.VarChar).Value = restaurante.Nome;
                    alterarRestaurante.Parameters.Add("@Capacidade", SqlDbType.VarChar).Value = restaurante.Capacidade;
                    alterarRestaurante.Parameters.Add("@Categoria", SqlDbType.VarChar).Value = restaurante.Categoria;
                    alterarRestaurante.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
                    conn.Open();
                    int resultado = alterarRestaurante.ExecuteNonQuery();
                }
            }
        }
    }
}

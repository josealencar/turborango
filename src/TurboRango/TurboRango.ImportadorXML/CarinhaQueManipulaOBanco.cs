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

        internal void Inserir(Contato contato)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string comandSQL = "INSERT INTO [dbo].[Contato]([Site], [Telefone]) VALUES (@Site, @Telefone)";
                using (SqlCommand inserirContato = new SqlCommand(comandSQL, conn))
                {
                    inserirContato.Parameters.Add("@Site", SqlDbType.VarChar).Value = contato.Site;
                    inserirContato.Parameters.Add("@Telefone", SqlDbType.VarChar).Value = contato.Telefone;
                    conn.Open();
                    int resultado = inserirContato.ExecuteNonQuery();
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
                        IList<Contato> contatos = null;
                        while (data.Read())
                        {
                            string site = data.IsDBNull(0) ? string.Empty : data.GetString(0);
                            string telefone = data.IsDBNull(1) ? string.Empty : data.GetString(1);
                            contatos.Add(new Contato
                            {
                                Site = site,
                                Telefone = telefone
                            });
                        }
                        return contatos.ToList();
                    }
                }
            }
        }
    }
}

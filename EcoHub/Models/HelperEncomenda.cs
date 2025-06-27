using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace EcoHub.Models.Helpers
{
    public class HelperEncomenda : HelperBase
    {

        public List<Encomenda> list(Encomenda.EstadoEncomenda estado)
        {
            DataTable dt = new DataTable();
            List<Encomenda> saida = new List<Encomenda>();
            SqlDataAdapter telefone = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QEncomenda_List";
            comando.Parameters.AddWithValue("@Estado_Encomenda", estado);

            telefone.SelectCommand = comando;
            telefone.Fill(dt);

            foreach (DataRow linha in dt.Rows)
            {
                Encomenda encomenda = new Encomenda(linha["GuidEncomenda"].ToString());
                encomenda.guidUsuario = Guid.Parse(linha["guidUsuario"].ToString());
                encomenda.data_pedido = Convert.ToDateTime(linha["data_pedido"]);
                encomenda.data_prevista_entrega = Convert.ToDateTime(linha["data_pedido"]);
                encomenda.estado_encomenda = (Encomenda.EstadoEncomenda)Convert.ToByte(linha["estado_encomenda"]);
                saida.Add(encomenda);
            }
            return saida;
        }

        public void delete(string guidEncomenda2Del)
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QEncomenda_Delete";
            comando.Parameters.AddWithValue("@GuidEncomenda", guidEncomenda2Del);
            conexao.Open();
            comando.ExecuteNonQuery();
            conexao.Close();
            conexao.Dispose();
        }

        public Encomenda? get(string guidEncomenda)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter telefone = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            //comando.CommandType = CommandType.Text;
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            //comando.CommandText = "SELECT * FROM tPrenda WHERE guidEncomenda = @GuidEncomenda";
            comando.CommandText = "QEncomenda_Get";
            comando.Parameters.AddWithValue("@GuidEncomenda", guidEncomenda);

            telefone.SelectCommand = comando;
            telefone.Fill(dt);

            if (dt.Rows.Count == 1)
            {
                //Tenho UMA encomenda
                DataRow linha = dt.Rows[0];
                Encomenda encomenda = new Encomenda("" + linha["GuidEncomenda"].ToString());
                encomenda.guidUsuario = Guid.Parse(linha["guidUsuario"].ToString());
                encomenda.data_pedido = Convert.ToDateTime(linha["data_pedido"]);
                encomenda.data_prevista_entrega = Convert.ToDateTime(linha["data_pedido"]);
                encomenda.estado_encomenda = (Encomenda.EstadoEncomenda)Convert.ToByte(linha["estado_encomenda"]);
                return encomenda;
            }
            return null;
        }

        public bool save(Encomenda encomendaSent, string guidEncomenda = "")
        {
            bool result = false;
            Encomenda? encomenda2Save;
            string instrucaoSQL = "";
            if (string.IsNullOrEmpty(guidEncomenda)) // Fixed the error by explicitly calling string.IsNullOrEmpty with the required parameter
            {
                encomenda2Save = new Encomenda();
            }
            else
            {
                encomenda2Save = get(guidEncomenda);
            }
            if (encomenda2Save != null)
            {
                encomenda2Save.guidUsuario = encomendaSent.guidUsuario;
                encomenda2Save.data_pedido = encomendaSent.data_pedido;
                encomenda2Save.data_prevista_entrega = encomendaSent.data_prevista_entrega;
                encomenda2Save.estado_encomenda = encomendaSent.estado_encomenda;
                if (encomenda2Save.GuidEncomenda == Guid.Empty.ToString())
                {
                    //instrucaoSQL = "INSERT INTO tPrenda " +
                    //                "(designacao, preco_unitario, estoque, estado_produto) " +
                    //                "VALUES " +
                    //                "(@designacao, @preco_unitario, @estoque, @Estado_Encomenda)";
                    instrucaoSQL = "QEncomenda_Insert";
                }
                else
                {
                    //instrucaoSQL = "UPDATE tPrenda " +
                    //                "SET designacao = @designacao, preco_unitario = @preco_unitario, " +
                    //                "estoque = @estoque, estado_produto = @Estado_Encomenda " +
                    //                "WHERE guidEncomenda = @GuidEncomenda";
                    instrucaoSQL = "QEncomenda_Update";
                }
                SqlCommand comando = new SqlCommand();
                SqlConnection conexao = new SqlConnection(ConetorHerdado);
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = instrucaoSQL;
                comando.Connection = conexao;

                comando.Parameters.AddWithValue("@Designacao", encomenda2Save.guidUsuario);
                comando.Parameters.AddWithValue("@Preco_Unitario", encomenda2Save.data_pedido);
                comando.Parameters.AddWithValue("@Estoque", encomenda2Save.data_prevista_entrega);
                comando.Parameters.AddWithValue("@Estado_Encomenda", encomenda2Save.estado_encomenda);
                comando.Parameters.AddWithValue("@GuidEncomenda", encomenda2Save.GuidEncomenda);

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();
                conexao.Dispose();
                result = true;

            }
            return result;
        }

    }
}

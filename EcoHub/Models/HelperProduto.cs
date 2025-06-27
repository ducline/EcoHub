using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Diagnostics;

namespace EcoHub.Models.Helpers
{
    public class HelperProduto : HelperBase
    {

        public List<Produto> list(Produto.EstadoProduto estado)
        {
            DataTable dt = new DataTable();
            List<Produto> saida = new List<Produto>();
            SqlDataAdapter telefone = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QProduto_List";
            comando.Parameters.AddWithValue("@Estado_Produto", estado);

            telefone.SelectCommand = comando;
            telefone.Fill(dt);

            foreach (DataRow linha in dt.Rows)
            {
                Produto produto = new Produto(linha["GuidProduto"].ToString());

                produto.designacao = linha["designacao"].ToString();
                produto.preco_unitario = Convert.ToDecimal(linha["preco_unitario"]);
                produto.estoque = Convert.ToDecimal(linha["estoque"]);
                produto.estado_produto = (Produto.EstadoProduto)Convert.ToByte(linha["estado_produto"]);
                saida.Add(produto);
            }
            return saida;
        }

        public void delete(string guidProduto2Del)
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QProduto_Delete";
            comando.Parameters.AddWithValue("@GuidProduto", guidProduto2Del);
            conexao.Open();
            comando.ExecuteNonQuery();
            conexao.Close();
            conexao.Dispose();
        }

        public Produto? get(string guidProduto)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter telefone = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            //comando.CommandType = CommandType.Text;
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            //comando.CommandText = "SELECT * FROM tPrenda WHERE guidProduto = @GuidProduto";
            comando.CommandText = "QProduto_Get";
            comando.Parameters.AddWithValue("@GuidProduto", guidProduto);

            telefone.SelectCommand = comando;
            telefone.Fill(dt);

            if (dt.Rows.Count == 1)
            {
                //Tenho UMA produto
                DataRow linha = dt.Rows[0];
                Produto produto = new Produto("" + linha["guidProduto"].ToString());
                produto.designacao = "" + linha["designacao"].ToString();
                produto.preco_unitario = Convert.ToDecimal(linha["preco_unitario"]);
                produto.estoque = Convert.ToDecimal(linha["estoque"]);
                produto.estado_produto = (Produto.EstadoProduto)Convert.ToByte(linha["estado_produto"]);
                return produto;
            }
            return null;
        }

        public bool save(Produto produtoSent, string guidProduto = "")
        {
            bool result = false;
            Produto? produto2Save;
            string instrucaoSQL = "";
            if (string.IsNullOrEmpty(guidProduto)) // Fixed the error by explicitly calling string.IsNullOrEmpty with the required parameter
            {
                produto2Save = new Produto();
            }
            else
            {
                produto2Save = get(guidProduto);
            }
            if (produto2Save != null)
            {
                produto2Save.designacao = produtoSent.designacao;
                produto2Save.preco_unitario = produtoSent.preco_unitario;
                produto2Save.estoque = produtoSent.estoque;
                produto2Save.estado_produto = produtoSent.estado_produto;
                if (produto2Save.GuidProduto == Guid.Empty.ToString())
                {
                    //instrucaoSQL = "INSERT INTO tPrenda " +
                    //                "(designacao, preco_unitario, estoque, estado_produto) " +
                    //                "VALUES " +
                    //                "(@designacao, @preco_unitario, @estoque, @Estado_Produto)";
                    instrucaoSQL = "QProduto_Insert";
                    Debug.WriteLine("Inserting new product: " + produto2Save.designacao);
                }
                else
                {
                    //instrucaoSQL = "UPDATE tPrenda " +
                    //                "SET designacao = @designacao, preco_unitario = @preco_unitario, " +
                    //                "estoque = @estoque, estado_produto = @Estado_Produto " +
                    //                "WHERE guidProduto = @GuidProduto";
                    instrucaoSQL = "QProduto_Update";
                    Debug.WriteLine("Updating product: " + produto2Save.designacao + " with Guid: " + produto2Save.GuidProduto);
                }
                SqlCommand comando = new SqlCommand();
                SqlConnection conexao = new SqlConnection(ConetorHerdado);
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = instrucaoSQL;
                comando.Connection = conexao;

                comando.Parameters.AddWithValue("@Designacao", produto2Save.designacao);
                comando.Parameters.AddWithValue("@Preco_Unitario", produto2Save.preco_unitario);
                comando.Parameters.AddWithValue("@Estoque", produto2Save.estoque);
                comando.Parameters.AddWithValue("@Estado_Produto", produto2Save.estado_produto);
                comando.Parameters.AddWithValue("@GuidProduto", produto2Save.GuidProduto);

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();
                conexao.Dispose();
                result = true;

            }
            Debug.WriteLine("Sucessfully Updated/Inserted product: " + produto2Save?.designacao);
            return result;
        }

        // METRICS

        public int getNrPrendas()
        {
            int nrPrendas = 0;
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QProduto_GetNumero";
            conexao.Open();
            nrPrendas = Convert.ToInt32(comando.ExecuteScalar());
            conexao.Close();
            conexao.Dispose();
            return nrPrendas;
        }

        public decimal getTotalPrendas()
        {
            decimal totalPrendas = 0;
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QProduto_GetValorTotal";
            conexao.Open();
            totalPrendas = Convert.ToDecimal(comando.ExecuteScalar());
            conexao.Close();
            conexao.Dispose();
            return totalPrendas;
        }
        public decimal getTotalPorAdquirir()
        {
            decimal totalPrendas = 0;
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QProduto_GetValorPorAdquirir";
            conexao.Open();
            try
            {
                totalPrendas = Convert.ToDecimal(comando.ExecuteScalar());

            }
            catch
            {
                totalPrendas = 0.0M;
            }

            conexao.Close();
            conexao.Dispose();
            return totalPrendas;
        }

    }
}

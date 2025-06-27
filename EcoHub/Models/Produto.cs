using static EcoHub.Models.Encomenda;

namespace EcoHub.Models {
    public class Produto {

        public enum EstadoProduto {
            Disponivel,
            Esgotado,
            Arquivado,
            Todas = 99
        }

        private Guid _guidProduto;
        
        public string GuidProduto {
            get { return _guidProduto.ToString(); }
        }

        public string designacao { get; set; }
        public decimal preco_unitario { get; set; }
        public decimal estoque { get; set; }
        public EstadoProduto estado_produto { get; set; }
        //public string imagem_nome { get; set; }


        public Produto() {
            _guidProduto = Guid.Empty;
            designacao = "";
            preco_unitario = 0.0M;
            estoque = 0.0M;
            estado_produto = EstadoProduto.Disponivel;
            //imagem_nome = "";
        }
        public Produto(string GuidProduto) {
            //_guidProduto = GuidProduto;
            Guid.TryParse(GuidProduto, out _guidProduto);
            designacao = "";
            preco_unitario = 0.0M;
            estoque = 0.0M;
            estado_produto = EstadoProduto.Disponivel;
            //imagem_nome = "";
        }
    }
}

using static EcoHub.Models.Encomenda;

namespace EcoHub.Models
{
  
    public class ItemPedido
    {

        public Guid _guidItemPedido;
        public string GuidItemPedido
        {
            get { return _guidItemPedido.ToString(); }
        }

        public Guid guidEncomenda { get; set; }

        public Guid guidProduto { get; set; }

        public decimal quantidade { get; set; }

        public ItemPedido()
        {
            _guidItemPedido = Guid.NewGuid();
            quantidade = 0.0M;
        }

        public ItemPedido(string GuidItemPedido)
        {
            //_guidEncomenda = GuidEncomenda;
            Guid.TryParse(GuidItemPedido, out _guidItemPedido);
            guidEncomenda = Guid.Empty;
            guidProduto = Guid.Empty;
            quantidade = 0.0M;
        }
    }
}

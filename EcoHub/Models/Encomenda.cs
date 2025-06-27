using static EcoHub.Models.Produto;

namespace EcoHub.Models {
    public class Encomenda {

        public enum EstadoEncomenda {
            EmCaminho,
            Cancelada,
            Entregue,
            Todas=99
        }

        private Guid _guidEncomenda;
        
        public string GuidEncomenda {
            get { return _guidEncomenda.ToString(); }
        }

        public Guid guidUsuario { get; set; }

        public DateTime data_pedido { get; set; }
        public DateTime data_prevista_entrega { get; set; }
        public EstadoEncomenda estado_encomenda { get; set; }


        public Encomenda() {
            _guidEncomenda = Guid.Empty;
            guidUsuario = Guid.Empty;
            data_pedido = DateTime.Now;
            data_prevista_entrega = DateTime.Now.AddDays(7);
            estado_encomenda = EstadoEncomenda.EmCaminho;
        }
        public Encomenda(string GuidEncomenda) {
            //_guidEncomenda = GuidEncomenda;
            Guid.TryParse(GuidEncomenda, out _guidEncomenda);
            guidUsuario = Guid.Empty;
            data_pedido = DateTime.Now;
            data_prevista_entrega = DateTime.Now.AddDays(7);
            estado_encomenda = EstadoEncomenda.EmCaminho;
        }
    }
}

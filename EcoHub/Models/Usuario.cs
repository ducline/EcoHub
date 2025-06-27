namespace EcoHub.Models {
    public class Usuario {
        public Guid _guidUsuario { get; set; } = Guid.NewGuid();
        public string nome { get; set; } 

        public string email { get; set; }
        public string endereco { get; set; }
        public string senha { get; set; } // Senha criptografada

        public int nivel_acesso { get; set; }  // 0 = Visitante, 1 = Cliente (Autor), 2 = Funcionario da EcoDesign (Editor/Admin)


    }
}

using System.Text.Json;
using EcoHub.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;


namespace EcoHub.Models {
    public class HelperUsuario : HelperBase
    {

        public Usuario setGuest()
        {
            return new Usuario
            {
                _guidUsuario = Guid.NewGuid(),
                nome = "Anónimo",
                email = "Anonimo@ISTEC.pt",
                nivel_acesso = 0,
                senha = ""
            };
        }


        public Usuario? authUser(string email, string senha)
        {
            Usuario? usuario = null;

            using (SqlConnection connection = new SqlConnection(ConetorHerdado))
            {
                string query = @"SELECT guidUsuario, nome, email, nivel_acesso, senha 
                         FROM tUsuario 
                         WHERE email = @Email AND senha = @Senha";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Senha", senha);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Debug.WriteLine("Utilizador encontrado.");
                            usuario = new Usuario
                            {
                                _guidUsuario = reader.GetGuid(0),
                                nome = reader.GetString(1),
                                email = reader.GetString(2),
                                nivel_acesso = (int)reader.GetByte(3),
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erro ao autenticar: {ex.Message}");
                }
            }
            Debug.WriteLine(usuario);
            return usuario;
        }


        public string serializeConta(Usuario conta) {
            return JsonSerializer.Serialize(conta);   
        }

        public Usuario? deserializeConta(string json) {
            Usuario? c;

            try {

                c = JsonSerializer.Deserialize<Usuario>(json);
            }
            catch {

                return null;

            }
            return c;
        }
    }
}
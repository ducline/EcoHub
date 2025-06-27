using EcoHub.Models;

namespace EcoHub
{
    public class Program
    {
        //public static List<Produto> _prendas;
        public static string Conetor = "";
        public static string SmtpIP = "";
        public static void Main(string[] args)
        {
            //_prendas = new List<Produto>();

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(20));
            builder.Services.AddMvc();
            var config = builder.Configuration.GetSection("Configuracao").Get<Configuracao>();
            Conetor = config.Conexao;
            SmtpIP = config.SmtpIP;

            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");
            app.UseSession();
            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Produto}/{action=Index}/{id?}"
            );  

            app.Run();
        }
    }
}

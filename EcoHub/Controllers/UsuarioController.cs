using EcoHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace EcoHub.Controllers {
    public class UsuarioController : Controller {
        //public IActionResult Login()
        //{
        //    // HttpContext.Session.SetString("nivelAcesso", "1");
        //    HelperUsuario helper = new HelperUsuario();
        //    HttpContext.Session.SetString("contaAcesso", helper.serializeConta(helper.authUser("operador@istec.pt", "4321")));
        //    return RedirectToAction("Index", "Produto");
        //}

        //public IActionResult LoginAdmin()
        //{
        //    //HttpContext.Session.SetString("nivelAcesso", "2");
        //    HelperUsuario helper = new HelperUsuario();
        //    HttpContext.Session.SetString("contaAcesso", helper.serializeConta(helper.authUser("admin@istec.pt", "1234")));
        //    return RedirectToAction("Index", "Produto");
        //}

        [HttpGet]

        public IActionResult Login() {
            //string nivelAcesso = HttpContext.Session.GetString("nivelAcesso") ?? "0";
            //if (nivelAcesso == "0") {
            return View();
            //}
            //return RedirectToAction("Index", "Produto");
        }

        [HttpPost]
        public IActionResult Login(Usuario contaEnviada)
        {
            HelperUsuario helper = new HelperUsuario();
            Usuario? user = helper.authUser(contaEnviada.email, contaEnviada.senha);
            ViewBag.MensagemErro = null; // Limpa a mensagem de erro

            if (user == null)
            {
                ViewBag.MensagemErro = "Email ou senha inválidos.";
                return View(); // volta à mesma página com a mensagem
            }

            HttpContext.Session.SetString("contaAcesso", helper.serializeConta(user));
            return RedirectToAction("Index", "Produto");
        }





        public IActionResult Logout() {
            // HttpContext.Session.SetString("nivelAcesso", "0");
            HelperUsuario helper = new HelperUsuario();
            HttpContext.Session.SetString("contaAcesso", helper.serializeConta(helper.setGuest()));
            return RedirectToAction("Index", "Produto");
        }
    }
}

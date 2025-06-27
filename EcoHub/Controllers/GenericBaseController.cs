using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EcoHub.Models;

namespace EcoHub.Controllers {
    public class GenericBaseController : Controller {

        //protected string _nivelAcesso = "0";

        protected Usuario? _usuario;

        public override void OnActionExecuting(ActionExecutingContext context) {
            base.OnActionExecuting(context);
            //_nivelAcesso = HttpContext.Session.GetString("contaAcesso") ?? "0";
            HelperUsuario helperUsuario = new HelperUsuario();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("contaAcesso"))) {
                //SE vier aqui estou a entrar no site pela primeira vez
                HttpContext.Session.SetString("contaAcesso", helperUsuario.serializeConta(helperUsuario.setGuest()));
                
            }
            // Havendo ou não uma Sessão, aqui ja tenho uma conta valida em session
            _usuario = helperUsuario.deserializeConta(HttpContext.Session.GetString("contaAcesso") ?? string.Empty);
            

        }
    }
}

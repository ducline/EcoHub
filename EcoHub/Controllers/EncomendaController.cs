using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EcoHub.Controllers;
using EcoHub.Models;
using EcoHub.Models.Helpers;
namespace EcoHub.Controllers {

    public class EncomendaController : GenericBaseController {

        //private string _usuario = "0";

        //public override void OnActionExecuting(ActionExecutingContext context) {
        //    base.OnActionExecuting(context);
        //    _usuario = HttpContext.Session.GetString("nivelAcesso") ?? "0";
        //}
        public IActionResult Lista(string id, string searchTerm) {
            Encomenda.EstadoEncomenda estadoEncomenda = Encomenda.EstadoEncomenda.EmCaminho;
            switch (id) {
                case "0":
                    estadoEncomenda = Encomenda.EstadoEncomenda.EmCaminho;
                    break;
                case "1":
                    estadoEncomenda = Encomenda.EstadoEncomenda.Cancelada;
                    break;
                case "2":
                    estadoEncomenda = Encomenda.EstadoEncomenda.Entregue;
                    break;
                case "3":
                    estadoEncomenda = Encomenda.EstadoEncomenda.Todas;
                    break;
                default:
                    estadoEncomenda = Encomenda.EstadoEncomenda.Todas;
                    break;
            }

            HelperEncomenda helper = new HelperEncomenda();
            List<Encomenda> lista = helper.list(estadoEncomenda);

            //ViewBag.NumeroPrendas = helper.getNrPrendas();
            //ViewBag.TotalPrendas = helper.getTotalPrendas();
            //ViewBag.TotalPorAdquirir = helper.getTotalPorAdquirir();
            //ViewBag.nivel_acesso = HttpContext.Session.GetString("nivelAcesso") ?? "0";
            // Filtrar por nome (case insensitive)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                lista = lista.Where(p => p.GuidEncomenda != null &&
                            p.GuidEncomenda.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                            .ToList();
            }

            ViewBag.CurrentPage = "Encomenda";
            ViewBag.SelectedFilter = id ?? "3";
            ViewBag.SearchTerm = searchTerm;
            ViewBag.Usuario = _usuario;

            return View(lista);
        }

        public IActionResult Feedback(string id) {
            //var encomenda = Program._prendas.Find(p => p.GuidPrenda == id);
            HelperEncomenda helper = new HelperEncomenda();
            Encomenda? encomenda = helper.get(id);
            if (encomenda != null) {
                return View(encomenda);
            }
            else {
                return RedirectToAction("ListaEncomenda", "Encomenda");
            }
        }

        [HttpGet]
        public IActionResult Encomendar() {
            //string nivelAcesso = HttpContext.Session.GetString("nivelAcesso") ?? "0";
            if (_usuario.nivel_acesso > 1) {
                return View();
            }
            return RedirectToAction("ListaEncomenda", "Encomenda");
        }

        [HttpPost]
        public IActionResult Encomendar(Encomenda encomenda) {
            //string nivelAcesso = HttpContext.Session.GetString("nivelAcesso") ?? "0";
            //if _nivelAcesso 
            if (_usuario.nivel_acesso > 0) {

                if (ModelState.IsValid) {
                    //Program._prendas.Add(encomenda);
                    HelperEncomenda helper = new HelperEncomenda();
                    helper.save(encomenda);
                    return RedirectToAction("ListaEncomenda", "Encomenda");
                }
                return View(encomenda);
            }
            return RedirectToAction("ListaEncomenda", "Encomenda");
        }


        public IActionResult Cancelar(string id) {
            if (_usuario.nivel_acesso == 2) {
                HelperEncomenda helper = new HelperEncomenda();
                helper.delete(id);
            }
            return RedirectToAction("ListaEncomenda", "Encomenda");
        }

    }

}

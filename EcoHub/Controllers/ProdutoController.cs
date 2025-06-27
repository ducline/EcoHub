using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EcoHub.Controllers;
using EcoHub.Models;
using EcoHub.Models.Helpers;
namespace EcoHub.Controllers {

    public class ProdutoController : GenericBaseController {

        //private string _usuario = "0";

        //public override void OnActionExecuting(ActionExecutingContext context) {
        //    base.OnActionExecuting(context);
        //    _usuario = HttpContext.Session.GetString("nivelAcesso") ?? "0";
        //}
        public IActionResult Index(string id, string searchTerm) {
            Produto.EstadoProduto estadoListagem = Produto.EstadoProduto.Disponivel;
            switch (id) {
                case "0":
                    estadoListagem = Produto.EstadoProduto.Disponivel;
                    break;
                case "1":
                    estadoListagem = Produto.EstadoProduto.Esgotado;
                    break;
                case "2":
                    estadoListagem = Produto.EstadoProduto.Arquivado;
                    break;
                case "3":
                    estadoListagem = Produto.EstadoProduto.Todas;
                    break;
                default:
                    estadoListagem = Produto.EstadoProduto.Todas;
                    break;
            }

            HelperProduto helper = new HelperProduto();
            List<Produto> lista = helper.list(estadoListagem);

            //ViewBag.NumeroPrendas = helper.getNrPrendas();
            //ViewBag.TotalPrendas = helper.getTotalPrendas();
            //ViewBag.TotalPorAdquirir = helper.getTotalPorAdquirir();
            //ViewBag.nivel_acesso = HttpContext.Session.GetString("nivelAcesso") ?? "0";
            // Filtrar por nome (case insensitive)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                lista = lista.Where(p => p.designacao != null &&
                            p.designacao.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                            .ToList();
            }

            ViewBag.CurrentPage = "Produto";
            ViewBag.SelectedFilter = id ?? "3";
            ViewBag.SearchTerm = searchTerm;
            ViewBag.Usuario = _usuario;

            return View(lista);
        }

        public IActionResult Detalhe(string id) {
            //var produto = Program._prendas.Find(p => p.GuidPrenda == id);
            HelperProduto helper = new HelperProduto();
            Produto? produto = helper.get(id);
            if (produto != null) {
                return View(produto);
            }
            else {
                return RedirectToAction("Index", "Produto");
            }
        }

        [HttpGet]
        public IActionResult Criar() {
            //string nivelAcesso = HttpContext.Session.GetString("nivelAcesso") ?? "0";
            if (_usuario.nivel_acesso > 1) {
                return View();
            }
            return RedirectToAction("Index", "Produto");
        }

        [HttpPost]
        public IActionResult Criar(Produto produto)
        {
            if (_usuario.nivel_acesso > 1)
            {
                if (ModelState.IsValid)
                {
                    // handle image upload
                    //if (imagem != null && imagem.Length > 0)
                    //{
                    //    string pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");
                    //    if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                    //    string nomeImagem = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
                    //    string caminhoCompleto = Path.Combine(pasta, nomeImagem);

                    //    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    //    {
                    //        imagem.CopyTo(stream);
                    //    }

                    //    produto.imagem_nome = nomeImagem;
                    //}

                    HelperProduto helper = new HelperProduto();
                    helper.save(produto);
                    return RedirectToAction("Index", "Produto");
                }

                return View(produto);
            }

            return RedirectToAction("Index", "Produto");
        }

        [HttpGet]
        public IActionResult Editar(string id) {
            //var prenda2Edit = Program._prendas.Find(p => p.GuidPrenda == id);
            if (_usuario.nivel_acesso > 1) {
                HelperProduto helper = new HelperProduto();
                Produto? prenda2Edit = helper.get(id);
                if (prenda2Edit != null) {
                    return View(prenda2Edit);
                }
                else {
                    return RedirectToAction("Index", "Produto");
                }
            }
            else {
                return RedirectToAction("Index", "Produto");
            }
        }

        [HttpPost]
        public IActionResult Editar(string id, Produto produtoPostado) {
            if (_usuario.nivel_acesso > 1) {
                if (ModelState.IsValid) {
                    HelperProduto helper = new HelperProduto();
                    helper.save(produtoPostado, id);
                    return RedirectToAction("Index", "Produto");
                }
                else {
                    return View(produtoPostado);
                }
            }
            else {
                return RedirectToAction("Index", "Produto");
            }
        }


        public IActionResult Matar(string id) {
            if (_usuario.nivel_acesso == 2) {
                HelperProduto helper = new HelperProduto();
                helper.delete(id);
            }
            return RedirectToAction("Index", "Produto");
        }

    }

}

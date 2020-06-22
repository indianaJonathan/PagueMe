using MePague.DAO;
using MePague.Models;
using MePague.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MePague.Controllers
{
    public class ContaController : Controller
    {
        // GET: Conta
        public ActionResult Edit()
        {
            if (Sessao.isLogged()) {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                return Redirect(Url.Action("Edit", "Usuario") + "/" + u.id);
            }
            return RedirectToAction("Login", "Usuario");
        }
    }
}
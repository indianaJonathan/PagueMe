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
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            if (Sessao.isLogged()) {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                if ("A".Equals(u.tipo)) {
                    List<Cliente> clientes = new List<Cliente>();
                    clientes = ClienteDAO.getClientes();
                    ViewBag.Clientes = clientes;
                    return View();
                } else
                {
                    return RedirectToAction("Index", "Despesa");
                }
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult Edit(int id)
        {
            if (Sessao.isLogged())
            {
                return View(ClienteDAO.getCliente(id));
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public ActionResult Edit(Cliente c)
        {
            if (Sessao.isLogged())
            {
                Cliente cliente = ClienteDAO.getCliente(c.id);
                cliente.id = c.id;
                cliente.nome = c.nome;
                cliente.email = c.email;


                ClienteDAO.updateCliente(cliente);
                return RedirectToAction("Index", "Cliente");
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult Delete(int id)
        {
            if (Sessao.isLogged())
            {
                ClienteDAO.deleteCliente(id);
                return RedirectToAction("Index", "Cliente");
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult Add()
        {
            if (Sessao.isLogged())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }


        [HttpPost]
        public ActionResult Add(Cliente c)
        {
            if (Sessao.isLogged())
            {
                if (ModelState.IsValid)
                {
                    if (ClienteDAO.addCliente(c))
                    {
                        return RedirectToAction("Index", "Cliente");
                    }
                    ModelState.AddModelError("", "Erro ao incluir usuario!");
                    return View(c);
                }
                return View(c);
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }
    }
}
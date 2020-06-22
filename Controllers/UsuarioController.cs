using MePague.DAO;
using MePague.Models;
using MePague.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;

namespace MePague.Controllers
{
    public class UsuarioController : Controller
    {
        static Usuario a = new Usuario();
        public ActionResult Index()
        {
            if (Sessao.isLogged()) {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                if (u.tipo.Equals(TipoUsuario.Administrador)) {
                    ViewBag.Usuarios = UsuarioDAO.getUsuarios();
                    return View();
                } else
                {
                    Sessao.Sair();
                    return RedirectToAction("Login", "Usuario");
                }
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult Add()
        {
            if (Sessao.isLogged())
            {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                if (TipoUsuario.Administrador.Equals(u.tipo)) {
                    List<Status> status = UsuarioDAO.status();
                    ViewBag.Status = new SelectList(status, "sigla", "nome");

                    return View();
                } else
                {
                    ViewBag.Despesa = DespesaDAO.searchDespesasPorDono(u.cliente.id);
                    return RedirectToAction("Index", "Despesa");
                }
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public ActionResult Add(Usuario u)
        {
            if (Sessao.isLogged()) {
                UsuarioDAO.Add(u);
                return RedirectToAction("Index", "Usuario");
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult Edit(int id)
        {
            if (Sessao.isLogged()) {
                a = UsuarioDAO.getUsuario(id);
                return View(UsuarioDAO.getUsuario(id));
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public ActionResult Edit(Usuario u)
        {
            if (Sessao.isLogged())
            {
                bool relogin = false;
                if (a.nome.Equals(Sessao.RetornarUsuario()))
                {
                    relogin = true;
                }
                Usuario usuario = UsuarioDAO.getUsuario(u.id);
                usuario.id = u.id;
                if (u.nome != null && !u.nome.Equals(""))
                {
                    usuario.nome = u.nome;
                }
                if (u.senha != null && !u.senha.Equals(""))
                {
                    usuario.senha = u.senha;
                }
                Cliente c = ClienteDAO.getCliente(u.cliente.id);
                if (u.cliente.nome != null && !u.cliente.nome.Equals(""))
                {
                    c.nome = u.cliente.nome;
                }
                if (u.cliente.email != null && !u.cliente.email.Equals(""))
                {
                    c.email = u.cliente.email;
                }
                usuario.cliente = c;

                UsuarioDAO.updateUsuario(usuario);
                if (TipoUsuario.Administrador.Equals(usuario.tipo))
                {
                    if (!relogin) {
                        return RedirectToAction("Index", "Usuario");
                    }
                }
            }
            return RedirectToAction("Login", "Usuario");
        }

        public ActionResult Delete(int id)
        {
            if (Sessao.isLogged())
            {
                Usuario u = UsuarioDAO.getUsuario(id);
                ClienteDAO.deleteCliente(u.cliente.id);
                UsuarioDAO.deleteUsuario(u.id);

                return RedirectToAction("Index", "Usuario");
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario u)
        {
            List<Cliente> clientes = ClienteDAO.getClientes();
            List<Dependente> dependentes = DependenteDAO.getDependentes();
            Usuario a = UsuarioDAO.searchUsuarioPorNome(u.nome);
            if (a != null)
            {
                if (u.senha.Equals(a.senha)) {
                    Sessao.Login(a.nome);
                    return RedirectToAction("Index", "Despesa");
                } else
                {
                    ModelState.AddModelError("", "Senha incorreta!");
                    return View(u);
                }
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult Logout()
        {
            if (Sessao.isLogged()) {
                Sessao.Sair();
            }
            return RedirectToAction("Login", "Usuario");
        }

        public ActionResult AddNew()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNew(Usuario u)
        {
            u.tipo = TipoUsuario.UsuarioComum;
            UsuarioDAO.Add(u);
            return RedirectToAction("Login", "Usuario");
        }
    }
}
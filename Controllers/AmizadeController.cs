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
    public class AmizadeController : Controller
    {

        static Cliente cliente = new Cliente();
        public ActionResult Index()
        {
            if (Sessao.isLogged()) {
                Usuario u = UsuarioDAO.searchUsuarioPorNome(Sessao.RetornarUsuario());
                List<Amizade> amizadesSolicitadas = AmizadeDAO.searchAmizadesSolicitadasAceitasPorCliente(u.cliente.id);
                amizadesSolicitadas.AddRange(AmizadeDAO.searchAmizadesSolicitadasPendentesPorCliente(u.cliente.id));
                List<Amizade> amizadesAceitas = AmizadeDAO.searchAmizadesAceitasPorCliente(u.cliente.id);
                List<Amizade> amizadesPendentes = AmizadeDAO.searchAmizadesPendentesPorCliente(u.cliente.id);
                ViewBag.AmizadesSolicitadas = amizadesSolicitadas;
                ViewBag.AmizadesAceitas = amizadesAceitas;
                ViewBag.AmizadesPendentes = amizadesPendentes;
                return View();
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
                List<Cliente> cs = ClienteDAO.getClientes();
                IEnumerable<SelectListItem> clientes = new SelectList(cs, "id", "nome");
                ViewBag.Clientes = clientes;
                cliente = u.cliente;
                return View();
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public ActionResult Add(Amizade a)
        {
            if(Sessao.isLogged())
            {
                Amizade amizade = new Amizade();
                Cliente solicitado = ClienteDAO.getCliente(a.solicitado.id);
                Cliente solicitante = cliente;
                amizade.solicitado = solicitado;
                amizade.solicitante = solicitante;
                amizade.status = StatusSolicitacao.Pendente;

                AmizadeDAO.Add(amizade);
                return RedirectToAction("Index", "Amizade"); 
            } else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult aceitarAmizade(int id)
        {
            Amizade a = AmizadeDAO.getAmizade(id);
            a.status = StatusSolicitacao.Aceito;
            AmizadeDAO.atualizarAmizade(a);
            return RedirectToAction("Index", "Amizade");
        }

        public ActionResult recusarAmizade(int id)
        {
            Amizade a = AmizadeDAO.getAmizade(id);
            a.status = StatusSolicitacao.Recusado;
            AmizadeDAO.atualizarAmizade(a);
            return RedirectToAction("Index", "Amizade");
        }

        public ActionResult desfazerAmizade(int id)
        {
            Amizade a = AmizadeDAO.getAmizade(id);
            AmizadeDAO.desfazerAmizade(a);
            return RedirectToAction("Index", "Amizade");
        }
    }
}
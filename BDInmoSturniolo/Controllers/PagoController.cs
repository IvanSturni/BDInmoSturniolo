using BDInmoSturniolo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Controllers
{
    public class PagoController : Controller
    {
        private readonly IRepositorioPago repositorio;
        private readonly IRepositorio<Contrato> repositorioContrato;

        public PagoController(IRepositorioPago repositorio, IRepositorio<Contrato> repositorioContrato)
        {
            this.repositorio = repositorio;
            this.repositorioContrato = repositorioContrato;
        }

        // GET: PagoController
        [Authorize]
        public ActionResult Index()
        {
            IList<Pago> lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: InmuebleController/PorPropietario/5
        [Authorize]
        public ActionResult PorContrato(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                IList<Pago> lista = repositorio.ObtenerPorContrato(id);
                return View("Index", lista);
            }
        }

        // GET: PagoController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var ent = repositorio.Obtener(id);
            return View(ent);
        }

        // GET: PagoController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Contratos = repositorioContrato.ObtenerTodos();
            return View();
        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Pago ent)
        {
            try
            {
                int res = repositorio.Alta(ent);
                TempData["Mensaje"] = $"Pago creado con éxito! Id: {res}";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                TempData["Error"] = e.Number + " " + e.Message;
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View();
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View();
            }
        }

        // GET: PagoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var ent = repositorio.Obtener(id);
            ViewBag.Contratos = repositorioContrato.ObtenerTodos();
            return View(ent);
        }

        // POST: PagoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Pago ent)
        {
            try
            {
                repositorio.Modificacion(ent);
                TempData["Mensaje"] = "Pago modificado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                TempData["Error"] = e.Number + " " + e.Message;
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View(ent);
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View(ent);
            }
        }

        // GET: PagoController/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var ent = repositorio.Obtener(id);
            return View(ent);
        }

        // POST: PagoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id, Pago ent)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Pago eliminado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                if (e.Number == 547)
                {
                    TempData["Error"] = "No se pudo eliminar, está en uso.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

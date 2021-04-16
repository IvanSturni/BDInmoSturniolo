using BDInmoSturniolo.Models;
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
        private readonly IRepositorio<Pago> repositorio;
        private readonly IRepositorio<Contrato> repositorioContrato;

        public PagoController(IRepositorio<Pago> repositorio, IRepositorio<Contrato> repositorioContrato)
        {
            this.repositorio = repositorio;
            this.repositorioContrato = repositorioContrato;
        }

        // GET: PagoController
        public ActionResult Index()
        {
            IList<Pago> lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: PagoController/Details/5
        public ActionResult Details(int id)
        {
            var p = repositorio.Obtener(id);
            return View(p);
        }

        // GET: PagoController/Create
        public ActionResult Create()
        {
            ViewBag.Contratos = repositorioContrato.ObtenerTodos();
            return View();
        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago p)
        {
            try
            {
                int res = repositorio.Alta(p);
                TempData["Mensaje"] = $"Pago creado con éxito! Id: {res}";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                TempData["Error"] = e.Number + " " + e.Message;
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View();
            }
            catch (Exception e)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View();
            }
        }

        // GET: PagoController/Edit/5
        public ActionResult Edit(int id)
        {
            var p = repositorio.Obtener(id);
            ViewBag.Contratos = repositorioContrato.ObtenerTodos();
            return View(p);
        }

        // POST: PagoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago p)
        {
            try
            {
                repositorio.Modificacion(p);
                TempData["Mensaje"] = "Pago modificado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                TempData["Error"] = e.Number + " " + e.Message;
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View(p);
            }
            catch (Exception e)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View(p);
            }
        }

        // GET: PagoController/Delete/5
        public ActionResult Delete(int id)
        {
            var p = repositorio.Obtener(id);
            return View(p);
        }

        // POST: PagoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pago i)
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
            catch (Exception e)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

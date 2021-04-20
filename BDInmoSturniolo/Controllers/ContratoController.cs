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
    public class ContratoController : Controller
    {
        private readonly IRepositorioContrato repositorio;
        private readonly IRepositorio<Inquilino> repositorioInquilino;
        private readonly IRepositorioInmueble repositorioInmueble;

        public ContratoController(IRepositorioContrato repositorio, IRepositorioInmueble repositorioInmueble, IRepositorio<Inquilino> repositorioInquilino)
        {
            this.repositorio = repositorio;
            this.repositorioInquilino = repositorioInquilino;
            this.repositorioInmueble = repositorioInmueble;
        }

        // GET: ContratoController
        [Authorize]
        public ActionResult Index()
        {
            IList<Contrato> lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: ContratoController/Cancelar
        [Authorize]
        public ActionResult Vigentes()
        {
            IList<Contrato> lista = repositorio.ObtenerVigentes();
            ViewBag.Cancelar = true;
            return View("Index",lista);
        }

        // GET: ContratoController/Cancelar
        [Authorize]
        public ActionResult PorInmueble(int id)
        {
            IList<Contrato> lista = repositorio.ObtenerPorInmueble(id);
            Inmueble ent = repositorioInmueble.Obtener(id);
            if (ent == null) return RedirectToAction(nameof(Index));
            ViewBag.PorInmueble = ent;
            return View("Index", lista);
        }

        // GET: ContratoController/ConfirmarCancelar
        [Authorize]
        public ActionResult Cancelar(int id)
        {
            var ent = repositorio.Obtener(id);
            if (ent == null) return RedirectToAction(nameof(Index));
            return View(ent);
        }

        // GET: ContratoController/ConfirmarCancelar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Cancelar(int id, Contrato ent)
        {
            repositorio.Cancelar(id);
            TempData["Mensaje"] = "Contrato cancelado con éxito!";
            return RedirectToAction(nameof(Vigentes));
        }

        // GET: ContratoController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var ent = repositorio.Obtener(id);
            if (ent == null) return RedirectToAction(nameof(Index));
            return View(ent);
        }

        // GET: ContratoController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            ViewBag.FechaInicio = DateTime.Now;
            return View();
        }

        // GET: ContratoController/Create
        [Authorize]
        public ActionResult CrearPara(int id)
        {
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            ViewBag.FechaInicio = TempData.ContainsKey("FechaInicio") ? TempData["FechaInicio"] : DateTime.Now;
            ViewBag.FechaFinal = TempData.ContainsKey("FechaFinal") ? TempData["FechaFinal"] : null;
            ViewBag.paraIdInmueble = id;
            return View(nameof(Create));
        }

        // GET: ContratoController/Renovar/5
        [Authorize]
        public ActionResult Renovar(int id)
        {
            try
            {
                Contrato prev = repositorio.Obtener(id);
                Contrato ent = new Contrato
                {
                    FechaInicio = prev.FechaFinal,
                    FechaFinal = prev.FechaFinal.AddMonths(12),
                    Monto = prev.Inmueble.Precio,
                    InquilinoId = prev.InquilinoId,
                    InmuebleId = prev.InmuebleId
                };
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                return View(ent);
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                return View(nameof(Vigentes));
            }
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato ent)
        {
            try
            {
                int res = repositorio.Alta(ent);
                TempData["Mensaje"] = $"Contrato creado con éxito! Id: {res}";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                TempData["Error"] = e.Number + " " + e.Message;
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                return View();
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                return View();
            }
        }


        // GET: ContratoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                var ent = repositorio.Obtener(id);
                if (ent == null) throw new Exception("No se encontró el Contrato indicado.");
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                return View(ent);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato ent)
        {
            try
            {
                repositorio.Modificacion(ent);
                TempData["Mensaje"] = "Contrato modificado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                TempData["Error"] = e.Number + " " + e.Message;
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                return View(ent);
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                return View(ent);
            }
        }

        // GET: ContratoController/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                var ent = repositorio.Obtener(id);
                if (ent == null) throw new Exception("No se encontró el Contrato indicado.");
                return View(ent);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id, Contrato ent)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Contrato eliminado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                if (e.Number == 547)
                {
                    TempData["Error"] = "No se pudo eliminar, está en uso.";
                }
                return RedirectToAction(nameof(Index)); ;
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                return RedirectToAction(nameof(Index)); ;
            }
        }
    }
}

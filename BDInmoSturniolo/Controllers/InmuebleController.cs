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
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorio<Propietario> repositorioPropietario;
        private readonly IRepositorioContrato repositorioContrato;

        public InmuebleController(IRepositorioInmueble repositorio, IRepositorio<Propietario> repositorioPropietario, IRepositorioContrato repositorioContrato)
        {
            this.repositorio = repositorio;
            this.repositorioPropietario = repositorioPropietario;
            this.repositorioContrato = repositorioContrato;
        }

        // GET: InmuebleController
        [Authorize]
        public ActionResult Index()
        {
            IList<Inmueble> lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: InmuebleController
        [Authorize]
        public ActionResult Disponible()
        {
            IList<Inmueble> lista = repositorio.ObtenerDisponibles();
            return View(lista);
        }

        // GET: InmuebleController
        [HttpPost]
        [Authorize]
        public ActionResult Disponible(FechasView f)
        {
            IList<Inmueble> lista = repositorio.ObtenerDisponiblesEntreFechas(f.FechaInicio, f.FechaFinal);
            ViewBag.Fechas = f;
            TempData["FechaInicio"] = f.FechaInicio;
            TempData["FechaFinal"] = f.FechaFinal;

            return View(lista);
        }

        [Authorize]
        public ActionResult Fechas()
        {
            return View();
        }


        // GET: InmuebleController/PorPropietario/5
        [Authorize]
        public ActionResult PorPropietario(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Propietario = repositorioPropietario.Obtener(id);
                IList<Inmueble> lista = repositorio.ObtenerPorPropietario(id);
                return View("Index", lista);
            }
        }

        // GET: InmuebleController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var ent = repositorio.Obtener(id);
            return View(ent);
        }

        // GET: InmuebleController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            return View();
        }

        // GET: InmuebleController/Create
        [Authorize]
        public ActionResult CrearParaPropietario(int id)
        {
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            ViewBag.ParaPropietario = id;
            return View("Create");
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble ent)
        {
            try
            {
                int res = repositorio.Alta(ent);
                TempData["Mensaje"] = $"Inmueble creado con éxito! Id: {res}";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                TempData["Error"] = e.Number + " " + e.Message;
                ViewBag.Usos = Inmueble.ObtenerUsos();
                ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                return View();
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                ViewBag.Usos = Inmueble.ObtenerUsos();
                ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                return View();
            }
        }

        // GET: InmuebleController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var ent = repositorio.Obtener(id);
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            return View(ent);
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble ent)
        {
            try
            {
                repositorio.Modificacion(ent);
                TempData["Mensaje"] = "Inmueble modificado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                TempData["Error"] = e.Number + " " + e.Message;
                ViewBag.Usos = Inmueble.ObtenerUsos();
                ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                return View(ent);
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                ViewBag.Usos = Inmueble.ObtenerUsos();
                ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                return View(ent);
            }
        }

        // GET: InmuebleController/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var ent = repositorio.Obtener(id);
            return View(ent);
        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id, Inmueble ent)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Inmueble eliminado con éxito!";
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

using BDInmoSturniolo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly RepositorioInmueble repositorio;
        private readonly RepositorioPropietario repositorioPropietario;

        public InmuebleController(IConfiguration configuration)
        {
            this.repositorio = new RepositorioInmueble(configuration);
            this.repositorioPropietario = new RepositorioPropietario(configuration);
        }

        // GET: InmuebleController
        public ActionResult Index()
        {
            IList<Inmueble> lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: InmuebleController/Details/5
        public ActionResult Details(int id)
        {
            var p = repositorio.Obtener(id);
            return View(p);
        }

        // GET: InmuebleController/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble i)
        {
            try
            {
                int res = repositorio.Alta(i);
                TempData["Mensaje"] = $"Inmueble creado con éxito! Id: {res}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["Mensaje"] = e.StackTrace;
                return View();
            }
        }

        // GET: InmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            var i = repositorio.Obtener(id);
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            return View(i);
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble i)
        {
            try
            {
                repositorio.Modificacion(i);
                TempData["Mensaje"] = "Inmueble modificado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            var i = repositorio.Obtener(id);
            return View(i);
        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble i)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Inmueble eliminado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

using BDInmoSturniolo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario repositorio;

        public PropietarioController(IConfiguration configuration)
        {
            this.repositorio = new RepositorioPropietario(configuration);
        }

        // GET: PropietarioController
        public ActionResult Index()
        {
            try
            {
                IList<Propietario> lista = repositorio.ObtenerTodos();
                return View(lista);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        // GET: PropietarioController/Details/5
        public ActionResult Details(int id)
        {
            var p = repositorio.Obtener(id);
            return View(p);
        }

        // GET: PropietarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
                int res = repositorio.Alta(p);
                TempData["Mensaje"] = $"Propietario creado con éxito! Id: {res}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["Mensaje"] = e.StackTrace;
                return View();
            }
        }

        // GET: PropietarioController/Edit/5
        public ActionResult Edit(int id)
        {
            var p = repositorio.Obtener(id);
            return View(p);
        }

        // POST: PropietarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario p)
        {
            try
            {
                repositorio.Modificacion(p);
                TempData["Mensaje"] = "Propietario modificado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PropietarioController/Delete/5
        public ActionResult Delete(int id)
        {
            var p = repositorio.Obtener(id);
            return View(p);
        }

        // POST: PropietarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario p)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Propietario eliminado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["Error"] = e.;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

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
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino repositorio;
        private readonly IConfiguration configuration;

        public InquilinoController(IConfiguration configuration)
        {
            this.repositorio = new RepositorioInquilino(configuration);
            this.configuration = configuration;
        }

        // GET: InquilinoController
        public ActionResult Index()
        {
            try
            {
                IList<Inquilino> lista = repositorio.ObtenerTodos();
                return View(lista);
            }
            catch (Exception)
            {
                throw;
            }

        }

        // GET: InquilinoController/Details/5
        public ActionResult Details(int id)
        {
            var p = repositorio.Obtener(id);
            return View(p);
        }

        // GET: InquilinoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InquilinoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino i)
        {
            try
            {
                int res = repositorio.Alta(i);
                TempData["Mensaje"] = $"Inquilino creado con éxito! Id: {res}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["Mensaje"] = e.StackTrace;
                return View();
            }
        }

        // GET: InquilinoController/Edit/5
        public ActionResult Edit(int id)
        {
            var i = repositorio.Obtener(id);
            return View(i);
        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino i)
        {
            try
            {
                repositorio.Modificacion(i);
                TempData["Mensaje"] = "Inquilino modificado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InquilinoController/Delete/5
        public ActionResult Delete(int id)
        {
            var i = repositorio.Obtener(id);
            return View(i);
        }

        // POST: InquilinoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino i)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Inquilino eliminado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(i);
            }
        }
    }
}

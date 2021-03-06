using BDInmoSturniolo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly IRepositorio<Inquilino> repositorio;

        public InquilinoController(IRepositorio<Inquilino> repositorio)
        {
            this.repositorio = repositorio;
        }

        // GET: InquilinoController
        [Authorize]
        public ActionResult Index()
        {
            IList<Inquilino> lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: InquilinoController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var ent = repositorio.Obtener(id);
            if (ent == null) return RedirectToAction(nameof(Index));
            return View(ent);
        }

        // GET: InquilinoController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: InquilinoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inquilino ent)
        {
            try
            {
                int res = repositorio.Alta(ent);
                TempData["Mensaje"] = $"Inquilino creado con éxito! Id: {res}";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                TempData["Error"] = e.Number + " " + e.Message;
                return View();
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                return View();
            }
        }

        // GET: InquilinoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var ent = repositorio.Obtener(id);
            if (ent == null) return RedirectToAction(nameof(Index));
            return View(ent);
        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inquilino ent)
        {
            try
            {
                repositorio.Modificacion(ent);
                TempData["Mensaje"] = "Inquilino modificado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                TempData["Error"] = e.Number + " " + e.Message;
                return View(ent);
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                return View(ent);
            }
        }

        // GET: InquilinoController/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var ent = repositorio.Obtener(id);
            if (ent == null) return RedirectToAction(nameof(Index));
            return View(ent);
        }

        // POST: InquilinoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id, Inquilino ent)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Inquilino eliminado con éxito!";
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

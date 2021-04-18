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
    public class PropietarioController : Controller
    {
        private readonly IRepositorio<Propietario> repositorio;

        public PropietarioController(IConfiguration configuration, IRepositorio<Propietario> repositorio)
        {
            this.repositorio = repositorio;
        }

        // GET: PropietarioController
        [Authorize]
        public ActionResult Index()
        {
            IList<Propietario> lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: PropietarioController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var ent = repositorio.Obtener(id);
            return View(ent);
        }

        // GET: PropietarioController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Propietario ent)
        {
            try
            {
                int res = repositorio.Alta(ent);
                TempData["Mensaje"] = $"Propietario creado con éxito! Id: {res}";
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

        // GET: PropietarioController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var ent = repositorio.Obtener(id);
            return View(ent);
        }

        // POST: PropietarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Propietario ent)
        {
            try
            {
                repositorio.Modificacion(ent);
                TempData["Mensaje"] = "Propietario modificado con éxito!";
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

        // GET: PropietarioController/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var ent = repositorio.Obtener(id);
            return View(ent);
        }

        // POST: PropietarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id, Propietario ent)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Propietario eliminado con éxito!";
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

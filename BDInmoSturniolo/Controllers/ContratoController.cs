﻿using BDInmoSturniolo.Models;
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
        private readonly IRepositorio<Contrato> repositorio;
        private readonly IRepositorio<Inquilino> repositorioInquilino;
        private readonly IRepositorioInmueble repositorioInmueble;

        public ContratoController(IRepositorio<Contrato> repositorio, IRepositorioInmueble repositorioInmueble, IRepositorio<Inquilino> repositorioInquilino)
        {
            this.repositorio = repositorio;
            this.repositorioInquilino = repositorioInquilino;
            this.repositorioInmueble = repositorioInmueble;
        }

        // GET: ContratoController
        public ActionResult Index()
        {
            IList<Contrato> lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: ContratoController/Details/5
        public ActionResult Details(int id)
        {
            var p = repositorio.Obtener(id);
            return View(p);
        }

        // GET: ContratoController/Create
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato c)
        {
            try
            {
                int res = repositorio.Alta(c);
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
            catch (Exception e)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                return View();
            }
        }

        // GET: ContratoController/Edit/5
        public ActionResult Edit(int id)
        {
            var i = repositorio.Obtener(id);
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            return View(i);
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato c)
        {
            try
            {
                repositorio.Modificacion(c);
                TempData["Mensaje"] = "Contrato modificado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException e)
            {
                TempData["Error"] = e.Number + " " + e.Message;
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                return View(c);
            }
            catch (Exception e)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                return View(c);
            }
        }

        // GET: ContratoController/Delete/5
        public ActionResult Delete(int id)
        {
            var i = repositorio.Obtener(id);
            return View(i);
        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contrato c)
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
            catch (Exception e)
            {
                TempData["Error"] = "Ocurrió un error inesperado.";
                return RedirectToAction(nameof(Index)); ;
            }
        }
    }
}

using CrudEmissoras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudEmissoras.Controllers
{
    [Route("[controller]/[action]")]
    public class EmissorasController : Controller
    {
        private readonly Contexto _contexto;

        public EmissorasController(Contexto contexto) 
        {
            _contexto = contexto;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            var emissoras = from s in _contexto.Emissoras
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                emissoras = emissoras.Where(s => s.Nome.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    emissoras = emissoras.OrderByDescending(s => s.Nome);
                    break;
            }
            return View(emissoras);
        }

        [HttpGet]
        public IActionResult CriarEmissora() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarEmissora(Emissora emissora) 
        {
            if (ModelState.IsValid)
            {
                Emissora emissoraCadastrada = _contexto.Emissoras.Where(e => e.Nome == emissora.Nome).FirstOrDefault();

                if (emissoraCadastrada == default)
                {
                    _contexto.Add(emissora);
                    await _contexto.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                } else 
                {
                    ViewBag.ErrorMessage = "Emissora já cadastrada.";
                    return View(emissora);
                }
            }
            else return View(emissora);
        }

        [HttpGet]
        public IActionResult AtualizarEmissora(int? id) 
        {
            if (id != null)
            {
                Emissora emissora = _contexto.Emissoras.Find(id);
                return View(emissora);
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarEmissora(int? id, Emissora emissora)
        {
            if (id != null)
            {
                if (ModelState.IsValid)
                {
                    _contexto.Update(emissora);
                    await _contexto.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else return View(emissora);
                
            }
            else return NotFound();
        }

        [HttpGet]
        public IActionResult ExcluirEmissora(int? id)
        {
            if (id != null)
            {
                Emissora emissora = _contexto.Emissoras.Find(id);
                return View(emissora);
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirEmissora(int? id, Emissora emissora)
        {
            if (id != null)
            {
                _contexto.Remove(emissora);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else return NotFound();
        }
    }
}

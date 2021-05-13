using CrudEmissoras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudEmissoras.Controllers
{
    [Route("[controller]/[action]")]
    public class AudienciaController : Controller
    {
        private readonly Contexto _contexto;

        public AudienciaController(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _contexto.Audiencias.ToListAsync());
        }

        [HttpGet]
        public IActionResult CriarAudiencia()
        {
            ViewBag.Emissoras = _contexto.Emissoras.Select(c => new SelectListItem() { Text = c.Nome, Value = c.Id.ToString()}).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarAudiencia(Audiencia audiencia)
            
        {
            audiencia.Emissora_audiencia = _contexto.Emissoras.Where(c => c.Id == audiencia.Emissora_audiencia.Id).FirstOrDefault();
            if (audiencia.Emissora_audiencia.Nome != null)
            {
                _contexto.Audiencias.Add(audiencia);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));                 
            }
            else return View(audiencia);
        }

        [HttpGet]
        public IActionResult AtualizarAudiencia(int? id)
        {
            if (id != null)
            {
                Audiencia audiencia = _contexto.Audiencias.First(c => c.Id == id);

                List<SelectListItem> selectListItems = _contexto.Emissoras.Select(c => new SelectListItem() { Text = c.Nome, Value = c.Id.ToString() }).ToList();
                ViewBag.Emissoras = selectListItems;
                return View(audiencia);
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarAudiencia(int? id, Audiencia audiencia)
        {
            if (id != null)
            {
                audiencia.Emissora_audiencia = _contexto.Emissoras.Where(c => c.Id == audiencia.Emissora_audiencia.Id).FirstOrDefault();

                if (audiencia.Emissora_audiencia.Nome != null)
                {
                    _contexto.Update(audiencia);
                    await _contexto.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else return View(audiencia);
            }
            else return NotFound();
        }

        [HttpGet]
        public IActionResult ExcluirAudiencia(int? id)
        {
            if (id != null)
            {
                Audiencia audiencia = _contexto.Audiencias.Find(id);
                return View(audiencia);
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirAudiencia(int? id, Audiencia audiencia)
        {
            if (id != null)
            {
                _contexto.Remove(audiencia);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else return NotFound();
        }
    }
}

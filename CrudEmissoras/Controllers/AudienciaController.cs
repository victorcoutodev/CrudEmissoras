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

        public async Task<IActionResult> Index(string sortOrder, string searchString) 
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["PointsSortParm"] = String.IsNullOrEmpty(sortOrder) ? "points_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            var audiencias = from s in _contexto.Audiencias
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                audiencias = audiencias.Where(s => s.NomeEmissora.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    audiencias = audiencias.OrderByDescending(s => s.NomeEmissora);
                    break;
                case "Date":
                    audiencias = audiencias.OrderBy(s => s.Data_hora_audiencia);
                    break;
                case "date_desc":
                    audiencias = audiencias.OrderByDescending(s => s.Data_hora_audiencia);
                    break;
                case "points_desc":
                    audiencias = audiencias.OrderByDescending(s => s.Pontos_audiencia);
                    break;
            }
            return View(await audiencias.ToListAsync());
        }

        [HttpGet]
        public IActionResult CriarAudiencia()
        {
            PopulataListaEmissoras();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarAudiencia([Bind("Pontos_audiencia,Data_hora_audiencia,Emissora_audiencia_id")] Audiencia audiencia)
        {
            var EmissoraEncontrada = await _contexto.Emissoras
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == audiencia.Emissora_audiencia_id);

            audiencia.NomeEmissora = EmissoraEncontrada.Nome;

            bool temConflitoDataHora = ExisteAudienciaCadastradaNaMesmaDataHora(audiencia);
            if(temConflitoDataHora)
            {
                ViewBag.ErrorMessage = "Audiencia já cadastrada nesta data hora.";
                PopulataListaEmissoras(EmissoraEncontrada);
                return View(audiencia);
            }

            if (ModelState.IsValid)
            {
                _contexto.Add(audiencia);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else return View(audiencia);
        }

        [HttpGet]
        public async Task<IActionResult> AtualizarAudiencia(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Audiencia audiencia = _contexto.Audiencias.Find(id);

            if (audiencia == null)
            {
                return NotFound();
            }
            PopulataListaEmissoras(audiencia.Emissora_audiencia_id);
            return View(audiencia);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarAudiencia(int? id, Audiencia audiencia)
        {
            var EmissoraEncontrada = await _contexto.Emissoras
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == audiencia.Emissora_audiencia_id);
            audiencia.NomeEmissora = EmissoraEncontrada.Nome;

            if (id == null)
            {
                return NotFound();
            }

            bool temConflitoDataHora = ExisteAudienciaCadastradaNaMesmaDataHora(audiencia);
            if (temConflitoDataHora)
            {
                ViewBag.ErrorMessage = "Audiencia já cadastrada nesta data hora.";
                PopulataListaEmissoras(EmissoraEncontrada);
                return View(audiencia);
            }

            if (ModelState.IsValid)
            {
                _contexto.Audiencias.Update(audiencia);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulataListaEmissoras(EmissoraEncontrada);
            return View(audiencia);
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
        private void PopulataListaEmissoras(object emissoraSelecionada = null)
        {
            var emissoraQuery = from d in _contexto.Emissoras
                                orderby d.Nome
                                select d;
            ViewBag.Emissoras = new SelectList(emissoraQuery.AsNoTracking(), "Id", "Nome", emissoraSelecionada);
        }

        private Boolean ExisteAudienciaCadastradaNaMesmaDataHora(Audiencia audiencia)
        {
            List<Audiencia> audiencias = _contexto.Audiencias.ToList();
            foreach (Audiencia audienciaPersistida in audiencias)
            {
                if (audienciaPersistida.NomeEmissora.Equals(audiencia.NomeEmissora)
                    && audienciaPersistida.Data_hora_audiencia.CompareTo(audienciaPersistida.Data_hora_audiencia) == 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}

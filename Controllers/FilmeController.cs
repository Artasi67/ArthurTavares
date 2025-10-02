using ArthurTavares.Config;
using ArthurTavares.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArthurTavares.Controllers
{
    [Authorize]
    public class FilmeController : Controller
    {
        private readonly DbConfig _dbconfig;

        public FilmeController(DbConfig dbconfig)
        {
            _dbconfig = dbconfig;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Listar()
        {
            var filmes = await _dbconfig.filmes
                                  .Include(p => p.Usuario)
                                  .ToListAsync();

            return View(filmes);
        }

        public IActionResult Cadastrar()
        {
            ViewBag.Usuarios = _dbconfig.usuarios.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(Filme filme)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                filme.Id_filme = int.Parse(userId);
            }

            _dbconfig.filmes.Add(filme);
            await _dbconfig.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var filme = await _dbconfig.filmes
                                         .Include(p => p.Usuario)
                                         .FirstOrDefaultAsync(p => p.Id_filme == id);

            if (filme == null)
            {
                return NotFound();
            }
            ViewBag.Usuarios = _dbconfig.usuarios.ToList();
            return View(filme);
        }

        [HttpPost]
        public async Task<IActionResult> Atualizar(Filme filme)
        {
            _dbconfig.filmes.Update(filme);
            await _dbconfig.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}


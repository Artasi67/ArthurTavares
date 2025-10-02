using ArthurTavares.Config;
using ArthurTavares.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArthurTavares.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly DbConfig _dbconfig;

        public UsuarioController(DbConfig dbconfig)
        {
            _dbconfig = dbconfig;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Editar(int id)
        {
            var usuario = await _dbconfig.usuarios.FindAsync(id);
            return View(usuario);
        }

        public async Task<ActionResult> Listar()
        {
            var usuarios = await _dbconfig.usuarios.ToListAsync();
            return View(usuarios);
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Salvar(Usuario usuario)
        {
            usuario.Senha_usuario = BCrypt.Net.BCrypt.HashPassword(usuario.Senha_usuario);
            await _dbconfig.usuarios.AddAsync(usuario);
            await _dbconfig.SaveChangesAsync();
            return RedirectToRoute("Usuario", "Listar");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Atualizar(Usuario usuario)
        {
            _dbconfig.usuarios.Update(usuario);
            await _dbconfig.SaveChangesAsync();
            return RedirectToRoute("Usuario", "Cadastro");
        }
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Deletar(int id)
        {
            var usuario = _dbconfig.usuarios.Find(id);
            _dbconfig.usuarios.Remove(usuario);
            await _dbconfig.SaveChangesAsync();
            return RedirectToRoute("Usuario", "Index");
        }
    }
}

using ArthurTavares.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArthurTavares.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly DbConfig _dbConfig;

        public AutenticacaoController(DbConfig dbconfig)
        {
            _dbConfig = dbconfig;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Autenticacao(string email, string senha)
        {
            var usuario = _dbConfig.usuarios.FirstOrDefault(
                    u => u.Email_usuario == email);

            if (usuario != null && BCrypt.Net.BCrypt.Verify(senha, usuario.Senha_usuario))
            {
                var regras = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id_usuario.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome_usuario),
                    new Claim(ClaimTypes.Email, usuario.Email_usuario)
                };

                var claimsIdentity = new ClaimsIdentity(
                        regras, CookieAuthenticationDefaults.AuthenticationScheme
                    );

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                    );

                return RedirectToAction("index", "usuario");
            }

            ViewBag.Mensagem = "E-mail ou senha inválidos.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
                );

            return RedirectToAction("autenticacao", "autenticador");
        }
    }
}

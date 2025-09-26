using ArthurTavares.Models;
using Microsoft.EntityFrameworkCore;

namespace ArthurTavares.Config
{
    public class DbConfig : DbContext
    {
        public DbConfig(DbContextOptions<DbConfig> options) : base(options) { }

        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Filme> filmes { get; set; }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoAnderson2.Models;

namespace ProjetoAnderson2.Data
{
    public class LocadoraContext : IdentityDbContext
    {
        public LocadoraContext(DbContextOptions<LocadoraContext> options)
            : base(options)
        {
        }
        public DbSet<Artista> Artista { get; set; } = default!;

        public DbSet<Genero> Genero { get; set; } = default!;

        public DbSet<Filme> Filme { get; set; } = default!;

        public DbSet<Situacao> Situacao { get; set; } = default!;

        public DbSet<ProjetoAnderson2.Models.Contato>? Contato { get; set; }
    }
}
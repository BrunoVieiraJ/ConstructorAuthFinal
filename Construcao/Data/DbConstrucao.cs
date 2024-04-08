using Construcao.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Construcao.Data
{
    public class DbConstrucao : IdentityDbContext
    {
        public DbConstrucao(DbContextOptions options) : base(options)
        { 
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ApplicationUser> users { get; set; }


    }
}

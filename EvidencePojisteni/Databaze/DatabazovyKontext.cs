using EvidencePojisteni.Models;
using EvidencePojisteni.Databaze.Konfigurace;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EvidencePojisteni.Databaze
{
    /// <summary>
    /// Databázový kontext aplikace Evidence pojištění.
    /// Dědí z IdentityDbContext, aby mohl spravovat i uživatele a role.
    /// </summary>
    public class DatabazovyKontext : IdentityDbContext<UzivatelAplikace>
    {
        /// <summary>
        /// Konstruktor kontextu, přijímá konfigurační parametry (např. connection string).
        /// </summary>
        public DatabazovyKontext(DbContextOptions<DatabazovyKontext> moznosti)
            : base(moznosti)
        {
        }

        /* ====================== DbSety (databázové tabulky) ====================== */

        /// <summary>
        /// Tabulka pro pojištěnce.
        /// </summary>
        public DbSet<Pojistenec> Pojistenci { get; set; }

        /// <summary>
        /// Tabulka pro pojistné smlouvy.
        /// </summary>
        public DbSet<Pojisteni> Pojisteni { get; set; }

        /// <summary>
        /// Tabulka pro pojistné události.
        /// </summary>
        public DbSet<PojistnaUdalost> PojistneUdalosti { get; set; }

        /* ====================== Konfigurace relací mezi entitami ====================== */

        /// <summary>
        /// Aplikuje konfigurace entit a jejich vztahů pomocí Fluent API.
        /// </summary>
        /// <param name="modelBuilder">Nástroj pro konfiguraci modelu.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Inicializace Identity systému (uživatelé, role atd.)
            base.OnModelCreating(modelBuilder);

            // Registrace vlastních konfigurací entit (relace, mazání, cizí klíče)
            modelBuilder.ApplyConfiguration(new KonfiguracePojisteni());
            modelBuilder.ApplyConfiguration(new KonfiguracePojistneUdalosti());
            modelBuilder.ApplyConfiguration(new KonfiguracePojistence());
        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EvidencePojisteni.Models
{
    /// <summary>
    /// Databázový kontext aplikace, který slouží k přístupu k databázovým entitám.
    /// Dědí z IdentityDbContext pro integraci s autentizací a správou uživatelů.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<UzivatelAplikace>
    {
        /// <summary>
        /// Konstruktor inicializuje databázový kontext se zadanými možnostmi.
        /// </summary>
        /// <param name="options">Nastavení a konfigurace databázového kontextu.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        /// <summary>
        /// Tabulka s informacemi o pojištěncích.
        /// </summary>
        public DbSet<Pojistenec> Pojistenci { get; set; }

        /// <summary>
        /// Tabulka s informacemi o pojistných smlouvách.
        /// </summary>
        public DbSet<Pojisteni> Pojisteni { get; set; }

        /// <summary>
        /// Tabulka s informacemi o pojistných událostech.
        /// </summary>
        public DbSet<PojistnaUdalost> PojistneUdalosti { get; set; }

        /// <summary>
        /// Konfigurace modelu pro databázové vztahy v aplikaci Evidence pojištění.
        /// </summary>
        /// <param name="modelBuilder">Model builder pro konfiguraci entit.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vztah mezi Pojisteni a Pojistenec: 1:N
            modelBuilder.Entity<Pojisteni>()
                .HasOne(p => p.Pojistenec)
                .WithMany(p => p.Pojisteni)
                .HasForeignKey(p => p.PojistenecId)
                .OnDelete(DeleteBehavior.Cascade); // Smazání pojištěnce smaže i jeho pojištění

            // Vztah mezi PojistnaUdalost a Pojistenec: 1:N
            modelBuilder.Entity<PojistnaUdalost>()
                .HasOne(u => u.Pojistenec)
                .WithMany(p => p.PojistneUdalosti)
                .HasForeignKey(u => u.PojistenecId)
                .OnDelete(DeleteBehavior.Cascade); // Smazání pojištěnce smaže i jeho pojistné události

            // Vztah mezi Pojistenec a UzivatelAplikace pro správné mazání
            modelBuilder.Entity<Pojistenec>()
                .HasOne<UzivatelAplikace>()
                .WithOne()
                .HasForeignKey<Pojistenec>(p => p.UzivatelID)
                .OnDelete(DeleteBehavior.Cascade); // Smazání uživatele smaže i pojištěnce
        }


    }
}

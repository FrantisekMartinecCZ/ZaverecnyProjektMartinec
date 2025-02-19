using EvidencePojisteni.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvidencePojisteni.Databaze.Konfigurace
{
    /// <summary>
    /// Konfigurace entitních pravidel pro entitu PojistnaUdalost.
    /// Nastavuje vztah mezi pojistnou událostí a pojištěncem.
    /// </summary>
    public class KonfiguracePojistneUdalosti : IEntityTypeConfiguration<PojistnaUdalost>
    {
        /// <summary>
        /// Konfiguruje vztahy a chování pro entitu PojistnaUdalost.
        /// </summary>
        /// <param name="builder">Builder pro konfiguraci entity.</param>
        public void Configure(EntityTypeBuilder<PojistnaUdalost> builder)
        {
            /* Vztah: každá pojistná událost patří jednomu pojištěnci,
               jeden pojištěnec může mít více pojistných událostí */
            builder.HasOne(u => u.Pojistenec)
                   .WithMany(p => p.PojistneUdalosti)
                   .HasForeignKey(u => u.PojistenecId)
                   .OnDelete(DeleteBehavior.Cascade); // Smazání pojištěnce smaže i jeho události
        }
    }
}

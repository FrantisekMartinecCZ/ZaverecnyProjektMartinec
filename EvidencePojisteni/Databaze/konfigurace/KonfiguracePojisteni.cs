using EvidencePojisteni.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvidencePojisteni.Databaze.Konfigurace
{
    /// <summary>
    /// Konfigurace entitních pravidel pro entitu Pojisteni.
    /// Nastavuje vztah mezi pojistnou smlouvou a pojištěncem.
    /// </summary>
    public class KonfiguracePojisteni : IEntityTypeConfiguration<Pojisteni>
    {
        /// <summary>
        /// Konfiguruje vztahy a chování pro entitu Pojisteni.
        /// </summary>
        /// <param name="builder">Builder pro konfiguraci entity.</param>
        public void Configure(EntityTypeBuilder<Pojisteni> builder)
        {
            /* Vztah: každý záznam pojištění patří jednomu pojištěnci,
               pojištěnec může mít více pojištění. */
            builder.HasOne(p => p.Pojistenec)
                   .WithMany(p => p.Pojisteni)
                   .HasForeignKey(p => p.PojistenecId)
                   .OnDelete(DeleteBehavior.Cascade); // Smazání pojištěnce smaže i jeho pojištění
        }
    }
}

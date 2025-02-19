using EvidencePojisteni.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvidencePojisteni.Databaze.Konfigurace
{
    /// <summary>
    /// Konfigurace entitních pravidel pro entitu Pojistenec.
    /// Nastavuje vazbu mezi pojištěncem a uživatelem aplikace.
    /// </summary>
    public class KonfiguracePojistence : IEntityTypeConfiguration<Pojistenec>
    {
        /// <summary>
        /// Konfiguruje vztahy a pravidla databázového modelu pro entitu Pojistenec.
        /// </summary>
        /// <param name="builder">Builder pro konfiguraci entity.</param>
        public void Configure(EntityTypeBuilder<Pojistenec> builder)
        {
            /* Nastavení 1:1 vztahu mezi Pojistencem a UzivatelemAplikace */
            builder.HasOne<UzivatelAplikace>()
                   .WithOne()
                   .HasForeignKey<Pojistenec>(p => p.UzivatelID)
                   .OnDelete(DeleteBehavior.Cascade); // Pokud se uživatel smaže, smaže se i pojištěnec
        }
    }
}

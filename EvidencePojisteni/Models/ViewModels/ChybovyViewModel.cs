namespace EvidencePojisteni.Models.ViewModels
{
    /// <summary>
    /// Model pro zobrazen� chybov�ch informac�, kter� se p�ed�v� do chybov�ho pohledu.
    /// Slou�� k identifikaci po�adavku, ve kter�m do�lo k chyb�.
    /// </summary>
    public class ChybovyViewModel
    {
        /// <summary>
        /// Identifik�tor po�adavku, kter� lze vyu��t pro diagnostiku chyb.
        /// M��e obsahovat hodnotu z Activity.Current?.IdPojisteni nebo HttpContext.TraceIdentifier.
        /// </summary>
        public string? IdPozadavku { get; set; } 

        /// <summary>
        /// Vypo��t�van� vlastnost, kter� vrac� true, pokud RequestId nen� null nebo pr�zdn�.
        /// Tato vlastnost se vyu��v� pro rozhodnut�, zda zobrazit RequestId ve v�pisu chyb.
        /// </summary>
        public bool ZobrazitIdPozadavku => !string.IsNullOrEmpty(IdPozadavku); 
    }
}

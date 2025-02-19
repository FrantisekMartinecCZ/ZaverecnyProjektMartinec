namespace EvidencePojisteni.Models.ViewModels
{
    /// <summary>
    /// Model pro zobrazení chybových informací, který se pøedává do chybového pohledu.
    /// Slouží k identifikaci požadavku, ve kterém došlo k chybì.
    /// </summary>
    public class ChybovyViewModel
    {
        /// <summary>
        /// Identifikátor požadavku, který lze využít pro diagnostiku chyb.
        /// Mùže obsahovat hodnotu z Activity.Current?.IdPojisteni nebo HttpContext.TraceIdentifier.
        /// </summary>
        public string? IdPozadavku { get; set; } 

        /// <summary>
        /// Vypoèítávaná vlastnost, která vrací true, pokud RequestId není null nebo prázdný.
        /// Tato vlastnost se využívá pro rozhodnutí, zda zobrazit RequestId ve výpisu chyb.
        /// </summary>
        public bool ZobrazitIdPozadavku => !string.IsNullOrEmpty(IdPozadavku); 
    }
}

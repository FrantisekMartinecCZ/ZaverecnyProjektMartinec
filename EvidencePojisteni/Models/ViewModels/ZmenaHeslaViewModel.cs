using EvidencePojisteni.Models;
using System.ComponentModel.DataAnnotations;

namespace EvidencePojisteni.Models.ViewModels
{
    /// <summary>
    /// Model pro změnu hesla, který obsahuje pole pro aktuální heslo, nové heslo a jeho potvrzení.
    /// Používá se v uživatelském rozhraní při změně hesla.
    /// </summary>
    public class ZmenaHeslaViewModel
    {
        /// <summary>
        /// Aktuální heslo uživatele, které se ověřuje při změně hesla.
        /// </summary>
        [Required(ErrorMessage = "Aktuální heslo je povinné.")] 
        [DataType(DataType.Password)] 
        public string StareHeslo { get; set; } = ""; 

        /// <summary>
        /// Nové heslo, které uživatel chce nastavit.
        /// Musí splňovat minimální a maximální délku a procházet vlastní validací.
        /// </summary>
        [Required(ErrorMessage = "Nové heslo je povinné.")] 
        [DataType(DataType.Password)] 
        [StringLength(100, ErrorMessage = "Heslo musí mít alespoň {2} znaků.", MinimumLength = 6)] 
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujHeslo))] 
        public string NoveHeslo { get; set; } = ""; 

        /// <summary>
        /// Potvrzení nového hesla, které musí odpovídat hodnotě v poli NoveHeslo.
        /// </summary>
        [Required(ErrorMessage = "Potvrzení hesla je povinné.")] 
        [DataType(DataType.Password)] 
        [Compare("NoveHeslo", ErrorMessage = "Nové heslo a potvrzení hesla se neshodují.")] 
        public string PotvrzeniHesla { get; set; } = ""; 
    }
}

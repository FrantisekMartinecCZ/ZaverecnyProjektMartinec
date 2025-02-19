using System.ComponentModel.DataAnnotations;

namespace EvidencePojisteni.Models.ViewModels
{
    /// <summary>
    /// ViewModel pro registraci pojišťováka.
    /// Tento model obsahuje email, heslo a potvrzení hesla, které jsou potřebné při registraci pojišťováka.
    /// </summary>
    public class RegistracePojistovnikaViewModel
    {
        /// <summary>
        /// Emailová adresa pojišťováka.
        /// Musí být vyplněna, odpovídat platnému formátu a splňovat specifické podmínky validace.
        /// </summary>
        [Required(ErrorMessage = "Email je povinný.")] 
        [EmailAddress(ErrorMessage = "Zadejte platnou emailovou adresu.")] 
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujEmailProPojistovaka))] 
        public string Email { get; set; } = ""; 

        /// <summary>
        /// Heslo pojišťováka.
        /// Musí být vyplněno, je maskováno a validováno pomocí vlastní validace.
        /// </summary>
        [Required(ErrorMessage = "Heslo je povinné.")] 
        [DataType(DataType.Password)] 
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujHeslo))] 
        [Display(Name = "Heslo")] 
        public string Heslo { get; set; } = ""; 

        /// <summary>
        /// Potvrzení hesla.
        /// Musí se shodovat s hodnotou v poli Heslo.
        /// </summary>
        [Compare("Heslo", ErrorMessage = "Hesla se neshodují.")] 
        [Display(Name = "Heslo")] 
        public string PotvrzeniHesla { get; set; } = ""; 
    }
}

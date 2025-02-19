using System.ComponentModel.DataAnnotations;

namespace EvidencePojisteni.Models
{
    /// <summary>
    /// Model registrace slouží k registraci nových uživatelů.
    /// Obsahuje email, heslo a potvrzení hesla.
    /// </summary>
    public class Registrace
    {
        /// <summary>
        /// Emailová adresa uživatele.
        /// Musí být vyplněna a odpovídat platnému formátu emailu.
        /// </summary>
        [Required(ErrorMessage = "Vyplňte emailovou adresu")]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        /// <summary>
        /// Heslo uživatele.
        /// Musí být vyplněno, je maskováno a podléhá vlastní validaci.
        /// </summary>
        [Required(ErrorMessage = "Heslo je povinné.")]
        [DataType(DataType.Password)]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujHeslo))]
        [Display(Name = "Heslo")]
        public string Heslo { get; set; } = "";

        /// <summary>
        /// Potvrzení hesla.
        /// Musí být vyplněno a musí se shodovat s hodnotou v poli Heslo.
        /// </summary>
        [Required(ErrorMessage = "Potvrďte heslo")]
        [DataType(DataType.Password)]
        [Compare(nameof(Heslo), ErrorMessage = "Hesla se neshodují.")]
        [Display(Name = "Potvrzení hesla")]
        public string PotvrzeniHesla { get; set; } = "";
    }
}

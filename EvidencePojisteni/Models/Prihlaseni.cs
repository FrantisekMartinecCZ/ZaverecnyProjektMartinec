using EvidencePojisteni.Models;
using System.ComponentModel.DataAnnotations;

namespace EvidencePojisteni.ViewModels
{
    /// <summary>
    /// ViewModel pro přihlášení uživatele.
    /// Obsahuje vlastnosti pro email, heslo a volbu "Pamatuj si mě".
    /// </summary>
    public class Prihlaseni
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
        /// Musí být vyplněno a odpovídat kritériím definovaným vlastní validací.
        /// </summary>
        [Required(ErrorMessage = "Heslo je povinné.")]
        [DataType(DataType.Password)]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujHeslo))]
        public string Heslo { get; set; } = "";

    }
}

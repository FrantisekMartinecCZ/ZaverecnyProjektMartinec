using System.ComponentModel.DataAnnotations;

namespace EvidencePojisteni.Models.ViewModels
{
    /// <summary>
    /// ViewModel pro pojištěnce, který se používá při registraci nebo editaci údajů o pojištěnci.
    /// Obsahuje základní osobní, kontaktní údaje a heslo.
    /// </summary>
    public class PojistenecViewModel
    {
        /// <summary>
        /// Jméno pojištěnce.
        
        /// </summary>
        [Required(ErrorMessage = "Jméno je povinné.")] 
        [StringLength(50, ErrorMessage = "Jméno může mít maximálně 50 znaků.")] 
        
        public string Jmeno { get; set; } = "";

        /// <summary>
        /// Příjmení pojištěnce.
        /// </summary>
        [Required(ErrorMessage = "Příjmení je povinné.")] 
        [StringLength(50, ErrorMessage = "Příjmení může mít maximálně 50 znaků.")] 
      
        public string Prijmeni { get; set; } = "";
        /// <summary>
        /// Datum narození
        /// </summary>
        [Required(ErrorMessage = "Příjmení je povinné.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujDatumNarozeni))]
        public DateTime? DatumNarozeni { get; set; }

        /// <summary>
        /// Emailová adresa pojištěnce.
        /// </summary>
        [Required(ErrorMessage = "Email je povinný.")] 
        [EmailAddress(ErrorMessage = "Zadejte platnou emailovou adresu.")] 
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ZakazatDomenuProPojistence))] 
        public string Email { get; set; } = "";

        /// <summary>
        /// Telefonní číslo pojištěnce.
        /// </summary>
        [Required(ErrorMessage = "Telefon je povinný.")] 
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujTelefon))]
        public string Telefon { get; set; } = "";

        /// <summary>
        /// Ulice
        /// </summary>
        [Required(ErrorMessage = "Ulice je povinná.")]
        [StringLength(100, ErrorMessage = "Ulice může mít maximálně 100 znaků.")]
        
        public string Ulice { get; set; } = "";

        /// <summary>
        /// Město
        /// </summary>
        [Required(ErrorMessage = "Město je povinné.")]
        [StringLength(50, ErrorMessage = "Město může mít maximálně 50 znaků.")]
      
        public string Mesto { get; set; } = "";


        /// <summary>
        /// PSČ pojištěnce.
        /// </summary>
        [Required(ErrorMessage = "PSČ je povinné.")] 
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujPSC))] 
        public string PSC { get; set; } = "";

        /// <summary>
        /// Země původu pojištěnce.
        /// </summary>
        [Required(ErrorMessage = "Země původu je povinná.")]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujZemi))]
        public string ZemePuvodu { get; set; } = "";



        /// <summary>
        /// Heslo pro účet pojištěnce.
        /// </summary>
        [Required(ErrorMessage = "Heslo je povinné.")] 
        [DataType(DataType.Password)] 
        public string Heslo { get; set; } = "";

        /// <summary>
        /// jméno zákonného zástupce
        /// </summary>
        public string? JmenoZakonnehoZastupce { get; set; }

        /// <summary>
        /// přijmení zákonného zástupce
        /// </summary>
        public string? PrijmeniZakonnehoZastupce { get; set; }


        /// <summary>
        /// Pohlaví pojištěnce.
        /// </summary>

        [Required(ErrorMessage = "Výběr pohlaví je povinný.")]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujPohlavi))]
        public Pohlavi? Pohlavi { get; set; }


    }
}


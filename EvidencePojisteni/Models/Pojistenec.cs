using System.ComponentModel.DataAnnotations; 
namespace EvidencePojisteni.Models
{
    /// <summary>
    /// Reprezentuje pojištěnce s osobními a kontaktními údaji.
    /// Třída obsahuje navigační vlastnosti pro pojistné smlouvy a pojistné události.
    /// </summary>
    public class Pojistenec
    {
        
        [Key]
        public int IdPojistence { get; set; }

        [Required(ErrorMessage = "Jméno je povinné.")]
        [StringLength(50, ErrorMessage = "Jméno může mít maximálně 50 znaků.")]
        public string Jmeno { get; set; } = "";
       
        [Required(ErrorMessage = "Příjmení je povinné.")]
        [StringLength(50, ErrorMessage = "Příjmení může mít maximálně 50 znaků.")]
        public string Prijmeni { get; set; } = "";
       
        [Required(ErrorMessage = "Datum narození je povinné.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujDatumNarozeni))]
        public DateTime? DatumNarozeni { get; set; }
    
        [Required(ErrorMessage = "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Zadejte platnou emailovou adresu.")]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ZakazatDomenuProPojistence))]
        public string Email { get; set; } = "";
  
        [Required(ErrorMessage = "Telefon je povinný.")]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujTelefon))]
        public string Telefon { get; set; } = "";

        [Required(ErrorMessage = "Ulice je povinná.")]
        [StringLength(100, ErrorMessage = "Ulice může mít maximálně 100 znaků.")]

        public string Ulice { get; set; } = "";


        [Required(ErrorMessage = "Město je povinné.")]
        [StringLength(50, ErrorMessage = "Město může mít maximálně 50 znaků.")]

        public string Mesto { get; set; } = "";

        [Required(ErrorMessage = "PSČ je povinné.")]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujPSC))]
        public string PSC { get; set; } = "";

        [Required(ErrorMessage = "Země původu je povinná.")]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujZemi))]
        public string ZemePuvodu { get; set; } = "";

        public string? UzivatelID { get; set; }
        public List<Pojisteni> Pojisteni { get; set; } = new List<Pojisteni>();
        public ICollection<PojistnaUdalost> PojistneUdalosti { get; set; } = new List<PojistnaUdalost>();

        public string? JmenoZakonnehoZastupce { get; set; }
        public string? PrijmeniZakonnehoZastupce { get; set; }

        [Required(ErrorMessage = "Výběr pohlaví je povinný.")]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujPohlavi))]
        public Pohlavi? Pohlavi { get; set; }






    }
}

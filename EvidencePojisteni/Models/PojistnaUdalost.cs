using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidencePojisteni.Models
{
    /// <summary>
    /// Reprezentuje pojistnou událost, která obsahuje informace o škodní události,
    /// jejím popisu, datu nahlášení, odhadu škody a stavu vyřízení.
    /// </summary>
    /// <summary>
    /// Reprezentuje pojistnou událost, např. nehoda, požár, krádež, atd.
    /// </summary>
    public class PojistnaUdalost
    {
        [Key]
        public int IdPojistnaUdalost { get; set; }

        [Required(ErrorMessage = "Popis události je povinný.")]
        [StringLength(500, ErrorMessage = "Popis nesmí být delší než 500 znaků.")]
        
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujPopis))]
        public string Popis { get; set; } = "";

        [Required(ErrorMessage = "Datum události je povinné.")]
        [DataType(DataType.Date)]
        // Pokud chceš zakázat budoucí datum, aktivuj:
        //[CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujDatumUdalosti))]
        public DateTime DatumUdalosti { get; set; } = DateTime.Now;
        /// <summary>
        /// Datum nahlášení
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? DatumNahlaseni { get; set; }
        /// <summary>
        /// Odhad škody
        /// </summary>
        [Required(ErrorMessage = "Odhad škody je povinný.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OdhadSkody { get; set; }
        /// <summary>
        /// stav události
        /// </summary>
        [Required(ErrorMessage = "Stav události je povinný.")]
        [RegularExpression("Otevřeno|Vyřízeno|Zamítnuto", ErrorMessage = "Neplatný stav.")]
        public string Stav { get; set; } = "Otevřeno";
        /// <summary>
        /// poznámky
        /// </summary>
        
        [StringLength(100, ErrorMessage = "Popis nesmí být delší než 100 znaků.")]
        public string? Poznamka { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "PojistenecId je povinné.")]
        [ForeignKey("Pojistenec")]
        public int PojistenecId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Pojistenec? Pojistenec { get; set; }
    }
}

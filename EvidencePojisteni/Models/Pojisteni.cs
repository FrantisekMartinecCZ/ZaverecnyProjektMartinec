using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidencePojisteni.Models
{
    /// <summary>
    /// Reprezentuje pojistnou smlouvu, která obsahuje informace o druhu pojištění,
    /// částce pojistky, platnosti smlouvy a propojení s pojištěncem.
    /// </summary>
    public class Pojisteni
    {
        /// <summary>
        /// Unikátní identifikátor pojistné smlouvy (primární klíč).
        /// </summary>
        [Key]
        public int IdPojisteni { get; set; }

        /// <summary>
        /// Typ pojistného produktu (např. životní, úrazové, cestovní pojištění).
        /// </summary>
        [Required(ErrorMessage = "Typ pojištění je povinný.")]
        public string TypPojisteni { get; set; } = "";

        /// <summary>
        /// Výše pojistné částky v měně.
        /// </summary>
        [Required(ErrorMessage = "Částka je povinná.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Částka musí být kladné číslo.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Castka { get; set; }

        /// <summary>
        /// Datum, od kterého je pojistná smlouva platná.
        /// </summary>
        [Required(ErrorMessage = "Datum platnosti od je povinné.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujPlatnostOd))]
        public DateTime PlatnostOd { get; set; } = DateTime.Today;

        /// <summary>
        /// Datum, do kterého je pojistná smlouva platná. Je volitelné.
        /// </summary>
        [DataType(DataType.Date)]
        [CustomValidation(typeof(ValidatorChyb), nameof(ValidatorChyb.ValidujPlatnostDo))]
        public DateTime? PlatnostDo { get; set; }

        /// <summary>
        /// Identifikátor pojištěnce, ke kterému pojistná smlouva patří.
        /// </summary>
        [Required(ErrorMessage = "ID pojištěnce je povinné.")]
        public int PojistenecId { get; set; }

        /// <summary>
        /// Předmět pojištění (např. automobil, nemovitost, zdraví atd.).
        /// </summary>
        [Required(ErrorMessage = "Předmět pojištění je povinný.")]
        public string PredmetPojisteni { get; set; } = string.Empty;

        /// <summary>
        /// Navigační vlastnost pro entitu Pojistenec, ke kterému smlouva náleží.
        /// </summary>
        [ForeignKey("PojistenecId")]
        public Pojistenec? Pojistenec { get; set; }

        /// <summary>
        /// Role v pojištění, může obsahovat například informace o tom, zda je pojištěný hlavním
        /// nebo vedlejším pojištěncem.
        /// </summary>
        [Required(ErrorMessage = "Role v pojištění je povinná.")]
        public RoleVPojisteni RolePojisteni { get; set; }
    }
}

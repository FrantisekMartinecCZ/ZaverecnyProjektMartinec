using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Linq;

namespace EvidencePojisteni.Models
{
    /// <summary>
    /// Statická třída ValidatorChyb obsahuje metody pro vlastní validaci vstupních údajů,
    /// jako jsou datum, popis, heslo, email, telefon a další.
    /// Používá se pomocí [CustomValidation(typeof(ValidatorChyb), nameof(Metoda))].
    /// </summary>
    public static class ValidatorChyb
    {
        /// <summary>
        /// Validuje, zda zadané datum patří do aktuálního roku.
        /// </summary>
        public static ValidationResult? ValidujAktualniRok(DateTime datum, ValidationContext kontext)
        {
            int aktualniRok = DateTime.Now.Year;
            if (datum.Year != aktualniRok)
            {
                return new ValidationResult($"Datum musí být v roce {aktualniRok}.");
            }
            return ValidationResult.Success;
        }

        /// <summary>
        /// Validuje popis (např. pro pojistnou událost):
        /// - neprázdný,
        /// - min. 10 znaků,
        /// - pouze písmena, číslice, mezery a ,.!?-
        /// </summary>
        public static ValidationResult? ValidujPopis(string popis, ValidationContext kontext)
        {
            if (string.IsNullOrWhiteSpace(popis))
            {
                return new ValidationResult("Popis události je povinný.");
            }

            if (popis.Length < 10)
            {
                return new ValidationResult("Popis musí mít alespoň 10 znaků.");
            }

            if (!Regex.IsMatch(popis, @"^[\p{L}0-9\s.,!?-]+$"))
            {
                return new ValidationResult("Popis obsahuje nepovolené znaky.");
            }
            return ValidationResult.Success;
        }

        /// <summary>
        /// Validuje datum "platnost od", aby nebylo v minulosti.
        /// </summary>
        public static ValidationResult? ValidujPlatnostOd(DateTime platnostOd, ValidationContext kontext)
        {
            if (platnostOd.Date < DateTime.Today)
            {
                return new ValidationResult("Datum platnosti od nemůže být v minulosti.");
            }
            return ValidationResult.Success;
        }

        /// <summary>
        /// Validuje datum "platnost do":
        /// - nesmí být před "platnost od",
        /// - nesmí být více než 30 let od "platnost od".
        /// </summary>
        public static ValidationResult? ValidujPlatnostDo(DateTime? platnostDo, ValidationContext kontext)
        {
            if (platnostDo.HasValue)
            {
                if (kontext.ObjectInstance is Pojisteni instance)
                {
                    if (platnostDo < instance.PlatnostOd)
                    {
                        return new ValidationResult("Datum platnosti do nemůže být dříve než datum platnosti od.");
                    }
                    if (platnostDo > instance.PlatnostOd.AddYears(30))
                    {
                        return new ValidationResult("Pojištění nemůže být delší než 30 let.");
                    }
                }
            }
            return ValidationResult.Success;
        }

        /// <summary>
        /// Validuje datum narození:
        /// - nesmí být budoucí,
        /// - maximální věk 100 let.
        /// </summary>
        public static ValidationResult? ValidujDatumNarozeni(DateTime? datumNarozeni, ValidationContext kontext)
        {
            if (!datumNarozeni.HasValue)
            {
                return new ValidationResult("Datum narození je povinné.");
            }

            if (datumNarozeni.Value > DateTime.Today)
            {
                return new ValidationResult("Datum narození nesmí být v budoucnosti.");
            }

            int vek = DateTime.Today.Year - datumNarozeni.Value.Year;
            if (datumNarozeni.Value.Date > DateTime.Today.AddYears(-vek)) vek--;

            if (vek > 100)
            {
                return new ValidationResult("Neplatné datum narození. Zkontrolujte správnost (věk nad 100 let?).");
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Validuje vstup pro jméno, příjmení:
        /// - neprázdné,
        /// - obsahuje jen písmena, čísla, mezery, diakritiku a základní symboly . , ! ? -
        /// </summary>
        public static ValidationResult? ValidujCeskeZnaky(string vstup, ValidationContext kontext)
        {
            if (string.IsNullOrWhiteSpace(vstup))
            {
                return new ValidationResult("Toto pole nesmí být prázdné.");
            }

            if (!Regex.IsMatch(vstup, @"^[\p{L}0-9\s.,!?-]+$"))
            {
                return new ValidationResult("Pole může obsahovat pouze písmena, číslice, mezery a některé interpunkční znaky.");
            }
            return ValidationResult.Success;
        }

        /// <summary>
        /// Validuje telefonní číslo (9-15 číslic, volitelná +, mezery a pomlčky).
        /// </summary>
        public static ValidationResult? ValidujTelefon(string? telefon, ValidationContext kontext)
        {
            if (string.IsNullOrWhiteSpace(telefon))
            {
                return new ValidationResult("Telefonní číslo je povinné.");
            }

            if (!Regex.IsMatch(telefon, @"^\+?[0-9\s\-]{9,15}$"))
            {
                return new ValidationResult("Zadejte platné telefonní číslo (9-15 číslic, ev. s +).");
            }

            return ValidationResult.Success;
        }


        /// <summary>
        /// Validuje PSČ (CZ formát): přesně 5 číslic.
        /// </summary>
        public static ValidationResult? ValidujPSC(string psc, ValidationContext kontext)
        {
            if (string.IsNullOrEmpty(psc))
            {
                return new ValidationResult("PSČ je povinné.");
            }

            if (!Regex.IsMatch(psc, @"^\d{5}$"))
            {
                return new ValidationResult("PSČ musí obsahovat přesně 5 číslic.");
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Validuje heslo: min 6 znaků, aspoň 1 číslo, 1 písmeno, 1 spec. znak, max 100.
        /// </summary>
        public static ValidationResult? ValidujHeslo(string heslo, ValidationContext kontext)
        {
            if (string.IsNullOrEmpty(heslo))
            {
                return new ValidationResult("Heslo je povinné.");
            }

            if (heslo.Length < 6)
            {
                return new ValidationResult("Heslo musí mít alespoň 6 znaků.");
            }

            if (!Regex.IsMatch(heslo, @"\d"))
            {
                return new ValidationResult("Heslo musí obsahovat alespoň jedno číslo.");
            }

            if (!Regex.IsMatch(heslo, @"[a-zA-Z]"))
            {
                return new ValidationResult("Heslo musí obsahovat alespoň jedno písmeno.");
            }

            if (!Regex.IsMatch(heslo, @"[!@#$%^&*(),.?""{}|<>]"))
            {
                return new ValidationResult("Heslo musí obsahovat alespoň jeden speciální znak (!@#$%^&* apod.).");
            }

            if (heslo.Length > 100)
            {
                return new ValidationResult("Heslo může mít maximálně 100 znaků.");
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Validuje email pro pojišťováka (musí mít doménu @pojistovna.cz).
        /// </summary>
        public static ValidationResult? ValidujEmailProPojistovaka(string email, ValidationContext kontext)
        {
            string povolenaDomena = "pojistovna.cz";
            var domena = email.Split('@').LastOrDefault();
            if (domena == null || !domena.Equals(povolenaDomena, StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult($"Email musí být registrován s doménou @{povolenaDomena}.");
            }
            return ValidationResult.Success;
        }

        /// <summary>
        /// Zakazuje doménu 'pojistovna.cz' pro pojištěnce.
        /// </summary>
        public static ValidationResult? ZakazatDomenuProPojistence(string email, ValidationContext kontext)
        {
            string zakazanaDomena = "pojistovna.cz";

            if (string.IsNullOrWhiteSpace(email))
            {
                return ValidationResult.Success;
            }

            var castiEmailu = email.Split('@');
            if (castiEmailu.Length != 2)
            {
                return new ValidationResult("E-mail není ve správném formátu.");
            }

            var domena = castiEmailu[1];
            if (domena.Equals(zakazanaDomena, StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult($"Pojištěnec nemůže mít e-mail s doménou @{zakazanaDomena}.");
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Validace pohlaví
        /// </summary>
        public static ValidationResult? ValidujPohlavi(Pohlavi? pohlavi, ValidationContext context)
        {
            if (!pohlavi.HasValue)
            {
                return new ValidationResult("Výběr pohlaví je povinný.");
            }

            return ValidationResult.Success;
        }


        /// <summary>
        /// Validuje, zda země původu je v povoleném seznamu (příklad).
        /// </summary>
        public static ValidationResult? ValidujZemi(string zeme, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(zeme))
            {
                return new ValidationResult("Země původu je povinná.");
            }

            var povoleneZeme = new[]
            {
                "Česká republika",
                "Slovensko",
                "Německo",
                "Rakousko",
                "Polsko",
                "Maďarsko",
                "Francie",
                "Velká Británie",
                "USA"
            };

            if (!povoleneZeme.Contains(zeme))
            {
                return new ValidationResult($"Neplatná země: {zeme}. Zvolte jednu z povolených zemí.");
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Validuje, že datum události není v budoucnu.
        /// (Použij, pokud nechceš povolit budoucí datum nehody.)
        /// </summary>
        public static ValidationResult? ValidujDatumUdalosti(DateTime datum, ValidationContext kontext)
        {
            if (datum > DateTime.Today)
            {
                return new ValidationResult("Datum události nesmí být v budoucnosti.");
            }
            return ValidationResult.Success;
        }
    }
}

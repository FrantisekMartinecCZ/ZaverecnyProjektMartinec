using System;
using System.Linq;

namespace EvidencePojisteni.PomocnaTrida
{
    /// <summary>
    /// Pomocná třída pro různé užitečné funkce v aplikaci.
    /// </summary>
    public static class PomocnaTrida
    {
        /// <summary>
        /// Formátuje telefonní číslo pro čitelné zobrazení (např. "+420 605 022 242" nebo "605 022 242").
        /// </summary>
        /// <param name="telefon">Telefonní číslo zadané uživatelem.</param>
        /// <returns>Naformátované telefonní číslo.</returns>
        public static string FormatujTelefonniCislo(string telefon)
        {
            /// <summary>
            /// Odstraní mezery na začátku a konci.
            /// </summary>
            string cislo = telefon.Trim();

            /// <summary>
            /// Zkontroluje, zda číslo obsahuje mezinárodní předvolbu (např. +420 nebo 420).
            /// </summary>
            string predvolba = "";
            if (cislo.StartsWith("+"))
            {
                int mezeraIndex = cislo.IndexOfAny(new[] { ' ', '-' });
                if (mezeraIndex > 0)
                {
                    predvolba = cislo.Substring(0, mezeraIndex);
                    cislo = cislo.Substring(mezeraIndex + 1).Replace(" ", "").Replace("-", "");
                }
                else
                {
                    predvolba = cislo.Substring(0, 4);
                    cislo = cislo.Substring(4);
                }
            }
            else if (cislo.Length >= 12 && cislo.StartsWith("420"))
            {
                predvolba = "+420";
                cislo = cislo.Substring(3);
            }

            /// <summary>
            /// Rozdělí číslo po trojicích pro lepší čitelnost.
            /// </summary>
            cislo = RozdelCisloPoTrojicich(cislo);

            return string.IsNullOrEmpty(predvolba) ? cislo : $"{predvolba} {cislo}";
        }

        /// <summary>
        /// Rozdělí číslo na skupiny po třech číslicích.
        /// </summary>
        /// <param name="cislo">Číslo k rozdělení.</param>
        /// <returns>Číslo rozdělené mezerami po trojicích.</returns>
        private static string RozdelCisloPoTrojicich(string cislo)
        {
            return string.Join(" ", Enumerable.Range(0, (cislo.Length + 2) / 3)
                                              .Select(i => cislo.Substring(i * 3, Math.Min(3, cislo.Length - i * 3))));
        }
    }
}

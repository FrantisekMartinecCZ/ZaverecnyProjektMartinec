using System.Collections.Generic;

namespace EvidencePojisteni.Models
{
    /// <summary>
    /// Třída reprezentující souhrnné statistiky týkající se pojištění v systému.
    /// Obsahuje informace o počtu pojištěnců, pojistných událostí, smluv
    /// a seznam nejčastějších typů pojištění.
    /// </summary>
    public class ZpravaStatistik
    {
        /// <summary>
        /// Celkový počet pojištěnců v systému.
        /// </summary>
        public int PocetPojistencu { get; set; }

        /// <summary>
        /// Celkový počet pojistných událostí evidovaných v systému.
        /// </summary>
        public int PocetPojistnychUdalosti { get; set; }

        /// <summary>
        /// Celkový počet pojistných smluv v systému.
        /// </summary>
        public int PocetPojisteni { get; set; }

        /// <summary>
        /// Počet pojistníků (osob, které uzavírají smlouvu a platí pojistné).
        /// </summary>
        public int PocetPojistniku { get; set; }

        /// <summary>
        /// Počet pojištěných osob (osob, které jsou kryté pojištěním).
        /// </summary>
        public int PocetPojistenych { get; set; }

        /// <summary>
        /// Seznam statistik nejčastějších typů pojištění podle počtu uzavřených smluv.
        /// </summary>
        public List<TypPojisteniStatistika> TopTypyPojisteni { get; set; } = new List<TypPojisteniStatistika>();
        public int PocetMladsi18 { get; internal set; }
        public int PocetStarsich18 { get; internal set; }

        // Nové vlastnosti pro statistiku podle pohlaví
        public int PocetMuzu { get; internal set; }
        public int PocetZen { get; internal set; }
        public int PocetJinych { get; internal set; }
    }

    /// <summary>
    /// Třída reprezentující statistiky pro konkrétní typ pojištění.
    /// Obsahuje informace o názvu pojištění, jeho předmětu a počtu uzavřených smluv.
    /// </summary>
    public class TypPojisteniStatistika
    {
        /// <summary>
        /// Název typu pojištění (např. životní pojištění, autopojištění).
        /// </summary>
        public string TypPojisteni { get; set; } = string.Empty;

        /// <summary>
        /// Konkrétní předmět pojištění (např. auto, dům, zdraví).
        /// </summary>
        public string PredmetPojisteni { get; set; } = string.Empty;

        /// <summary>
        /// Počet uzavřených smluv pro tento typ pojištění.
        /// </summary>
        public int PocetSmluv { get; set; }

        /// <summary>
        /// Počet pojištěnců mladších 18 let.
        /// </summary>
        public int PocetMladsi18 { get;  set; }

        /// <summary>
        /// Počet pojištěnců starších 18 let.
        /// </summary>
        public int PocetStarsich18 { get;  set; }

    


    }
}

using System.Diagnostics;
using EvidencePojisteni.Models;
using EvidencePojisteni.Models.ViewModels;
using EvidencePojisteni.Sluzby;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EvidencePojisteni.Controllers
{
    /// <summary>
    /// Kontroler pro domovskou stránku a obecné informace.
    /// </summary>
    public class DomaController : Controller
    {
        private readonly SluzbaStatistik _sluzbaStatistik;
        private readonly ILogger<DomaController> _logger;

        /// <summary>
        /// Konstruktor – injektuje službu statistik a logger.
        /// </summary>
        public DomaController(SluzbaStatistik sluzbaStatistik, ILogger<DomaController> logger)
        {
            _sluzbaStatistik = sluzbaStatistik;
            _logger = logger;
        }

        /* ================================================
           STATISTIKA – pøehled dat z databáze
        ================================================= */

        /// <summary>
        /// Akce pro zobrazení stránky se statistikami o systému.
        /// </summary>
        /// <returns>View se statistikami nebo pøesmìrování na chybovou stránku.</returns>
        public async Task<IActionResult> Statistika()
        {
            _logger.LogInformation("Zahájení naèítání statistik pro domovskou stránku.");

            try
            {
                var statistiky = await _sluzbaStatistik.ZiskatStatistikyAsync();

                // Statistiky byly naèteny bez chyby
                _logger.LogInformation("Statistiky byly úspìšnì naèteny.");
                return View(statistiky);
            }
            catch (Exception ex)
            {
                // Výjimka pøi naèítání statistik
                _logger.LogError(ex, "Chyba pøi naèítání statistik.");
                TempData["ChybovaZprava"] = "Nepodaøilo se naèíst statistiky. Zkuste to prosím pozdìji.";

                return RedirectToAction("Chyba");
            }
        }

        /* ================================================
           OCHRANA SOUKROMÍ – statická informaèní stránka
        ================================================= */

        /// <summary>
        /// Zobrazí stránku s informacemi o ochranì osobních údajù.
        /// </summary>
        public IActionResult OchranaSoukromi()
        {
            _logger.LogInformation("Pøístup na stránku Ochrana soukromí.");
            return View();
        }

        /* ================================================
           CHYBOVÁ STRÁNKA – v pøípadì výjimek
        ================================================= */

        /// <summary>
        /// Zobrazí chybovou stránku s informací o ID požadavku.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Chyba()
        {
            var idPozadavku = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // Výpis ID požadavku do logu pro ladìní
            _logger.LogWarning($"Zobrazení chybové stránky pro ID požadavku: {idPozadavku}");

            var model = new ChybovyViewModel { IdPozadavku = idPozadavku };
            return View(model);
        }
    }
}

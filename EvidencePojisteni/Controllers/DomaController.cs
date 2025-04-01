using System.Diagnostics;
using EvidencePojisteni.Models;
using EvidencePojisteni.Models.ViewModels;
using EvidencePojisteni.Sluzby;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EvidencePojisteni.Controllers
{
    /// <summary>
    /// Kontroler pro domovskou str�nku a obecn� informace.
    /// </summary>
    public class DomaController : Controller
    {
        private readonly SluzbaStatistik _sluzbaStatistik;
        private readonly ILogger<DomaController> _logger;

        /// <summary>
        /// Konstruktor � injektuje slu�bu statistik a logger.
        /// </summary>
        public DomaController(SluzbaStatistik sluzbaStatistik, ILogger<DomaController> logger)
        {
            _sluzbaStatistik = sluzbaStatistik;
            _logger = logger;
        }

        /* ================================================
           STATISTIKA � p�ehled dat z datab�ze
        ================================================= */

        /// <summary>
        /// Akce pro zobrazen� str�nky se statistikami o syst�mu.
        /// </summary>
        /// <returns>View se statistikami nebo p�esm�rov�n� na chybovou str�nku.</returns>
        public async Task<IActionResult> Statistika()
        {
            _logger.LogInformation("Zah�jen� na��t�n� statistik pro domovskou str�nku.");

            try
            {
                var statistiky = await _sluzbaStatistik.ZiskatStatistikyAsync();

                // Statistiky byly na�teny bez chyby
                _logger.LogInformation("Statistiky byly �sp�n� na�teny.");
                return View(statistiky);
            }
            catch (Exception ex)
            {
                // V�jimka p�i na��t�n� statistik
                _logger.LogError(ex, "Chyba p�i na��t�n� statistik.");
                TempData["ChybovaZprava"] = "Nepoda�ilo se na��st statistiky. Zkuste to pros�m pozd�ji.";

                return RedirectToAction("Chyba");
            }
        }

        /* ================================================
           OCHRANA SOUKROM� � statick� informa�n� str�nka
        ================================================= */

        /// <summary>
        /// Zobraz� str�nku s informacemi o ochran� osobn�ch �daj�.
        /// </summary>
        public IActionResult OchranaSoukromi()
        {
            _logger.LogInformation("P��stup na str�nku Ochrana soukrom�.");
            return View();
        }

        /* ================================================
           CHYBOV� STR�NKA � v p��pad� v�jimek
        ================================================= */

        /// <summary>
        /// Zobraz� chybovou str�nku s informac� o ID po�adavku.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Chyba()
        {
            var idPozadavku = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // V�pis ID po�adavku do logu pro lad�n�
            _logger.LogWarning($"Zobrazen� chybov� str�nky pro ID po�adavku: {idPozadavku}");

            var model = new ChybovyViewModel { IdPozadavku = idPozadavku };
            return View(model);
        }
    }
}

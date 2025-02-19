using EvidencePojisteni.Models;
using EvidencePojisteni.Sluzby;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EvidencePojisteni.Controllers
{
    /// <summary>
    /// Řadič pro správu pojistných událostí (CRUD).
    /// Přístup mají pojišťovatelé, kromě MojeUdalosti(), která je dostupná pojištěným.
    /// </summary>
    [Authorize(Roles = "Pojistitel")]
    public class PojistnaUdalostController : Controller
    {
        private readonly ISluzbaUdalosti _sluzba;

        /// <summary>
        /// Konstruktor controlleru – injektuje službu pro práci s pojistnými událostmi.
        /// </summary>
        public PojistnaUdalostController(ISluzbaUdalosti sluzba)
        {
            _sluzba = sluzba;
        }

        /* ================================
           SMAZÁNÍ POJISTNÉ UDÁLOSTI
        ================================== */

        /// <summary>
        /// Zobrazí potvrzovací stránku pro smazání události.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SmazaniUdalosti(int id, int pojistenecId)
        {
            var udalost = await _sluzba.ZiskatUdalostAsync(id, pojistenecId);
            if (udalost == null)
                return NotFound("Pojistná událost nebyla nalezena.");

            return View(udalost);
        }

        /// <summary>
        /// Potvrdí a provede smazání události.
        /// </summary>
        [HttpPost, ActionName("SmazaniUdalosti")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PotvrzeniSmazaniUdalosti(int id, int pojistenecId)
        {
            var vysledek = await _sluzba.SmazatUdalostAsync(id, pojistenecId);
            if (!vysledek)
                return NotFound("Pojistná událost nebyla nalezena nebo již byla odstraněna.");

            TempData["Zprava"] = "Pojistná událost byla úspěšně smazána.";
            return RedirectToAction("Detaily", "Pojistenec", new { id = pojistenecId });
        }

        /* ================================
           VYTVOŘENÍ POJISTNÉ UDÁLOSTI
        ================================== */

        /// <summary>
        /// Zobrazí formulář pro přidání nové události.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> VytvoritUdalost(int pojistenecId)
        {
            if (!await _sluzba.PojistenecExistujeAsync(pojistenecId))
                return NotFound("Pojištěnec nebyl nalezen.");

            ViewBag.PojistenecId = pojistenecId;
            return View(new PojistnaUdalost { PojistenecId = pojistenecId });
        }

        /// <summary>
        /// Zpracuje odeslaný formulář s novou pojistnou událostí.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VytvoritUdalost(PojistnaUdalost udalost)
        {
            if (!ModelState.IsValid)
                return View(udalost);

            var ulozeno = await _sluzba.UlozitUdalostAsync(udalost);
            if (!ulozeno)
                return NotFound("Pojištěnec neexistuje.");

            TempData["Zprava"] = "Pojistná událost byla přidána.";
            return RedirectToAction("Detaily", "Pojistenec", new { id = udalost.PojistenecId });
        }

        /* ================================
           ÚPRAVA POJISTNÉ UDÁLOSTI
        ================================== */

        /// <summary>
        /// Zobrazí formulář pro úpravu pojistné události.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EditovatUdalost(int pojistenecId, int id)
        {
            var udalost = await _sluzba.ZiskatUdalostAsync(id, pojistenecId);
            if (udalost == null)
                return NotFound();

            return View(udalost);
        }

        /// <summary>
        /// Zpracuje odeslaný formulář s úpravou události.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditovatUdalost(int pojistenecId, int id, PojistnaUdalost udalost)
        {
            if (id != udalost.IdPojistnaUdalost || pojistenecId != udalost.PojistenecId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(udalost);

            var aktualizovano = await _sluzba.AktualizovatUdalostAsync(udalost);
            if (!aktualizovano)
                return NotFound();

            TempData["Zprava"] = "Pojistná událost byla upravena.";
            return RedirectToAction("Detaily", "Pojistenec", new { id = udalost.PojistenecId });
        }

        /* ================================
           SEZNAM MOJICH UDÁLOSTÍ (POJIŠTĚNÝ)
        ================================== */

        /// <summary>
        /// Zobrazí seznam pojistných událostí aktuálně přihlášeného pojištěnce.
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> MojeUdalosti()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var udalosti = await _sluzba.ZiskatUdalostiUzivateleAsync(userId);
                return View(udalosti);
            }
            catch (Exception ex)
            {
                // Výpis chyby do konzole při vývoji (logování se hodí přidat později)
                Console.WriteLine($"Chyba v MojeUdalosti: {ex}");
                return RedirectToAction("Chyba", "Doma");
            }
        }
    }
}

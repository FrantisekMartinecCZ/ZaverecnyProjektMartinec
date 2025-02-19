using EvidencePojisteni.Databaze;
using EvidencePojisteni.Models;
using EvidencePojisteni.Models.ViewModels;
using EvidencePojisteni.Sluzby;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EvidencePojisteni.Controllers
{
    /// <summary>
    /// Kontroler pro správu pojištěnců – výpis, detail, vytvoření, úprava, smazání.
    /// </summary>
    [Authorize]
    public class PojistenecController : Controller
    {
        // Závislosti injektované pomocí konstruktoru
        private readonly DatabazovyKontext _db;
        private readonly UserManager<UzivatelAplikace> _userManager;
        private readonly ISluzbaPojistence _sluzba;
        private readonly ILogger<PojistenecController> _logger;

        /// <summary>
        /// Konstruktor kontroleru – zavedení služeb.
        /// </summary>
        public PojistenecController(
            DatabazovyKontext db,
            UserManager<UzivatelAplikace> userManager,
            ISluzbaPojistence sluzba,
            ILogger<PojistenecController> logger)
        {
            _db = db;
            _userManager = userManager;
            _sluzba = sluzba;
            _logger = logger;
        }

        /* -------------------------- Výpis pojištěnců -------------------------- */

        /// <summary>
        /// Vrací seznam všech pojištěnců.
        /// </summary>
        public async Task<IActionResult> SeznamPojistencu()
        {
            var seznam = await _db.Pojistenci.ToListAsync();
            return View(seznam);
        }

        /* -------------------------- Detail pojištěnce -------------------------- */

        /// <summary>
        /// Zobrazí detaily konkrétního pojištěnce podle ID. Přístupné pro admina a pojistitele.
        /// </summary>
        [Authorize(Roles = "Admin,Pojistitel")]
        public async Task<IActionResult> Detaily(int id)
        {
            var pojistenec = await _db.Pojistenci
                .Include(p => p.Pojisteni)
                .Include(p => p.PojistneUdalosti)
                .FirstOrDefaultAsync(p => p.IdPojistence == id);

            if (pojistenec == null)
                return NotFound();

            // Pojištěný uživatel může vidět pouze své vlastní údaje
            if (User.IsInRole("Pojištěný"))
            {
                string email = User.FindFirstValue(ClaimTypes.Email);
                if (email != pojistenec.Email)
                    return Forbid();
            }

            ViewData["Vek"] = _sluzba.VypocitatVek(pojistenec.DatumNarozeni);
            return View(pojistenec);
        }

        /// <summary>
        /// Zobrazí detaily právě přihlášeného pojištěnce. Používá se pro profil.
        /// </summary>
        [Authorize(Roles = "Pojištěný")]
        public async Task<IActionResult> MojeDetaily()
        {
            string uzivatelId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var pojistenec = await _db.Pojistenci
                .Include(p => p.Pojisteni)
                .Include(p => p.PojistneUdalosti)
                .FirstOrDefaultAsync(p => p.UzivatelID == uzivatelId);

            if (pojistenec == null)
                return NotFound();

            ViewData["Vek"] = _sluzba.VypocitatVek(pojistenec.DatumNarozeni);
            return View("Detaily", pojistenec); // použijeme view z Detaily
        }

        /* -------------------------- Vytvoření pojištěnce -------------------------- */

        /// <summary>
        /// Zobrazí formulář pro vytvoření nového pojištěnce.
        /// </summary>
        [Authorize(Roles = "Pojistitel")]
        public IActionResult VytvoreniPojistence() => View();

        /// <summary>
        /// Zpracuje odeslání formuláře pro vytvoření pojištěnce.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Pojistitel")]
        public async Task<IActionResult> VytvoreniPojistence(PojistenecViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Mladiství musejí mít zákonného zástupce
            if (model.DatumNarozeni.HasValue && _sluzba.VypocitatVek(model.DatumNarozeni) < 18)
            {
                if (string.IsNullOrWhiteSpace(model.JmenoZakonnehoZastupce) ||
                    string.IsNullOrWhiteSpace(model.PrijmeniZakonnehoZastupce))
                {
                    ModelState.AddModelError("", "Mladistvý musí mít zákonného zástupce.");
                    return View(model);
                }
            }

            var (uspech, chyby) = await _sluzba.VytvoritNovehoPojistenceAsync(model);

            if (uspech)
            {
                TempData["Zprava"] = "Pojištěnec úspěšně vytvořen.";
                return RedirectToAction("SeznamPojistencu");
            }

            foreach (var chyba in chyby)
                ModelState.AddModelError("", chyba);

            return View(model);
        }

        /* -------------------------- Editace pojištěnce -------------------------- */

        /// <summary>
        /// Zobrazí formulář pro editaci údajů pojištěnce.
        /// </summary>
        [Authorize(Roles = "Pojistitel")]
        public async Task<IActionResult> EditacePojistence(int? id)
        {
            if (id == null) return NotFound();
            var pojistenec = await _db.Pojistenci.FindAsync(id);
            if (pojistenec == null) return NotFound();

            return View(pojistenec);
        }

        /// <summary>
        /// Zpracuje úpravu pojištěnce po odeslání formuláře.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Pojistitel")]
        public async Task<IActionResult> EditacePojistence(int id, Pojistenec pojistenec)
        {
            if (id != pojistenec.IdPojistence)
                return NotFound();

            if (!ModelState.IsValid)
                return View(pojistenec);

            try
            {
                _db.Update(pojistenec);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(SeznamPojistencu));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.Pojistenci.Any(p => p.IdPojistence == id))
                    return NotFound();
                throw;
            }
        }

        /* -------------------------- Smazání pojištěnce -------------------------- */

        /// <summary>
        /// Zobrazí stránku s potvrzením o smazání pojištěnce.
        /// </summary>
        [Authorize(Roles = "Pojistitel")]
        public async Task<IActionResult> SmazaniPojistenec(int? id)
        {
            if (id == null) return NotFound();
            var pojistenec = await _db.Pojistenci.FirstOrDefaultAsync(p => p.IdPojistence == id);
            if (pojistenec == null) return NotFound();

            ViewData["Vek"] = _sluzba.VypocitatVek(pojistenec.DatumNarozeni);
            return View(pojistenec);
        }

        /// <summary>
        /// Zpracuje požadavek na smazání pojištěnce.
        /// </summary>
        [HttpPost, ActionName("SmazaniPojistenec")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Pojistitel")]
        public async Task<IActionResult> PotvrzeniSmazaniPojistence(int id)
        {
            var uspech = await _sluzba.SmazatPojistenceAsync(id);

            if (uspech)
            {
                TempData["Zprava"] = "Pojištěnec byl úspěšně smazán.";
            }
            else
            {
                TempData["Chyba"] = "Pojištěnce se nepodařilo smazat.";
            }

            return RedirectToAction("Statistika", "Doma");
        }
    }
}

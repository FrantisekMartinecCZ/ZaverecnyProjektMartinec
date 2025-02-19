using EvidencePojisteni.Models;
using EvidencePojisteni.Sluzby;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvidencePojisteni.Controllers
{
    /// <summary>
    /// Řadič pro správu pojistných smluv – přidávání, úpravy, mazání.
    /// Přístup mají pouze uživatelé s rolí „Pojistitel“.
    /// </summary>
    [Authorize(Roles = "Pojistitel")]
    public class PojisteniController : Controller
    {
        // Služba, která obsahuje logiku pro práci s pojištěními
        private readonly ISluzbaPojisteni _sluzba;

        /// <summary>
        /// Konstruktor controlleru, který přijímá službu pojištění přes dependency injection.
        /// </summary>
        /// <param name="sluzba">Implementace rozhraní ISluzbaPojisteni</param>
        public PojisteniController(ISluzbaPojisteni sluzba)
        {
            _sluzba = sluzba;
        }

        /* ======================================================
           PŘIDÁNÍ POJIŠTĚNÍ
        ====================================================== */

        /// <summary>
        /// Zobrazí formulář pro přidání nového pojištění danému pojištěnci.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> PridatPojisteni(int pojistenecId)
        {
            var pojistenec = await _sluzba.ZiskatPojistenceAsync(pojistenecId);
            if (pojistenec == null)
                return NotFound("Pojištěnec nebyl nalezen.");

            int vek = _sluzba.VypocitatVek(pojistenec.DatumNarozeni);

            // Předáme do view ID a věk pojištěnce
            ViewBag.PojistenecId = pojistenecId;
            ViewBag.Vek = vek;

            return View(new Pojisteni { PojistenecId = pojistenecId });
        }

        /// <summary>
        /// Zpracuje odeslaný formulář pro vytvoření nového pojištění.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PridatPojisteni(Pojisteni pojisteni)
        {
            var pojistenec = await _sluzba.ZiskatPojistenceAsync(pojisteni.PojistenecId);
            if (pojistenec == null)
            {
                ModelState.AddModelError("", "Pojištěnec nebyl nalezen.");
                return View(pojisteni);
            }

            int vek = _sluzba.VypocitatVek(pojistenec.DatumNarozeni);
            ViewBag.PojistenecId = pojisteni.PojistenecId;
            ViewBag.Vek = vek;

            if (!_sluzba.JeTypPojisteniPovolenyProMlade(pojisteni.TypPojisteni, vek))
            {
                ModelState.AddModelError("", "Osoby mladší 18 let mohou mít pouze Pojištění zdraví nebo Cestovní pojištění.");
                return View(pojisteni);
            }

            if (!ModelState.IsValid)
                return View(pojisteni);

            await _sluzba.UlozitNovePojisteniAsync(pojisteni);
            TempData["Zprava"] = "Pojištění bylo úspěšně přidáno.";

            return RedirectToAction("Detaily", "Pojistenec", new { id = pojisteni.PojistenecId });
        }

        /* ======================================================
           ÚPRAVA POJIŠTĚNÍ
        ====================================================== */

        /// <summary>
        /// Zobrazí formulář pro úpravu existujícího pojištění.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EditovatPojisteni(int pojistenecId, int id)
        {
            var pojisteni = await _sluzba.ZiskatPojisteniAsync(id, pojistenecId);
            if (pojisteni == null)
                return NotFound();

            ViewBag.PojistenecId = pojistenecId;
            return View(pojisteni);
        }

        /// <summary>
        /// Zpracuje odeslaný formulář pro úpravu pojištění.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditovatPojisteni(int pojistenecId, int id, Pojisteni pojisteni)
        {
            if (id != pojisteni.IdPojisteni || pojistenecId != pojisteni.PojistenecId)
            {
                ModelState.AddModelError("", "ID neodpovídají.");
                return View(pojisteni);
            }

            var pojistenec = await _sluzba.ZiskatPojistenceAsync(pojistenecId);
            if (pojistenec == null)
            {
                ModelState.AddModelError("", "Pojištěnec neexistuje.");
                return View(pojisteni);
            }

            int vek = _sluzba.VypocitatVek(pojistenec.DatumNarozeni);
            ViewBag.PojistenecId = pojistenecId;
            ViewBag.Vek = vek;

            if (!_sluzba.JeTypPojisteniPovolenyProMlade(pojisteni.TypPojisteni, vek))
            {
                ModelState.AddModelError("", "Osoby mladší 18 let mohou mít pouze Pojištění zdraví nebo Cestovní pojištění.");
                return View(pojisteni);
            }

            if (!ModelState.IsValid)
                return View(pojisteni);

            await _sluzba.UlozitZmenenePojisteniAsync(pojisteni);
            TempData["Zprava"] = "Pojištění bylo úspěšně upraveno.";

            return RedirectToAction("Detaily", "Pojistenec", new { id = pojistenecId });
        }

        /* ======================================================
           SMAZÁNÍ POJIŠTĚNÍ
        ====================================================== */

        /// <summary>
        /// Zobrazí potvrzení o smazání pojištění.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SmazaniPojisteni(int id, int pojistenecId)
        {
            var pojisteni = await _sluzba.ZiskatPojisteniAsync(id, pojistenecId);
            if (pojisteni == null)
                return NotFound("Pojištění nebylo nalezeno.");

            return View(pojisteni);
        }

        /// <summary>
        /// Zpracuje potvrzení smazání pojištění.
        /// </summary>
        [HttpPost, ActionName("SmazaniPojisteni")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PotvrzeniSmazaniPojisteni(int id, int pojistenecId)
        {
            var vysledek = await _sluzba.SmazatPojisteniAsync(id, pojistenecId);

            TempData[vysledek ? "Zprava" : "Chyba"] = vysledek
                ? "Pojištění bylo úspěšně smazáno."
                : "Pojištění již neexistuje nebo bylo smazáno.";

            return RedirectToAction("Detaily", "Pojistenec", new { id = pojistenecId });
        }
    }
}

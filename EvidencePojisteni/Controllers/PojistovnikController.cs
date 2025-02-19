using EvidencePojisteni.Models;
using EvidencePojisteni.Models.ViewModels;
using EvidencePojisteni.Sluzby;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvidencePojisteni.Controllers
{
    /// <summary>
    /// Controller pro správu pojišťovatelů. Přístup má pouze Admin.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class PojistovnikController : Controller
    {
        private readonly ISluzbaPojistovatelu _sluzba;

        /// <summary>
        /// Konstruktor controlleru, přijímá službu pojišťovatelů.
        /// </summary>
        public PojistovnikController(ISluzbaPojistovatelu sluzba)
        {
            _sluzba = sluzba;
        }

        /* ================================
           REGISTRACE POJIŠŤOVATELE
        ================================== */

        /// <summary>
        /// Zobrazí formulář pro registraci nového pojišťovatele.
        /// </summary>
        [HttpGet]
        public IActionResult RegistracePojistitele()
        {
            return View();
        }

        /// <summary>
        /// Zpracuje registraci nového pojišťovatele.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistracePojistitele(RegistracePojistovnikaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var vysledek = await _sluzba.VytvorPojistiteleAsync(model.Email, model.Heslo);

            if (vysledek.Succeeded)
            {
                TempData["Zprava"] = "Pojistitel byl úspěšně zaregistrován.";
                return RedirectToAction("Statistika", "Doma");
            }

            // Přidání chyb do modelu
            foreach (var chyba in vysledek.Errors)
            {
                ModelState.AddModelError("", chyba.Description);
            }

            return View(model);
        }

        /* ================================
           SEZNAM POJIŠŤOVATELŮ
        ================================== */

        /// <summary>
        /// Vrací seznam všech pojišťovatelů mimo admina.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SeznamPojistovatelu()
        {
            var pojistovatele = await _sluzba.ZiskatPojistovateleBezAdminuAsync();
            return View(pojistovatele);
        }

        /* ================================
           MAZÁNÍ POJIŠŤOVATELE
        ================================== */

        /// <summary>
        /// Smaže vybraného pojišťovatele (pokud není admin).
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SmazatPojistovatele(string id)
        {
            var vysledek = await _sluzba.SmazatUzivateleAsync(id);

            TempData[vysledek ? "Zprava" : "Chyba"] = vysledek
                ? "Pojišťovatel byl úspěšně smazán."
                : "Pojišťovatele nelze smazat nebo nebyl nalezen.";

            return RedirectToAction("SeznamPojistovatelu");
        }

        /* ================================
           ZMĚNA HESLA POJIŠŤOVATELE
        ================================== */

        /// <summary>
        /// Změní heslo zvolenému pojišťovateli.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ZmenitHeslo(string id, string noveHeslo)
        {
            if (string.IsNullOrWhiteSpace(noveHeslo) || noveHeslo.Length < 6)
            {
                TempData["Chyba"] = "Heslo musí mít alespoň 6 znaků.";
                return RedirectToAction("SeznamPojistovatelu");
            }

            var vysledek = await _sluzba.ZmenitHesloAsync(id, noveHeslo);

            TempData[vysledek ? "Zprava" : "Chyba"] = vysledek
                ? "Heslo bylo úspěšně změněno."
                : "Nepodařilo se změnit heslo.";

            return RedirectToAction("SeznamPojistovatelu");
        }
    }
}

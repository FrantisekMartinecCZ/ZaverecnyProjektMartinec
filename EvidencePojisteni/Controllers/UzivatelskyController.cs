using EvidencePojisteni.Databaze;
using EvidencePojisteni.Models;
using EvidencePojisteni.Models.ViewModels;
using EvidencePojisteni.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EvidencePojisteni.Controllers
{
    /// <summary>
    /// Řadič pro správu uživatelských operací, jako je přihlášení, registrace, změna hesla a odhlášení.
    /// </summary>
    public class UzivatelskyController : Controller
    {
        private readonly UserManager<UzivatelAplikace> _userManager;
        private readonly SignInManager<UzivatelAplikace> _signInManager;
        private readonly DatabazovyKontext _databazeKontext;

        /// <summary>
        /// Konstruktor, který inicializuje controller s požadovanými službami.
        /// </summary>
        public UzivatelskyController(DatabazovyKontext databazeKontext, UserManager<UzivatelAplikace> userManager, SignInManager<UzivatelAplikace> signInManager)
        {
            _databazeKontext = databazeKontext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /* ========================================
           PŘIHLÁŠENÍ
        ======================================== */

        /// <summary>
        /// GET akce pro zobrazení přihlašovacího formuláře.
        /// </summary>
        [HttpGet]
        public IActionResult Prihlaseni(string? navratURL = null)
        {
            ViewData["navratURL"] = navratURL;
            return View();
        }

        /// <summary>
        /// POST akce pro zpracování přihlašovacího formuláře.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Prihlaseni(Prihlaseni modelPrihlasovaciUdaje, string? navratURL = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var uzivatel = await _userManager.FindByEmailAsync(modelPrihlasovaciUdaje.Email);
                    if (uzivatel == null)
                    {
                        ModelState.AddModelError(string.Empty, "Neplatné přihlašovací údaje.");
                        return View(modelPrihlasovaciUdaje);
                    }

                    var vysledek = await _signInManager.PasswordSignInAsync(
                        uzivatel.UserName,
                        modelPrihlasovaciUdaje.Heslo,
                        isPersistent: true,
                        lockoutOnFailure: false
                    );

                    if (vysledek.Succeeded)
                        return RedirectToLocal(navratURL);

                    ModelState.AddModelError(string.Empty, "Neplatné přihlašovací údaje.");
                }
                return View(modelPrihlasovaciUdaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chyba v metodě Prihlaseni: " + ex);
                return RedirectToAction("Chyba", "Doma");
            }
        }

        /* ========================================
           ZMĚNA HESLA (uživatel)
        ======================================== */

        /// <summary>
        /// GET akce pro zobrazení formuláře pro změnu hesla.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult ZmenaHesla()
        {
            return View();
        }

        /// <summary>
        /// POST akce pro zpracování formuláře pro změnu hesla.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ZmenaHesla(ZmenaHeslaViewModel modelStareNoveHeslo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var uzivatel = await _userManager.GetUserAsync(User);
                    var vysledek = await _userManager.ChangePasswordAsync(uzivatel, modelStareNoveHeslo.StareHeslo, modelStareNoveHeslo.NoveHeslo);

                    if (vysledek.Succeeded)
                    {
                        TempData["Zprava"] = "Heslo bylo úspěšně změněno.";
                        return RedirectToAction("SeznamPojistencu", "Pojistenec");
                    }
                    foreach (var chyba in vysledek.Errors)
                        ModelState.AddModelError(string.Empty, chyba.Description);
                }
                return View(modelStareNoveHeslo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chyba v metodě ZmenaHesla: " + ex);
                return RedirectToAction("chyba", "Doma");
            }
        }

        /* ========================================
           ODHLÁŠENÍ
        ======================================== */

        /// <summary>
        /// Akce pro odhlášení aktuálně přihlášeného uživatele.
        /// </summary>
        public async Task<IActionResult> Odhlaseni()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Prihlaseni", "Uzivatelsky");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chyba v metodě Odhlaseni: " + ex);
                return RedirectToAction("Chyba", "Doma");
            }
        }

        /* ========================================
           ADMIN - Správa uživatelů
        ======================================== */

        /// <summary>
        /// Zobrazí seznam všech uživatelů s rolí "Pojištěný".
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SeznamUzivatelu()
        {
            var pojistenci = await _userManager.GetUsersInRoleAsync("Pojištěný");
            return View(pojistenci);
        }

        /// <summary>
        /// Smaže zvoleného uživatele.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SmazatUzivatele(string id)
        {
            var uzivatel = await _databazeKontext.Users.FindAsync(id);
            if (uzivatel != null)
            {
                _databazeKontext.Users.Remove(uzivatel);
                await _databazeKontext.SaveChangesAsync();
                TempData["Zprava"] = "Uživatel byl úspěšně smazán.";
            }
            else
            {
                TempData["Zprava"] = "Uživatel nebyl nalezen.";
            }
            return RedirectToAction("SeznamUzivatelu");
        }

        /// <summary>
        /// Umožňuje adminovi změnit heslo libovolnému uživateli.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ZmenitHeslo(string id, string noveHeslo)
        {
            var uzivatel = await _userManager.FindByIdAsync(id);
            if (uzivatel == null)
            {
                TempData["Chyba"] = "Uživatel nebyl nalezen.";
                return RedirectToAction("SeznamUzivatelu");
            }

            if (string.IsNullOrWhiteSpace(noveHeslo) || noveHeslo.Length < 6)
            {
                TempData["Chyba"] = "Heslo musí mít alespoň 6 znaků.";
                return RedirectToAction("SeznamUzivatelu");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(uzivatel);
            var vysledek = await _userManager.ResetPasswordAsync(uzivatel, token, noveHeslo);

            TempData[vysledek.Succeeded ? "Zprava" : "Chyba"] = vysledek.Succeeded
                ? $"Heslo uživatele {uzivatel.Email} bylo úspěšně změněno."
                : "Chyba při změně hesla.";

            return RedirectToAction("SeznamUzivatelu");
        }

        /* ========================================
           PŘÍSTUP ZAKÁZÁN
        ======================================== */

        [HttpGet]
        public IActionResult PristupZakazan(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /* ========================================
           POMOCNÁ METODA
        ======================================== */

        /// <summary>
        /// Přesměruje uživatele na bezpečnou URL nebo výchozí stránku.
        /// </summary>
        private IActionResult RedirectToLocal(string navratURL)
        {
            if (!string.IsNullOrEmpty(navratURL) && Url.IsLocalUrl(navratURL))
                return Redirect(navratURL);

            return RedirectToAction("OchranaSoukromi", "Doma");
        }
    }
}

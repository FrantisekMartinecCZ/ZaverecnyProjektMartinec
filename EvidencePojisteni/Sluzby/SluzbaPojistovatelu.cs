using EvidencePojisteni.Models;
using Microsoft.AspNetCore.Identity;

namespace EvidencePojisteni.Sluzby
{
    public class SluzbaPojistovatelu :ISluzbaPojistovatelu
    {
        private readonly UserManager<UzivatelAplikace> _userManager;

        public SluzbaPojistovatelu(UserManager<UzivatelAplikace> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> VytvorPojistiteleAsync(string email, string heslo)
        {
            var uzivatel = new UzivatelAplikace
            {
                UserName = email,
                Email = email,
                
            };

            var vysledek = await _userManager.CreateAsync(uzivatel, heslo);
            if (vysledek.Succeeded)
            {
                await _userManager.AddToRoleAsync(uzivatel, "Pojistitel");
            }

            return vysledek;
        }

        public async Task<List<UzivatelAplikace>> ZiskatPojistovateleBezAdminuAsync()
        {
            var pojistitele = await _userManager.GetUsersInRoleAsync("Pojistitel");
            return pojistitele.Where(u => !_userManager.IsInRoleAsync(u, "Admin").Result).ToList();
        }

        public async Task<bool> SmazatUzivateleAsync(string id)
        {
            var uzivatel = await _userManager.FindByIdAsync(id);
            if (uzivatel == null) return false;

            if (await _userManager.IsInRoleAsync(uzivatel, "Admin")) return false;

            var vysledek = await _userManager.DeleteAsync(uzivatel);
            return vysledek.Succeeded;
        }

        public async Task<bool> ZmenitHesloAsync(string id, string noveHeslo)
        {
            var uzivatel = await _userManager.FindByIdAsync(id);
            if (uzivatel == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(uzivatel);
            var vysledek = await _userManager.ResetPasswordAsync(uzivatel, token, noveHeslo);
            return vysledek.Succeeded;
        }
    }
}

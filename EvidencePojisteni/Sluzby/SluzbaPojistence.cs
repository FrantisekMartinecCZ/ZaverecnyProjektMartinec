using EvidencePojisteni.Databaze;
using EvidencePojisteni.Models;
using EvidencePojisteni.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EvidencePojisteni.Sluzby
{
    public class SluzbaPojistence :ISluzbaPojistence
    {
        private readonly DatabazovyKontext _kontext;
        private readonly UserManager<UzivatelAplikace> _userManager;

        public SluzbaPojistence(DatabazovyKontext kontext, UserManager<UzivatelAplikace> userManager)
        {
            _kontext = kontext;
            _userManager = userManager;
        }

        public int VypocitatVek(DateTime? datumNarozeni)
        {
            if (!datumNarozeni.HasValue)
                return 0;

            var dnes = DateTime.Today;
            int vek = dnes.Year - datumNarozeni.Value.Year;
            if (datumNarozeni.Value.Date > dnes.AddYears(-vek)) vek--;

            return vek;
        }

        public async Task<bool> SmazatPojistenceAsync(int id)
        {
            var pojistenec = await _kontext.Pojistenci
                .Include(p => p.Pojisteni)
                .Include(p => p.PojistneUdalosti)
                .FirstOrDefaultAsync(p => p.IdPojistence == id);

            if (pojistenec == null)
                return false;

            if (!string.IsNullOrEmpty(pojistenec.UzivatelID))
            {
                var uzivatel = await _userManager.FindByIdAsync(pojistenec.UzivatelID);
                if (uzivatel != null)
                    await _userManager.DeleteAsync(uzivatel);
            }

            if (pojistenec.Pojisteni?.Any() == true)
                _kontext.Pojisteni.RemoveRange(pojistenec.Pojisteni);

            if (pojistenec.PojistneUdalosti?.Any() == true)
                _kontext.PojistneUdalosti.RemoveRange(pojistenec.PojistneUdalosti);

            _kontext.Pojistenci.Remove(pojistenec);
            await _kontext.SaveChangesAsync();
            return true;
        }

        public async Task<(bool Uspech, List<string> Chyby)> VytvoritNovehoPojistenceAsync(PojistenecViewModel model)
        {
            var uzivatel = new UzivatelAplikace
            {
                UserName = model.Email,
                Email = model.Email
            };

            var vysledek = await _userManager.CreateAsync(uzivatel, model.Heslo);
            if (!vysledek.Succeeded)
                return (false, vysledek.Errors.Select(e => e.Description).ToList());

            await _userManager.AddToRoleAsync(uzivatel, "Pojištěný");

            var pojistenec = new Pojistenec
            {
                Jmeno = model.Jmeno,
                Prijmeni = model.Prijmeni,
                Email = model.Email,
                Pohlavi = model.Pohlavi,
                DatumNarozeni = model.DatumNarozeni,
                ZemePuvodu = model.ZemePuvodu,
                Telefon = model.Telefon,
                Ulice = model.Ulice,
                Mesto = model.Mesto,
                PSC = model.PSC,
                UzivatelID = uzivatel.Id,
                JmenoZakonnehoZastupce = model.JmenoZakonnehoZastupce,
                PrijmeniZakonnehoZastupce = model.PrijmeniZakonnehoZastupce
            };

            _kontext.Pojistenci.Add(pojistenec);
            await _kontext.SaveChangesAsync();

            return (true, new List<string>());
        }
    }
}

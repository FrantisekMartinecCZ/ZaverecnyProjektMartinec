using EvidencePojisteni.Databaze;
using EvidencePojisteni.Models;
using Microsoft.EntityFrameworkCore;

namespace EvidencePojisteni.Sluzby
{
    public class SluzbaPojistnychUdalosti : ISluzbaUdalosti
    {
        private readonly DatabazovyKontext _db;

        public SluzbaPojistnychUdalosti(DatabazovyKontext db)
        {
            _db = db;
        }

        public async Task<bool> PojistenecExistujeAsync(int pojistenecId)
        {
            return await _db.Pojistenci.AnyAsync(p => p.IdPojistence == pojistenecId);
        }

        public async Task<PojistnaUdalost?> ZiskatUdalostAsync(int id, int pojistenecId)
        {
            return await _db.PojistneUdalosti
                .FirstOrDefaultAsync(p => p.IdPojistnaUdalost == id && p.PojistenecId == pojistenecId);
        }

        public async Task<List<PojistnaUdalost>> ZiskatUdalostiUzivateleAsync(string uzivatelId)
        {
            return await _db.PojistneUdalosti
                .Include(u => u.Pojistenec)
                .Where(u => u.Pojistenec.UzivatelID == uzivatelId)
                .ToListAsync();
        }

        public async Task<bool> UlozitUdalostAsync(PojistnaUdalost udalost)
        {
            if (!await PojistenecExistujeAsync(udalost.PojistenecId)) return false;

            udalost.DatumNahlaseni = DateTime.Now;
            _db.PojistneUdalosti.Add(udalost);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AktualizovatUdalostAsync(PojistnaUdalost udalost)
        {
            var existujici = await ZiskatUdalostAsync(udalost.IdPojistnaUdalost, udalost.PojistenecId);
            if (existujici == null) return false;

            _db.Entry(existujici).CurrentValues.SetValues(udalost);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SmazatUdalostAsync(int id, int pojistenecId)
        {
            var udalost = await ZiskatUdalostAsync(id, pojistenecId);
            if (udalost == null) return false;

            _db.PojistneUdalosti.Remove(udalost);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

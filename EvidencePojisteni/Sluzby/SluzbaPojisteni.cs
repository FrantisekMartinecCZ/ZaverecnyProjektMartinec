using EvidencePojisteni.Databaze;
using EvidencePojisteni.Models;
using Microsoft.EntityFrameworkCore;

namespace EvidencePojisteni.Sluzby
{
    public class SluzbaPojisteni : ISluzbaPojisteni
    {
        private readonly DatabazovyKontext _db;

        public SluzbaPojisteni(DatabazovyKontext db)
        {
            _db = db;
        }

        public int VypocitatVek(DateTime? datumNarozeni)
        {
            if (!datumNarozeni.HasValue) return 0;

            var dnes = DateTime.Today;
            int vek = dnes.Year - datumNarozeni.Value.Year;
            if (datumNarozeni.Value.Date > dnes.AddYears(-vek)) vek--;

            return vek;
        }

        public bool JeTypPojisteniPovolenyProMlade(string typ, int vek)
        {
            var povoleneTypy = new List<string> { "Pojištění zdraví", "Cestovní pojištění" };
            return vek >= 18 || povoleneTypy.Contains(typ);
        }

        public async Task<Pojisteni?> ZiskatPojisteniAsync(int id, int pojistenecId)
        {
            return await _db.Pojisteni
                .FirstOrDefaultAsync(p => p.IdPojisteni == id && p.PojistenecId == pojistenecId);
        }

        public async Task<bool> PojistenecExistujeAsync(int id)
        {
            return await _db.Pojistenci.AnyAsync(p => p.IdPojistence == id);
        }

        public async Task<Pojistenec?> ZiskatPojistenceAsync(int id)
        {
            return await _db.Pojistenci.FindAsync(id);
        }

        public async Task UlozitNovePojisteniAsync(Pojisteni pojisteni)
        {
            _db.Pojisteni.Add(pojisteni);
            await _db.SaveChangesAsync();
        }

        public async Task UlozitZmenenePojisteniAsync(Pojisteni pojisteni)
        {
            _db.Pojisteni.Update(pojisteni);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> SmazatPojisteniAsync(int id, int pojistenecId)
        {
            var pojisteni = await ZiskatPojisteniAsync(id, pojistenecId);
            if (pojisteni == null)
                return false;

            _db.Pojisteni.Remove(pojisteni);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

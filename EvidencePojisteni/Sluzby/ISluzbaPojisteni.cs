using EvidencePojisteni.Models;

namespace EvidencePojisteni.Sluzby
{
    public interface ISluzbaPojisteni
    {
        int VypocitatVek(DateTime? datumNarozeni);
        bool JeTypPojisteniPovolenyProMlade(string typ, int vek);
        Task<Pojisteni?> ZiskatPojisteniAsync(int id, int pojistenecId);
        Task<bool> PojistenecExistujeAsync(int id);
        Task<Pojistenec?> ZiskatPojistenceAsync(int id);
        Task UlozitNovePojisteniAsync(Pojisteni pojisteni);
        Task UlozitZmenenePojisteniAsync(Pojisteni pojisteni);
        Task<bool> SmazatPojisteniAsync(int id, int pojistenecId);
    }
}

using EvidencePojisteni.Models.ViewModels;

namespace EvidencePojisteni.Sluzby
{
    public interface ISluzbaPojistence
    {
        int VypocitatVek(DateTime? datumNarozeni);
        Task<bool> SmazatPojistenceAsync(int id);
        Task<(bool Uspech, List<string> Chyby)> VytvoritNovehoPojistenceAsync(PojistenecViewModel model);
    }
}

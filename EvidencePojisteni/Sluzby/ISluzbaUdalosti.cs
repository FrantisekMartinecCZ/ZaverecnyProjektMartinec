using EvidencePojisteni.Models;

namespace EvidencePojisteni.Sluzby
{
    public interface ISluzbaUdalosti
    {
        Task<bool> PojistenecExistujeAsync(int pojistenecId);
        Task<PojistnaUdalost?> ZiskatUdalostAsync(int id, int pojistenecId);
        Task<List<PojistnaUdalost>> ZiskatUdalostiUzivateleAsync(string uzivatelId);
        Task<bool> UlozitUdalostAsync(PojistnaUdalost udalost);
        Task<bool> AktualizovatUdalostAsync(PojistnaUdalost udalost);
        Task<bool> SmazatUdalostAsync(int id, int pojistenecId);
    }
}

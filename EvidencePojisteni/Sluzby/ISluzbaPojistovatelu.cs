using EvidencePojisteni.Models;
using Microsoft.AspNetCore.Identity;

namespace EvidencePojisteni.Sluzby
{
    public interface ISluzbaPojistovatelu
    {
        Task<IdentityResult> VytvorPojistiteleAsync(string email, string heslo);
        Task<List<UzivatelAplikace>> ZiskatPojistovateleBezAdminuAsync();
        Task<bool> SmazatUzivateleAsync(string id);
        Task<bool> ZmenitHesloAsync(string id, string noveHeslo);
    }
}

using EvidencePojisteni.Databaze;
using EvidencePojisteni.Models;
using Microsoft.EntityFrameworkCore;

namespace EvidencePojisteni.Sluzby
{
    /// <summary>
    /// Služba pro získání statistik o pojištěncích, pojistných smlouvách a událostech.
    /// </summary>
    public class SluzbaStatistik
    {
        private readonly DatabazovyKontext _dbkontext;

        public SluzbaStatistik(DatabazovyKontext dbKontext)
        {
            _dbkontext = dbKontext;
        }

        /// <summary>
        /// Vrací souhrnný model se statistikami.
        /// </summary>
        public async Task<ZpravaStatistik> ZiskatStatistikyAsync()
        {
            return new ZpravaStatistik
            {
                PocetPojistencu = await ZiskatPocetPojistencuAsync(),
                PocetPojistnychUdalosti = await ZiskatPocetUdalostiAsync(),
                PocetPojisteni = await ZiskatPocetPojisteniAsync(),
                PocetPojistniku = await ZiskatPocetPojistnikuAsync(),
                PocetPojistenych = await ZiskatPocetPojistenychAsync(),
                PocetMladsi18 = await ZiskatPocetMladsi18Async(),
                PocetStarsich18 = await ZiskatPocetStarsich18Async(),
                PocetMuzu = await ZiskatPocetMuzuAsync(),
                PocetZen = await ZiskatPocetZenAsync(),
                PocetJinych = await ZiskatPocetJinychAsync(),
                TopTypyPojisteni = await ZiskatTopTypyPojisteniAsync()
            };
        }

        private async Task<int> ZiskatPocetPojistencuAsync() =>
            await _dbkontext.Pojistenci.CountAsync();

        private async Task<int> ZiskatPocetUdalostiAsync() =>
            await _dbkontext.PojistneUdalosti.CountAsync();

        private async Task<int> ZiskatPocetPojisteniAsync() =>
            await _dbkontext.Pojisteni.CountAsync();

        private async Task<int> ZiskatPocetPojistnikuAsync() =>
            await _dbkontext.Pojisteni.CountAsync(p => p.RolePojisteni == RoleVPojisteni.Pojistnik);

        private async Task<int> ZiskatPocetPojistenychAsync() =>
            await _dbkontext.Pojisteni.CountAsync(p => p.RolePojisteni == RoleVPojisteni.Pojisteny);

        private async Task<int> ZiskatPocetMladsi18Async() =>
            await _dbkontext.Pojistenci.CountAsync(p =>
                EF.Functions.DateDiffYear(p.DatumNarozeni, DateTime.Today) < 18);

        private async Task<int> ZiskatPocetStarsich18Async() =>
            await _dbkontext.Pojistenci.CountAsync(p =>
                EF.Functions.DateDiffYear(p.DatumNarozeni, DateTime.Today) >= 18);

        private async Task<int> ZiskatPocetMuzuAsync() =>
            await _dbkontext.Pojistenci.CountAsync(p => p.Pohlavi == Pohlavi.muž);

        private async Task<int> ZiskatPocetZenAsync() =>
            await _dbkontext.Pojistenci.CountAsync(p => p.Pohlavi == Pohlavi.žena);

        private async Task<int> ZiskatPocetJinychAsync() =>
            await _dbkontext.Pojistenci.CountAsync(p => p.Pohlavi == Pohlavi.jiné);

        private async Task<List<TypPojisteniStatistika>> ZiskatTopTypyPojisteniAsync() =>
            await _dbkontext.Pojisteni
                .GroupBy(p => p.TypPojisteni)
                .Select(g => new TypPojisteniStatistika
                {
                    TypPojisteni = g.Key,
                    PredmetPojisteni = g.First().PredmetPojisteni,
                    PocetSmluv = g.Count()
                })
                .OrderByDescending(stat => stat.PocetSmluv)
                .Take(10)
                .ToListAsync();
    }
}

using Microsoft.AspNetCore.Identity;

namespace EvidencePojisteni.Models
{
    /// <summary>
    /// Statická třída, která inicializuje role a vytváří výchozího administrátora.
    /// Tato třída se používá k "seedování" (naplnění) databáze rolemi a administrátorským účtem při startu aplikace.
    /// </summary>
    public static class RoleInitializer
    {
        /// <summary>
        /// Seeduje role a vytváří výchozího admina.
        /// Pokud role nebo admin účet neexistují, vytvoří je.
        /// </summary>
        /// <param name="roleManager">RoleManager pro správu rolí.</param>
        /// <param name="userManager">UserManager pro správu uživatelů.</param>
        /// <returns>Asynchronní úloha bez návratové hodnoty.</returns>
        public static async Task SeedRolesAndAdmin(RoleManager<IdentityRole> roleManager, UserManager<UzivatelAplikace> userManager)
        {
            /* ===== Vytvoření rolí, pokud neexistují ===== */

            if (!await roleManager.RoleExistsAsync("Pojistitel"))
            {
                await roleManager.CreateAsync(new IdentityRole("Pojistitel")); // Role pro zaměstnance pojišťovny
            }

            if (!await roleManager.RoleExistsAsync("Pojištěný"))
            {
                await roleManager.CreateAsync(new IdentityRole("Pojištěný")); // Role pro běžného uživatele (klient)
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin")); // Role pro správce systému
            }

            /* ===== Vytvoření výchozího admina, pokud neexistuje ===== */

            string adminEmail = "admin@pojisteniapp.cz";
            string adminPassword = "Admin1234!";

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var adminUser = new UzivatelAplikace
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                   // MusisZmenitHeslo = false // výchozí admin nemusí měnit heslo po prvním přihlášení
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                
            }
        }
    }
}

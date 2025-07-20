# ZaverecnyProjektMartinec

Tento projekt vznikl jako **závěrečný projekt** kurzu [Základy programování v C#](https://www.itnetwork.cz/csharp) na ITnetwork.cz. Jedná se o jednoduchou evidenci pojištění s využitím ASP.NET Core MVC.

## 📌 Funkce aplikace

- Správa pojištěnců (vytváření, úprava, mazání, zobrazení)
- Evidence pojištění (typ, částka, platnost)
- Správa pojistných událostí
- Databázový kontext přes Entity Framework Core
- Jednoduché ovládání přes webové rozhraní

## 💻 Použité technologie

- C# (.NET 6 / ASP.NET Core MVC)
- Entity Framework Core
- Razor Pages
- SQL Server (lokální)

## 🔧 Jak spustit projekt

1. Otevři soubor `EvidencePojisteni.sln` ve Visual Studiu.
2. Ujisti se, že je nastaven správný `DbContext` a connection string v `appsettings.json`.
3. Spusť migrace (případně ručně pomocí `Update-Database` v Package Manager Console).
4. Spusť aplikaci (`IIS Express` nebo `Kestrel`).
5. Otevři v prohlížeči `https://localhost:xxxx`.

## 📂 Struktura složek

- `Controllers` – kontrolery jednotlivých entit
- `Databaze` – modely, konfigurace a DbContext
- `Migrations` – EF Core migrace
- `Views` – Razor View šablony pro práci s daty

## 👨‍🎓 Autor

**František Martinec**  
GitHub: [FrantisekMartinecCZ](https://github.com/FrantisekMartinecCZ)

---

*Projekt slouží výhradně pro výukové účely.*

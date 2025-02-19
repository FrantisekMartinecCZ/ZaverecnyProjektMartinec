document.addEventListener("DOMContentLoaded", function () {
    // 🛠️ Možnosti předmětů pojištění
    const moznostiPredmetu = {
        "Vyber pojištění": ["Vyber pojištění"],
        "Pojištění majetku": ["Byt", "Dům", "Chata", "Vozidlo", "Cennosti"],
        "Pojištění odpovědnosti": ["Pracovní odpovědnost", "Občanská odpovědnost"],
        "Pojištění zdraví": ["Úrazové pojištění", "Nemocenské pojištění"],
        "Cestovní pojištění": ["Evropa", "Svět"],
        "Životní pojištění": ["Krátkodobé pojištění", "Dlouhodobé pojištění"],
        "Pojištění vozidel": ["Osobní vozidlo", "Motocykl", "Nákladní vozidlo"],
        "Pojištění podnikání": ["Malé podniky", "Střední podniky", "Velké podniky"],
        "Pojištění právní ochrany": ["Ochrana jednotlivců", "Ochrana rodiny", "Ochrana firem"]
    };

    const typPojisteniSelect = document.getElementById("typPojisteniSelect");
    const predmetPojisteniSelect = document.getElementById("predmetPojisteniSelect");

    // 🛠️ Naplnění seznamu typů pojištění
    if (typPojisteniSelect) {
        if (typPojisteniSelect.options.length === 0) {
            Object.keys(moznostiPredmetu).forEach(typ => {
                const option = document.createElement("option");
                option.value = typ;
                option.textContent = typ;
                typPojisteniSelect.appendChild(option);
            });
            NaplnPredmetyPoisteni("Vyber pojištění");
        }

        typPojisteniSelect.addEventListener("change", function () {
            NaplnPredmetyPoisteni(this.value);
        });
    }

    // 🛠️ Naplnění předmětů pojištění
    function NaplnPredmetyPoisteni(typ) {
        predmetPojisteniSelect.innerHTML = '';
        const vychozi = document.createElement("option");
        vychozi.value = "";
        vychozi.textContent = "Vyberte předmět pojištění";
        predmetPojisteniSelect.appendChild(vychozi);

        if (moznostiPredmetu[typ]) {
            moznostiPredmetu[typ].forEach(predmet => {
                const option = document.createElement("option");
                option.value = predmet;
                option.textContent = predmet;
                predmetPojisteniSelect.appendChild(option);
            });
        }
    }

    // 🛠️ Naplnění seznamu zemí
    const zemePuvoduSelect = document.getElementById('ZemePuvodu');
    if (zemePuvoduSelect) {
        zemePuvoduSelect.innerHTML = '';
        const zeme = ["Vyber zemi....", "Česká republika", "Slovensko", "Německo", "Rakousko", "Polsko", "Maďarsko", "Francie", "Velká Británie", "USA"];
        zeme.forEach(z => {
            const option = document.createElement('option');
            option.value = z;
            option.textContent = z;
            zemePuvoduSelect.appendChild(option);
        });
    }

  
    // 🛠️ Zobrazení zákonného zástupce
    const datumNarozeniInput = document.getElementById('datumNarozeni') || document.getElementById('DatumNarozeni');
    const zastupceSection = document.getElementById('zastupceSection');
    const zastupceSection2 = document.getElementById('zastupceSection2');
    const jmenoZastupce = document.getElementById('JmenoZakonnehoZastupce');
    const prijmeniZastupce = document.getElementById('PrijmeniZakonnehoZastupce');
    const errorMessage = document.getElementById('ageError');

    // 🎯 Výpočet věku
    function spocitejVek(datum) {
        const dnes = new Date();
        let vek = dnes.getFullYear() - datum.getFullYear();
        if (dnes.getMonth() < datum.getMonth() ||
            (dnes.getMonth() === datum.getMonth() && dnes.getDate() < datum.getDate())) {
            vek--;
        }
        return vek;
    }

    // 🎯 Aktualizace zobrazení sekce zástupce
    function aktualizujSekciZastupce() {
        const datum = new Date(datumNarozeniInput.value);
        if (!isNaN(datum.getTime())) {
            const vek = spocitejVek(datum);
            if (vek < 18) {
                zastupceSection.style.display = 'block';
                zastupceSection2.style.display = 'block';
                jmenoZastupce.value = jmenoZastupce.value || "Jméno zákonného zástupce";
                prijmeniZastupce.value = prijmeniZastupce.value || "Příjmení zákonného zástupce";
                errorMessage.textContent = `⚠️ Uživatel je mladší 18 let (${vek} let) – vyžaduje zákonného zástupce.`;
                console.warn(`⚠️ Uživatel je mladší 18 let (${vek} let) – zobrazena sekce zástupce.`);
            } else {
                zastupceSection.style.display = 'none';
                zastupceSection2.style.display = 'none';
                jmenoZastupce.value = '';
                prijmeniZastupce.value = '';
                errorMessage.textContent = "";
                console.log(`✅ Uživatel má ${vek} let – sekce zástupce skryta.`);
            }
        } else {
            errorMessage.textContent = "❌ Neplatné datum narození!";
        }
    }

    if (datumNarozeniInput) {
        aktualizujSekciZastupce();
        datumNarozeniInput.addEventListener('input', aktualizujSekciZastupce);
    }

    // 🛠️ Ošetření polí pro zákonného zástupce
    function nastavPlaceholderOdstraneni(input, vychoziText) {
        input.addEventListener('focus', () => {
            if (input.value === vychoziText) {
                input.value = '';
            }
        });

        input.addEventListener('blur', () => {
            if (input.value.trim() === '') {
                input.value = vychoziText;
            }
        });

        if (!input.value) {
            input.value = vychoziText;
        }
    }

    if (jmenoZastupce) {
        nastavPlaceholderOdstraneni(jmenoZastupce, "Jméno zákonného zástupce");
    }
    if (prijmeniZastupce) {
        nastavPlaceholderOdstraneni(prijmeniZastupce, "Příjmení zákonného zástupce");
    }
});

let showUrl = firebaseUrl + "/predstave/" + getParamValue("showList") + "/" + getParamValue("showId") + ".json";
let showData;

let editForm = document.getElementById("forma-predstava");
let delButton = document.getElementById("delete-button");


/** Popunjava formu za edit predstave iz Firebase */
function populateForm() {
    showData = sendRequestGET(showUrl, "Greška prilikom učitavanja predstave.");
    
    let title = document.getElementById("input-title");
    title.value = showData.naziv;
    let code = document.getElementById("input-code");
    code.value = showData.kod;
    let length = document.getElementById("input-length");
    length.value = showData.trajanje;
    let price = document.getElementById("input-price");
    price.value = showData.cena;
    let person = document.getElementById("input-person");
    person.value = showData.maxOsobe;
    let picture = document.getElementById("input-pic");
    picture.value = showData.slika;
    let short = document.getElementById("input-short");
    short.value = showData.kratakOpis;
    let long = document.getElementById("input-long");
    long.value = showData.opis;
    let radio = document.getElementById(showData.zanr);
    radio.checked = "checked";
}


/**
 * Dobavlja zauzete kodove predstava
 * @returns {Array} zauzeti kodovi
 */
function getUsedCodes() {
    let codes = [];
    let allShows = sendRequestGET(showsUrl, "Greška prilikom učitavanja predstava.");
    for (let groupId in allShows) {
        let group = allShows[groupId];

        for (let sh in group) {
            if (showData.kod != group[sh].kod) {
                codes.push(group[sh].kod);
            }
        }
    }
    return codes;
}

/** Regex validacija cene */
function validatePrice(price) {
    return /^[0-9]+[.]?[0-9]+$/.test(price);
}


populateForm();
let codes = getUsedCodes();


/** Event listener: popuni formu na reset */
editForm.addEventListener("reset", function (e) {
    e.preventDefault();
    populateForm();
});
/** Event listener: azurira predstavu nakon validacije */
editForm.addEventListener("submit", function (e) {
    e.preventDefault();

    let title = document.querySelector("#input-title").value.trim();
    let code = document.querySelector("#input-code").value;
    let length = document.querySelector("#input-length").value;
    let genre = document.querySelector('input[name=zanr]:checked').value;
    let price = document.querySelector("#input-price").value;
    let person = document.querySelector("#input-person").value;
    let pic = document.querySelector("#input-pic").value.trim();
    let short = document.querySelector("#input-short").value.trim();
    let long = document.querySelector("#input-long").value.trim();

    if (title == "") {
        let input = document.getElementById("input-title");
        input.setCustomValidity("Naziv predstave je obavezno polje.");
        input.reportValidity();
        return;
    }  if (!validateNumber(code)) {
        let input = document.getElementById("input-code");
        input.setCustomValidity("Šifra predstave nije validna.");
        input.reportValidity();
        return;
    } if (codes.includes(code)) {
        let input = document.getElementById("input-code");
        input.setCustomValidity("Predstava sa unesenom šifrom već postoji u bazi.");
        input.reportValidity();
        return;
    } if (!validateNumber(length)) {
        let input = document.getElementById("input-length");
        input.setCustomValidity("Trajanje predstave mora da bude pozitivan broj.");
        input.reportValidity();
        return;
    }  if (!validatePrice(price)) {
        let input = document.getElementById("input-price");
        input.setCustomValidity("Cena predstave nije validna.");
        input.reportValidity();
        return;
    } if (!validateNumber(person)) {
        let input = document.getElementById("input-person");
        input.setCustomValidity("Maksimalan broj osoba mora da bude pozitivan broj.");
        input.reportValidity();
        return;
    } if (pic == "") {
        let input = document.getElementById("input-pic");
        input.reportValidity();
        return;
    } if (short == "") {
        let input = document.getElementById("input-short");
        input.setCustomValidity("Kratak opis je obavezno polje.");
        input.reportValidity();
        return;
    } if (long == "") {
        let input = document.getElementById("input-long");
        input.setCustomValidity("Detaljan opis je obavezno polje.");
        input.reportValidity();
        return;
    }

    
    if (!confirm('Da li želite da snimite unesene podatke?')) return;

    showData = {
        cena: price, 
        kod: code, 
        kratakOpis: short,
        maxOsobe: person,
        naziv: title,
        ocena: showData.ocena,
        ocene: showData.ocene,
        opis: long,
        slika: pic,
        trajanje: length,
        zanr: genre
    };

    sendRequestPUT(showUrl, "Greška prilikom izmene predstave.", showData);
    window.location.reload();
});
delButton.addEventListener("click", function (e) {
    e.preventDefault();

    if (!confirm('Da li ste sigurni da želite da obrišete predstavu iz baze podataka?\nOvu akciju nije moguće poništiti.')) return; 

    sendRequestDELETE(showUrl, "Greška prilikom brisanja predstave.");
    window.location.href = "index.html";
});

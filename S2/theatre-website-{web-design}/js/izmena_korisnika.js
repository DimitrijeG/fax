let userUrl = firebaseUrl + "/korisnici/" + getParamValue("id") + ".json";
let originalUsername;
let user;

let editForm = document.getElementById("forma-korisnik");
let deactParagraph = document.getElementById("deact-user");
let deactButton = document.getElementById("deactivate-button");


/** Popunjava formu za edit korisnika iz Firebase */
function populateForm() {
    user = sendRequestGET(userUrl, "Greška prilikom učitavanja korisnika.");

    let username = document.getElementById("input-username");
    originalUsername = user.korisnickoIme;
    username.value = user.korisnickoIme;
    username.setCustomValidity('');
    let name = document.getElementById("input-name");
    name.value = user.ime;
    name.setCustomValidity('');
    let surname = document.getElementById("input-surname");
    surname.value = user.prezime;
    surname.setCustomValidity('');
    let email = document.getElementById("input-email");
    email.value = user.email;
    email.setCustomValidity('');
    let pass = document.getElementById("input-pass");
    pass.value = user.lozinka;
    pass.setCustomValidity('');
    let date = document.getElementById("input-date");
    date.value = user.datumRodjenja;
    date.setCustomValidity('');
    let address = document.getElementById("input-address");
    address.value = user.adresa;
    address.setCustomValidity('');
    let phone = document.getElementById("input-phone");
    phone.value = user.telefon;
    phone.setCustomValidity('');

    if (user.blokiran) {
        deactButton.innerText = "Aktiviraj";
        deactButton.classList.add("activate-button");
        deactParagraph.innerText = "Deaktiviran";
        deactParagraph.style.color = "#9D4E45";
    } else {
        deactButton.classList.add("deactivate-button");
        deactParagraph.innerText = "Aktiviran";
        deactParagraph.style.color = "#649657";
    }
}


populateForm();


/** Event listener: popunjava formu na dugme reset. */
editForm.addEventListener("reset", function (e) {
    e.preventDefault();
    user = populateForm();
});
/** Event listener: salje podatke u bazu na submit. */
editForm.addEventListener("submit", function (e) {
    e.preventDefault();

    let username = document.querySelector("#input-username").value.trim();
    let name = document.querySelector("#input-name").value.trim();
    let surname = document.querySelector("#input-surname").value.trim();
    let email = document.querySelector("#input-email").value.trim();
    let pass = document.querySelector("#input-pass").value.trim();
    let date = Date.parse(document.querySelector("#input-date").value);
    let address = document.querySelector("#input-address").value.trim();
    let phone = document.querySelector("#input-phone").value.trim();
    let today = new Date();

    if (username != originalUsername && usernames.includes(username)) {
        let input = document.getElementById("input-username");
        input.setCustomValidity("Korisnicko ime je zauzeto.");
        input.reportValidity();
        return;
    } else if (!validateUsername(username)) {
        let input = document.getElementById("input-username");
        input.setCustomValidity("Korisničko ime nije validno. Razmaci i specijalni karakteri su zabranjeni a minimalna dužina je 5.");
        input.reportValidity();
        return;
    } if (!validateText(name)) {
        let input = document.getElementById("input-name");
        input.setCustomValidity("Ime nije validno.");
        input.reportValidity();
        return;
    } if (!validateText(surname)) {
        let input = document.getElementById("input-surname");
        input.setCustomValidity("Prezime nije validno.");
        input.reportValidity();
        return;
    } if (!validateEmail(email)) {
        let input = document.getElementById("input-email");
        input.setCustomValidity("Uneta email adresa nije validna.");
        input.reportValidity();
        return;
    } if (!validatePassword(pass)) {
        let input = document.getElementById("input-pass");
        input.setCustomValidity("Lozinka mora da ima najmanje 5 karaktera i da sadrži barem jedno slovo i broj.");
        input.reportValidity();
        return;
    } if (date >= today) {
        let input = document.getElementById("input-date");
        input.setCustomValidity("Uneti datum rodjenja nije validan.");
        input.reportValidity();
        return;
    } if (address == "") {
        let input = document.getElementById("input-address");
        input.setCustomValidity("Adresa je obavezno polje.");
        input.reportValidity();
        return;
    } if (!validatePhone(phone)) {
        let input = document.getElementById("input-phone");
        input.setCustomValidity("Broj telefona nije validan.");
        input.reportValidity();
        return;
    }

    user = {
        adresa: address,
        datumRodjenja: (new Date(date)).toISOString().substring(0, 10),
        email: email,
        ime: name,
        korisnickoIme: username,
        lozinka: pass,
        prezime: surname,
        telefon: phone,
        blokiran: user.blokiran
    };

    if (!confirm('Da li želite da snimite unesene podatke?')) return;

    sendRequestPUT(userUrl, "Greška prilikom izmene korisnika.", user);
    window.location.reload();
});
/** Event listener: deaktivira ili aktivira korisnika. */
deactButton.addEventListener("click", function (e) {
    e.preventDefault();

    if (sessionStorage.length != 0 && user.korisnickoIme == JSON.parse(sessionStorage.user).korisnickoIme) {
        alert("Ne možete da deaktivirate sebe.");
        return
    }
    if (user.blokiran) {
        if (!confirm('Potvrđujete aktiviranje korisnika?')) return;
        user.blokiran = false;
    } else {
        if (!confirm('Da li ste sigurni da želite da deaktivirate korisnika?')) return;
        user.blokiran = true;
    }

    sendRequestPUT(userUrl, "Greška prilikom deaktiviranja/aktiviranja korisnika.", user);
    window.location.reload();
});

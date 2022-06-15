let firebaseUrl = "https://pozoriste-uns31-default-rtdb.europe-west1.firebasedatabase.app";
let usersUrl = firebaseUrl + "/korisnici.json";
let theatresUrl = firebaseUrl + "/pozorista.json";
let showsUrl = firebaseUrl + "/predstave.json";

let loggedUser = undefined;
let allUsers = [];
let usernames = [];

let login = document.getElementById("login");
let register = document.getElementById("registration");

let logout = document.getElementById("logout-button");
let userButton = document.getElementById("user-button");
let loginButton = document.getElementsByClassName("login-menu");
let registerButton = document.getElementsByClassName("registration-menu");
let closeLogin = document.getElementById("close-login");
let closeRegistration = document.getElementById("close-registration");

let loginForm = document.getElementById("login-form");
let registerForm = document.getElementById("register-form");

let allInputs = document.querySelectorAll("input");


/**
 * proverava stanje sesije:
 * - u slucaju da korisnik nije ulogovan dobavlja sva korisnicka imena, emailove i sifre za login
 * - u suprotnom sakriva dugmice za login i registraciju i prikazuje user button
 */
function checkSession() {
    if (sessionStorage.length != 0) {
        loggedUser = JSON.parse(sessionStorage.user);

        loginButton[0].style.display = "none";
        loginButton[1].style.display = "none";
        registerButton[0].style.display = "none";
        registerButton[1].style.display = "none";

        userButton.innerText = loggedUser.ime;
        userButton.style.display = "flex";
    } else {
        users = sendRequestGET(usersUrl, "Greška prilikom učitavanja korisnika.");

        for (let id in users) {
            let usr = users[id];
            allUsers.push(usr);
            usernames.push(usr.korisnickoIme);
        }
    }
}


/**
 * Popunjava liste u vidu kartica
 * @param {object} objects objekti za prikaz (pozorista/predstave)
 * @param {boolean} shows logicka vrednost da li se prikazuje pozoriste ili predstava
 * @param {string} listId id document elementa u koji se postavljaju elementi
 * @returns {}
 */
function populateCardList(objects, shows, listId) {
    list = document.getElementById(listId);

    for (let id in objects) {
        let currentItem = objects[id];
        let card = document.createElement("a");
        card.classList.add("card");

        let title = document.createElement("h2");
        title.innerText = currentItem.naziv;
        let paragraph = document.createElement("p");
        paragraph.classList.add("info");
        let showTag = document.createElement("p");
        showTag.classList.add("tag");

        if (shows) {
            paragraph.innerHTML = currentItem.kratakOpis;
            showTag.innerHTML = currentItem.zanr;

            let newHref = addParam('showList', getParamValue('showList'), 'predstava.html');
            newHref = addParam('showId', id, newHref);

            card.href = newHref;
        } else {
            paragraph.innerHTML = currentItem.adresa;

            let newHref = addParam('theatreId', id, 'pozoriste.html');
            newHref = addParam('showList', currentItem.idPredstava, newHref);

            card.href = newHref;
        }

        card.appendChild(title);
        card.appendChild(paragraph);
        if (shows) {
            card.appendChild(showTag);
        }

        list.appendChild(card);
    }
}


/**
 * Cita parametar iz Url-a po kljucu
 * @param {string} key kljuc parametra
 * @returns {string} vrednost parametra
 */
function getParamValue(key) {
    let location = decodeURI(window.location.toString());
    let index = location.indexOf("?") + 1;
    let subs = location.substring(index, location.length);
    let splitted = subs.split("&");

    for (i = 0; i < splitted.length; i++) {
        let s = splitted[i].split("=");
        let pName = s[0];
        let pValue = s[1];
        if (pName == key) {
            return pValue;
        }
    }
}
function removeParam(key, sourceURL) {
    let rtn = sourceURL.split("?")[0],
        param,
        params_arr = [],
        queryString = (sourceURL.indexOf("?") !== -1) ? sourceURL.split("?")[1] : "";
    if (queryString !== "") {
        params_arr = queryString.split("&");
        for (let i = params_arr.length - 1; i >= 0; i -= 1) {
            param = params_arr[i].split("=")[0];
            if (param === key) {
                params_arr.splice(i, 1);
            }
        }
        if (params_arr.length) rtn = rtn + "?" + params_arr.join("&");
    }
    return rtn;
}
function addParam(key, value, sourceURL) {
    let params = sourceURL.split("?")[1];
    if (!params) {
        sourceURL += '?' + key + '=' + value;
    } else {
        sourceURL += '&' + key + '=' + value;
    }
    return sourceURL
}


/**
 * Salje GET zahtev na url adresu i vraca response podatke
 * @param {string} url adresa
 * @param {string} fmessage poruka koju je potrebno ispisati u slucaju neuspelog zahteva
 * @param {boolean} [async=false] opcioni parametar sinhroni/asinhroni zahtev
 * @returns {object} podaci
 */
function sendRequestGET(url, fmessage, async = false) {
    let data;
    let request = new XMLHttpRequest();
    request.onreadystatechange = function (e) {
        if (this.readyState == 4) {
            if (this.status == 200) {
                data = JSON.parse(request.responseText);
            } else {
                alert(fmessage);
            }
        }
    };
    request.open("GET", url, async);
    request.send();
    return data;
}
/**
 * Salje POST zahtev na url adresu i vraca novokreirani ID
 * @param {string} url adresa
 * @param {string} fmessage poruka koju je potrebno ispisati u slucaju neuspelog zahteva
 * @param {object} data podaci koji se dodavaju
 * @param {boolean} [async=false] opcioni parametar sinhroni/asinhroni zahtev
 * @returns {string} ID kreiran od strane baze podataka
 */
function sendRequestPOST(url, fmessage, data, async = false) {
    let newId;
    let request = new XMLHttpRequest();
    request.onreadystatechange = function (e) {
        if (this.readyState == 4) {
            if (this.status == 200) {
                newId = JSON.parse(request.responseText).name;
            } else {
                alert(fmessage);
            }
        }
    };
    request.open("POST", url, async);
    request.send(JSON.stringify(data));
    return newId;
}
/**
 * Salje POST zahtev na url adresu i vraca novokreirani ID
 * @param {string} url adresa
 * @param {string} fmessage poruka koju je potrebno ispisati u slucaju neuspelog zahteva
 * @param {object} data podaci koji se azuriraju
 * @param {boolean} [async=false] opcioni parametar sinhroni/asinhroni zahtev
 */
function sendRequestPUT(url, fmessage, data, async = false) {
    let putRequest = new XMLHttpRequest();
    putRequest.onreadystatechange = function (e) {
        if (this.readyState == 4) {
            if (this.status != 200) {
                alert(fmessage);
            }
        }
    };
    putRequest.open("PUT", url, async);
    putRequest.send(JSON.stringify(data));
}
/**
 * Brise objekat na adresi
 * @param {string} url adresa za brisanje
 * @param {string} fmessage poruka koju je potrebno ispisati u slucaju neuspelog zahteva
 * @param {boolean} [async=false] opcioni parametar sinhroni/asinhroni zahtev
 */
function sendRequestDELETE(url, fmessage, async = false) {
    let request = new XMLHttpRequest();
    request.onreadystatechange = function (e) {
        if (this.readyState == 4) {
            if (this.status != 200) {
                alert(fmessage);
            }
        }
    };
    request.open("DELETE", url, async);
    request.send();
}


/**
 * Funkcije za validaciju koje koriste regex
 * @param {string} text za validaciju
 * @returns {boolean} rezultat validacije (true - validno)
 */
function validateText(text) {
    return /^[a-zA-ZčćžđšČĆŽĐŠ]+$/.test(text);
}
function validateNumber(num) {
    return /^[0-9]+$/.test(num);
}
function validateUsername(user) {
    return /^(?=.{5,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$/.test(user);
}
function validateEmail(mail) {
    return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail);
}
function validatePassword(pw) {
    return /[A-Za-z]/.test(pw) &&
        /[0-9]/.test(pw) &&
        pw.length > 4;
}
function validatePhone(phone) {
    return phone.length > 7 && /^[+]?[0-9]+$/.test(phone);
}


for (let i = 0; i < allInputs.length; i++) {
    allInputs[i].setAttribute('onblur', "setCustomValidity('')");
    allInputs[i].setAttribute('oninput', "setCustomValidity('')");
    allInputs[i].setAttribute('onchange', "setCustomValidity('')");
}

checkSession();


/** Event listener: sakriva forme za prijavu i registraciju na pritisnut Esc */
document.addEventListener('keydown', function (event) {
    if (event.key === "Escape") {
        login.style.display = "none";
        register.style.display = "none";
    }
});
/** Event listener: pokazuje formu za prijavu */
for (let i = 0; i < 2; i++) {
    loginButton[i].addEventListener("click", function () {
        login.style.display = "block";
    
        openRegister = document.getElementById("open-registration");
        openRegister.addEventListener("click", function () {
            login.style.display = "none";
            register = document.getElementById("registration");
            register.style.display = "block";
        });
    });
    /** Event listener: pokazuje formu za registraciju */
    registerButton[i].addEventListener("click", function () {
        register.style.display = "block";
    });
}
/** Event listener: zatvara prozor prijave */
closeLogin.addEventListener("click", function () {
    login.style.display = "none";
});
/** Event listener: zatvara prozor registracije */
closeRegistration.addEventListener("click", function () {
    register.style.display = "none";
});
/** Event listener: toggle pokazuje dugme za odjavu */
userButton.addEventListener("click", function () {
    if (logout.style.display == "flex") {
        logout.style.display = "none";
    } else {
        logout.style.display = "flex";
    }
});
/** Event listener: dugme za odjavu */
logout.addEventListener("click", function () {
    if (!confirm('Da li ste sigurni da želite da se odjavite?')) return;

    sessionStorage.removeItem('user');
    window.location.reload();
});
/** Event listener: prijavljuje korisnika posle validacija unosa */
loginForm.addEventListener("submit", function (e) {
    e.preventDefault();

    let inputUser = document.getElementById("input-user");
    let inputPass = document.getElementById("input-passwd");

    let user = document.querySelector("#input-user").value.trim();
    let index = -1;
    for (let i = 0; i < allUsers.length; i++) {
        if (user == allUsers[i].korisnickoIme || user == allUsers[i].email) {
            index = i;
        }
    }

    let pass = document.querySelector("#input-passwd").value.trim();
    if (index == -1) {
        inputUser.setCustomValidity("Neispravno korisničko ime ili email.");
        inputUser.reportValidity();
        return;
    } else if (allUsers[index].blokiran) {
        inputUser.setCustomValidity("Korisnik sa unetim korisničkim imenom je deaktiviran.");
        inputUser.reportValidity();
        return;
    } else if (pass != allUsers[index].lozinka) {
        inputPass.setCustomValidity("Pogrešna lozinka.");
        inputPass.reportValidity();
        return;
    }

    alert("Prijava uspešna!\nPozdrav " + allUsers[index].ime);

    sessionStorage.setItem('user', JSON.stringify(allUsers[index]));
    window.location.reload();
});
/** Event listener: registruje korisnika posle validacija unosa */
registerForm.addEventListener("submit", function (e) {
    e.preventDefault();

    let username = document.getElementById("korisnicko").value.trim();
    let name = document.getElementById("ime").value.trim();
    let surname = document.getElementById("prezime").value.trim();
    let email = document.getElementById("email").value.trim();
    let pass = document.getElementById("pass").value.trim();
    let date = Date.parse(document.getElementById("datum-rodj").value);
    let address = document.getElementById("adresa").value.trim();
    let phone = document.getElementById("telefon").value.trim();
    let today = new Date();

    if (usernames.includes(username)) {
        let input = document.getElementById("korisnicko");
        input.setCustomValidity("Korisnicko ime je zauzeto.");
        input.reportValidity();
        return;
    } else if (!validateUsername(username)) {
        let input = document.getElementById("korisnicko");
        input.setCustomValidity("Korisničko ime nije validno. Razmaci i specijalni karakteri su zabranjeni a minimalna dužina je 5.");
        input.reportValidity();
        return;
    } if (!validateText(name)) {
        let input = document.getElementById("ime");
        input.setCustomValidity("Ime nije validno.");
        input.reportValidity();
        return;
    } if (!validateText(surname)) {
        let input = document.getElementById("prezime");
        input.setCustomValidity("Prezime nije validno.");
        input.reportValidity();
        return;
    } if (!validateEmail(email)) {
        let input = document.getElementById("email");
        input.setCustomValidity("Uneta email adresa nije validna.");
        input.reportValidity();
        return;
    } if (!validatePassword(pass)) {
        let input = document.getElementById("pass");
        input.setCustomValidity("Lozinka mora da ima najmanje 5 karaktera i da sadrži barem jedno slovo i broj.");
        input.reportValidity();
        return;
    } if (!date || date >= today) {
        let input = document.getElementById("datum-rodj");
        input.setCustomValidity("Uneti datum rodjenja nije validan.");
        input.reportValidity();
        return;
    } if (address == "") {
        let input = document.getElementById("adresa");
        input.setCustomValidity("Adresa je obavezno polje.");
        input.reportValidity();
        return;
    } if (!validatePhone(phone)) {
        let input = document.getElementById("telefon");
        input.setCustomValidity("Broj telefona nije validan.");
        input.reportValidity();
        return;
    }

    newUser = {
        adresa: address,
        datumRodjenja: (new Date(date)).toISOString().substring(0, 10),
        email: email,
        ime: name,
        korisnickoIme: username,
        lozinka: pass,
        prezime: surname,
        telefon: phone,
        blokiran: false
    };

    sendRequestPOST(usersUrl, "Greška prilikom registracije korisnika.", newUser);
    alert("Registracija uspešna!\nDobro došli " + name);

    sessionStorage.setItem('user', JSON.stringify(newUser));
    window.location.reload();
});

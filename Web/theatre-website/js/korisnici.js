let table = document.getElementById("users-table");
let users = sendRequestGET(usersUrl, "Greška prilikom učitavanja korisnika.");

for (let id in users) {
    let user = users[id];
    let newRow = document.createElement("a");
    newRow.classList.add("table-row")
    newRow.href = "korisnik.html?id=" + id;

    let username = document.createElement("div");
    username.classList.add("table-cell");
    username.innerText = user.korisnickoIme
    let name = document.createElement("div");
    name.classList.add("table-cell");
    name.innerText = user.ime
    let surname = document.createElement("div");
    surname.classList.add("table-cell");
    surname.innerText = user.prezime
    let email = document.createElement("div");
    email.classList.add("table-cell");
    email.innerText = user.email
    let phone = document.createElement("div");
    phone.classList.add("table-cell");
    phone.innerText = user.telefon
    let pass = document.createElement("div");
    pass.classList.add("table-cell");
    pass.innerText = user.lozinka
    let date = document.createElement("div");
    date.classList.add("table-cell");
    date.innerText = user.datumRodjenja
    let adress = document.createElement("div");
    adress.classList.add("table-cell");
    adress.innerText = user.adresa

    newRow.appendChild(username)
    newRow.appendChild(name)
    newRow.appendChild(surname)
    newRow.appendChild(email)
    newRow.appendChild(phone)
    newRow.appendChild(pass)
    newRow.appendChild(date)
    newRow.appendChild(adress)

    table.appendChild(newRow);
}

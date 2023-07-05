let userUrl = firebaseUrl + "/korisnici/" + getParamValue("id") + ".json";

let user = sendRequestGET(userUrl, "Greška prilikom učitavanja korisnika.");

let username = document.getElementById("data-username");
username.innerText = user.korisnickoIme;
let name = document.getElementById("data-name");
name.innerText = user.ime;
let surname = document.getElementById("data-surname");
surname.innerText = user.prezime;
let email = document.getElementById("data-email");
email.innerText = user.email;
let phone = document.getElementById("data-phone");
phone.innerText = user.telefon;
let pass = document.getElementById("data-pass");
pass.innerText = user.lozinka;
let date = document.getElementById("data-date");
date.innerText = user.datumRodjenja;
let adress = document.getElementById("data-adress");
adress.innerText = user.adresa;

let editButton = document.getElementById("edit-button");
editButton.href = "izmena_korisnika.html?id=" + getParamValue("id");

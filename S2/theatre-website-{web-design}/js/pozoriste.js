let theatreUrl = firebaseUrl + "/pozorista/" + getParamValue("theatreId") + ".json";
let theatreShowsUrl = firebaseUrl + "/predstave/" + getParamValue("showList") + ".json";

mainBanner = document.getElementById("main-theatre-banner");
let theatreData = sendRequestGET(theatreUrl, "Greška prilikom učitavanja pozorišta.");

let img = document.createElement("img");
img.src = theatreData.slika;
img.alt = "theatre image";
img.classList.add("main-banner-image");

let theatreInfo = document.createElement("div");
theatreInfo.classList.add("theatre-info");

let title = document.createElement("h1");
title.innerText = theatreData.naziv;
let hr = document.createElement("hr");
hr.classList.add("black-hr");
let address = document.createElement("p");
address.innerText = theatreData.adresa;
let showNum = document.createElement("p");
showNum.innerText = "Broj predstava: " + theatreData.brojPredstava;

theatreInfo.appendChild(title);
theatreInfo.appendChild(hr);
theatreInfo.appendChild(address);
theatreInfo.appendChild(showNum);
mainBanner.appendChild(img);
mainBanner.appendChild(theatreInfo);


let shows = sendRequestGET(theatreShowsUrl, "Greška prilikom učitavanja predstava.");
populateCardList(shows, true, "show-card-list");

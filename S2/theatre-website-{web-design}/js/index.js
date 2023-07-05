
let theatres = sendRequestGET(theatresUrl, "Greška prilikom učitavanja pozorišta.");
populateCardList(theatres, false, "theatres-card-list");  // popunjava listu pozorista

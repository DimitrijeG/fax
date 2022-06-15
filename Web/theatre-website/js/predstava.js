let showUrl = firebaseUrl + "/predstave/" + getParamValue("showList") + "/" + getParamValue("showId") + ".json";
let showData;


/**
 * kreira kontejner za komentar sa svim odgovorima
 * @param {object} comment JSON podaci komentara
 * @param {string} path adresa komentara u bazi
 * @returns {object} novi kontejner komentara
 */
function newCommentContainer(comment, path) {
    let commentContainer = document.createElement("div");
    commentContainer.classList.add("comment-container");
    let commentCard = document.createElement("div");
    commentCard.classList.add("comment-card");

    // ime i prezime korisnika
    let h3 = document.createElement("h3");
    h3.classList.add("comment-title");
    h3.innerText = comment.korisnik.ime;
    // telo komentara
    let p = document.createElement("p");
    p.innerText = comment.text;

    let commentFooter = document.createElement("div");
    commentFooter.classList.add("comment-footer");
    let like = document.createElement("a");  // dugme za lajkovanje
    like.classList.add("like");
    like.innerHTML = "<span class='fa fa-thumbs-up'> <span>" + comment.lajkovi + "</span></span> ";
    let reply = document.createElement("a");  // dugme za unosenje novog komentara na trenutni
    reply.classList.add("reply");
    if (comment.odgovori) {
        // prikaz broja komentara na trenutni
        reply.innerText = "Odgovori " + Object.keys(comment.odgovori).length;
    } else {
        reply.innerText = "Odgovori 0";
    }
    let deleteComm = document.createElement("a");  // dugme za brisanje
    deleteComm.classList.add("deleteComm", "reply");
    deleteComm.innerHTML = "<span class='fa fa-trash-o'> <span>" + "Obriši" + "</span></span> ";
    if (sessionStorage.length != 0 && comment.korisnik.korisnickoIme == JSON.parse(sessionStorage.user).korisnickoIme) {
        deleteComm.style.display = "block";
        like.style.display = "none";
    } else {
        deleteComm.style.display = "none";
    }

    // div za unos novog komentara (nevidljiv je dok se ne klikne na 'reply')
    let inputReply = document.createElement("div");
    inputReply.classList.add("input-comment");

    let textArea = document.createElement("textarea");
    textArea.type = "text";
    textArea.classList.add("comment-card");
    textArea.rows = "3";
    textArea.placeholder = "Dodaj komentar...";
    textArea.setAttribute('onblur', "setCustomValidity('')");
    textArea.setAttribute('oninput', "setCustomValidity('')");
    textArea.setAttribute('onchange', "setCustomValidity('')");
    // dugme za odustanak (brise value 'textArea' i krije inputReply)
    let cancButton = document.createElement("button");
    cancButton.classList.add("styled-button");
    cancButton.style.marginRight = "4px";
    cancButton.innerText = "Odustani";
    // dugme za objavu
    let submitButton = document.createElement("button");
    submitButton.classList.add("styled-button");
    submitButton.innerText = "Pošalji";

    inputReply.appendChild(textArea);
    inputReply.appendChild(cancButton);
    inputReply.appendChild(submitButton);
    inputReply.style.display = "none";


    like.addEventListener("click", function () {
        comment.lajkovi += 1;

        like.style.color = "#1d65ff";
        like.style.cursor = "pointer";
        like.style.opacity = "1";
        like.innerHTML = "<span class='fa fa-thumbs-up'> <span>" + comment.lajkovi + "</span></span> ";

        updateDB(path + '.json', comment, "Greška prilikom dodavanja komentara");
    });
    reply.addEventListener("click", function () {
        if (inputReply.style.display == "none") {
            inputReply.style.display = "block";
        } else {
            inputReply.style.display = "none";
        }
    });
    cancButton.addEventListener("click", function () {
        textArea.value = "";
        inputReply.style.display = "none";
    });
    submitButton.addEventListener("click", function () {
        if (textArea.value == "") {
            textArea.setCustomValidity("Komentar koji šaljete ne može da bude prazan.");
            textArea.reportValidity();
            return;
        }
        inputReply.style.display = "none";


        korisnik = {'ime': "Anonymous", 'korisnickoIme': 'anonymous'};
        if (sessionStorage.length != 0) {
            currentUser = JSON.parse(sessionStorage.user);
            korisnik = {
                'ime': currentUser.ime + ' ' + currentUser.prezime,
                'korisnickoIme': currentUser.korisnickoIme
            }
        }

        newComm = {
            korisnik: korisnik,
            text: textArea.value,
            lajkovi: 0,
            odgovori: undefined
        }

        let newId = sendRequestPOST(path + '/odgovori.json', "Greška prilikom dodavanja komentara", newComm);
        commentContainer.appendChild(newCommentContainer(newComm, path + "/odgovori/" + newId));
        textArea.value = "";
    });
    deleteComm.addEventListener("click", function () {
        commentContainer.parentNode.removeChild(commentContainer);
        sendRequestDELETE(path + '.json', "Greška prilikom brisanja komentara.");
    });



    commentFooter.appendChild(deleteComm);
    commentFooter.appendChild(like);
    commentFooter.appendChild(reply);

    commentCard.appendChild(h3);
    commentCard.appendChild(p);
    commentCard.appendChild(commentFooter);
    commentContainer.appendChild(commentCard);
    commentContainer.appendChild(inputReply);

    
    if (comment.odgovori) {
        // dodavanje odgovora pomocu rekurzije
        for (let id in comment.odgovori) {
            if (comment.odgovori[id]) {
                let newReply = newCommentContainer(comment.odgovori[id], path + '/odgovori/' + id);
                commentContainer.appendChild(newReply);
            }
        }
    }
    return commentContainer;
}


/** kreira sekciju za komentare */
function displayCommentSection() {
    let path = showUrl.substring(0, showUrl.length - 5) + '/komentari';

    let commentsWrapper = document.getElementById("comments-wrapper");
    let cancelFirstComment = document.getElementById("cancel-first-comment");
    let submitFirstComment = document.getElementById("submit-first-comment");
    

    cancelFirstComment.addEventListener("click", function () {
        document.getElementById("first-comment-text").value = "";
    });
    submitFirstComment.addEventListener("click", function () {
        let textArea = document.getElementById("first-comment-text");
        textArea.setAttribute('onblur', "setCustomValidity('')");
        textArea.setAttribute('oninput', "setCustomValidity('')");
        textArea.setAttribute('onchange', "setCustomValidity('')");
        
        if (textArea.value == "") {
            textArea.setCustomValidity("Komentar koji šaljete ne može da bude prazan.");
            textArea.reportValidity();
            return;
        }

        korisnik = {'ime': "Anonymous", 'korisnickoIme': 'anonymous'};
        if (sessionStorage.length != 0) {
            currentUser = JSON.parse(sessionStorage.user);
            korisnik = {
                'ime': currentUser.ime + ' ' + currentUser.prezime,
                'korisnickoIme': currentUser.korisnickoIme
            }
        }

        newComm = {
            korisnik: korisnik,
            text: textArea.value,
            lajkovi: 0,
            odgovori: undefined
        }

        let newId = sendRequestPOST(path + '.json', "Greška prilikom dodavanja komentara", newComm);
        commentsWrapper.appendChild(newCommentContainer(newComm, path + "/" + newId));
        textArea.value = "";
    });


    if (showData.komentari) {
        // poziva funkciju za kreiranje kontejnera za svaki komentar
        for (let id in showData.komentari) { 
            let comment = newCommentContainer(showData.komentari[id], path + '/' + id);
            commentsWrapper.appendChild(comment);
        }
    }
}


showData = sendRequestGET(showUrl, "Greška prilikom učitavanja predstave.");

title = document.getElementById("show-title");
title.innerText = showData.naziv
editLink = document.getElementById("show-edit-link");

let newHref = addParam('showList', getParamValue('showList'), 'izmena_predstave.html');
newHref = addParam('showId', getParamValue('showId'), newHref);
editLink.href = newHref;

let code = document.getElementById("show-code");
code.innerText = showData.kod;
let length = document.getElementById("show-length");
length.innerText = showData.trajanje;
let genre = document.getElementById("show-genre");
genre.innerText = showData.zanr;
let price = document.getElementById("show-price");
price.innerText = showData.cena;
let personNum = document.getElementById("show-person");
personNum.innerText = showData.maxOsobe;

let about = document.getElementById("about-show");
about.innerText = showData.opis;
let showBanner = document.getElementById("show-banner");
showBanner.src = showData.slika;


// sekcija za ocene
let rating = Math.round(showData.ocena);
for (let i = 1; i <= rating; i++) {
    let star = document.getElementsByClassName("star" + i);
    star[0].classList.add("checked");
}
document.getElementById("average-score").innerText = showData.ocena;

let rateNum = 0;
for (let i = 0; i < 5; i++) {
    let currentNum = showData.ocene[i];
    rateNum += currentNum;
}
document.getElementById("score-num").innerText = rateNum;

for (let i = 1; i <= 5; i++) {
    let currentNum = showData.ocene[i - 1];
    let percentage = currentNum / rateNum * 100;
    document.getElementById("text-" + i).innerText = currentNum.toString();
    document.getElementById("bar-" + i).style.width = percentage.toString() + "%";
}


// sekcija za komentare
displayCommentSection();

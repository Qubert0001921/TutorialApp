//window.addEventListener("resize", e => {
//    console.log(window.innerWidth);
//});

function redirectToPage(page) {
    window.location.href = page;
}

document.querySelectorAll(".section").forEach(section => {
    let btnExpand = section.querySelector(".btn-section-expand");
    let topics = section.querySelector(".topics");

    if (btnExpand != null && topics != null) {
        section.addEventListener("click", e => {
            if (topics.style.display == "none") {
                topics.style.display = "block";
            } else {
                topics.style.display = "none";
            }
        })
    }

})


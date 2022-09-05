//window.addEventListener("resize", e => {
//    console.log(window.innerWidth);
//});

//document.addEventListener("resize", e => {
//    if (window.innerWidth <= 540) {
//        let menu_options = document.querySelector("#menu-options");
//        console.log(document.querySelector("#menu-options"));
//    }
//})

//window.onresize = function () {
//    console.log(document.querySelector("#menu-options"))
//}
let addTopicSectionId = "";
let addTopicTutorialId = "";
let addTopicElement = null;

let addTopicTitleError = "";
let addTopicDescriptionError = "";

function setTopicSectionIdAndTutorialId(sectionId, tutorialId, element) {
    addTopicSectionId = sectionId;
    addTopicTutorialId = tutorialId;
    addTopicElement = element;
}

function clearAddTopicModal() {
    document.getElementById("add-topic-title-error").innerText = "";
    document.getElementById("add-topic-description-error").innerText = "";
    document.getElementById("Title").value = "";
    document.getElementById("ShortDescription").value = "";
}

document.querySelector("#add-topic-form").addEventListener("submit", e => {
    e.preventDefault();
    const baseForm = document.getElementById("add-topic-form");
    const form = new FormData();
    const title = baseForm["Title"].value;
    const shortDescription = baseForm["ShortDescription"].value;

    form.append("Title", title);
    form.append("ShortDescription", shortDescription);

    $.ajax({
        type: "POST",
        url: `/Tutorials/${addTopicTutorialId}/Section/${addTopicSectionId}/Topics`,
        dataType: "json",
        contentType: "application/x-www-form-urlencoded",
        data: {
            Title: title,
            ShortDescription: shortDescription
        },
        statusCode: {
            201: onAddTopicSuccess,
            400: function (result) {
                const jsonResult = JSON.parse(result.responseText);
                let titleError = "";
                let descriptionError = "";

                console.log(jsonResult);

                if (jsonResult.Title != null)
                    titleError = jsonResult.Title[0];
                else titleError = "";

                if (jsonResult.ShortDescription != null)
                    descriptionError = jsonResult.ShortDescription[0];
                else descriptionError = "";

                document.getElementById("add-topic-title-error").innerText = titleError;
                document.getElementById("add-topic-description-error").innerText = descriptionError;
            }
        }
    })
})

function onAddTopicSuccess(data, textStatus) {
    clearAddTopicModal();

    $("#add-topic-modal").modal("hide");

    let topics = addTopicElement.parentElement.querySelector(".topics");
    let noTopics = addTopicElement.parentElement.querySelector(".no-topics");
    let topicId = data.id;
    let topicTitle = data.title;

    if (topics.style.display == "none") {
        noTopics.style.display = "none";
        topics.style.display = "block";
    }

    let topic = document.createElement("li");
    let topicLink = document.createElement("a")

    topicLink.href = `/Tutorials/${addTopicTutorialId}/Section/${addTopicSectionId}/Topics/${topicId}`;
    topicLink.innerText = topicTitle;

    topic.appendChild(topicLink);
    topics.appendChild(topic);
}

function redirectToPage(page) {
    window.location.href = page;
}

document.querySelectorAll(".section").forEach(section => {
    let btnExpand = section.querySelector(".btn-section-expand");
    let sectionDetails = section.querySelector(".section-details");
    let sectionHeader = section.querySelector(".section-header");

    if (btnExpand != null && sectionDetails != null) {
        sectionHeader.addEventListener("click", e => {
            if (sectionDetails.style.display == "none") {
                sectionDetails.style.display = "block";
                btnExpand.innerHTML = '<i class="icon-up-dir"></i>';
            } else {
                sectionDetails.style.display = "none";
                btnExpand.innerHTML = '<i class="icon-down-dir"></i>';
            }
        })
    }

});



let addTopicSectionId = "";
let addTopicTutorialId = "";
let addTopicElement = null;

let addTopicTitleError = "";
let addTopicDescriptionError = "";
let addTopicVideoFileError = "";
let addTopicDocumentFileError = "";

function setTopicSectionIdAndTutorialId(sectionId, tutorialId, element) {
    addTopicSectionId = sectionId;
    addTopicTutorialId = tutorialId;
    addTopicElement = element;
}

function clearAddTopicModal() {
    document.getElementById("add-topic-title-error").innerText = "";
    document.getElementById("add-topic-description-error").innerText = "";
    document.getElementById("add-video-file-error").innerText = "";
    document.getElementById("add-document-file-error").innerText = "";
    document.getElementById("Title").value = "";
    document.getElementById("ShortDescription").value = "";
    document.getElementById("VideoFile").value = "";
    document.getElementById("DocumentFile").value = "";
}

document.querySelector("#add-topic-form").addEventListener("submit", e => {
    e.preventDefault();
    const baseForm = document.getElementById("add-topic-form");
    const form = new FormData();
    const title = baseForm["Title"].value;
    const shortDescription = baseForm["ShortDescription"].value;
    const videoFile = baseForm["VideFile"].files[0];
    const documentFile = baseForm["DocumentFile"].files[0];

    form.append("ShortDescription", shortDescription);
    form.append("Title", title);
    form.append("VideoFile", videoFile);
    form.append("DocumentFile", documentFile);

    console.log(videoFile)

    //$.ajax({
    //    type: "POST",
    //    url: `/Tutorials/${addTopicTutorialId}/Section/${addTopicSectionId}/Topics`,
    //    dataType: "json",
    //    contentType: "multipart/form-data",
    //    data: {
    //        Title: title,
    //        ShortDescription: shortDescription,
    //        VideFile: videoFile,
    //        DocumentFile: documentFile
    //    },
    //    statusCode: {
    //        201: onAddTopicSuccess,
    //        400: function (result) {
    //            const jsonResult = JSON.parse(result.responseText);
    //            let titleError = "";
    //            let descriptionError = "";
    //            let videoError = "";
    //            let documentError = "";

    //            console.log(jsonResult);

    //            if (jsonResult.Title != null)
    //                titleError = jsonResult.Title[0];
    //            else titleError = "";

    //            if (jsonResult.ShortDescription != null)
    //                descriptionError = jsonResult.ShortDescription[0];
    //            else descriptionError = "";

    //            document.getElementById("add-topic-title-error").innerText = titleError;
    //            document.getElementById("add-topic-description-error").innerText = descriptionError;
    //        }
    //    }
    //})

    fetch(`/Tutorials/${addTopicTutorialId}/Section/${addTopicSectionId}/Topics`, {
        method: "POST",
        headers: {
            //"Content-Type": "multipart/form-data"
        },
        body: form,
    })
        .then(res => {
            if (res.status == 201)
                res.json().then(data => onAddTopicSuccess(data))
        })
        .then(data => console.log(data))
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
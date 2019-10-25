 function projectsLinkClick(projectId) {
    return new Promise(resolve => {
        let getProjectXhr = new XMLHttpRequest();
        getProjectXhr.open("GET", `/projects/${projectId}`, true);
        getProjectXhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
        getProjectXhr.onload = function () {
            ajaxFinished = true; 
            resolve(getProjectXhr.response);
        };
        getProjectXhr.onerror = function () {
            resolve({ success: false }, { message: "Request was finished with error" })
        };
        getProjectXhr.send();
    });
}
window.addEventListener("load", () => {
    let contentContainer = document.querySelector(".content-container");
    let linkButtons = document.querySelectorAll(".link-button");
    if ((linkButtons == "undefined") || (linkButtons == null)) return;
    for (let linkButton of linkButtons) {
        linkButton.addEventListener("click", async function () {
            ajaxFinished = false; 
            let result = await Promise.all([
                projectsLinkClick(linkButton.getAttribute("project-id")),
                FadeInAnimation(animationDuration)]);
            let progressImage = document.getElementById("progress-image");
            progressImage.style.display = "none";
            window.scrollTo(0, 0);
            contentContainer.innerHTML = result[0];                        
            FadeOutAnimation(animationDuration);  
            history.pushState("", document.title, window.location.origin
                + "/projects/" + linkButton.getAttribute("project-id"));
        });
    }
});
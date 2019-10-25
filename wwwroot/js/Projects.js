window.addEventListener("load", () => {
    let contentContainer = document.querySelector(".content-container");
    async function GetAboutHtml() {
        return new Promise(resolve => {
            let getAboutXhr = new XMLHttpRequest();
            getAboutXhr.open("GET", "/home/projects", true);
            getAboutXhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
            getAboutXhr.onload = function () {
                ajaxFinished = true; 
                resolve(getAboutXhr.response);
            };
            getAboutXhr.onerror = function () {
                resolve({ success: false }, { message: "Request was finished with error" })
            };
            getAboutXhr.send();
        });
    }
   
    document
        .getElementById("projects-button")
        .addEventListener("click", async function () {
            ajaxFinished = false; 
            clearBlogDOM();
            let result = await Promise.all([GetAboutHtml(), FadeInAnimation(animationDuration)]);
            let progressImage = document.getElementById("progress-image");
            progressImage.style.display = "none"; 
            window.scrollTo(0, 0);
            contentContainer.innerHTML = result[0];
            FadeOutAnimation(animationDuration);  
            history.pushState("", document.title, window.location.origin
                + "/projects");
            let linkButtons = document.querySelectorAll(".link-button");
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
                    anime({
                        targets: ".content-container",
                        opacity: [0, 1],               
                        duration: animationDuration,
                        easing: "easeInOutQuad"
                    });
                    history.pushState("", document.title, window.location.origin
                        + "/projects/" + linkButton.getAttribute("project-id"));
                });
            }
        });
});
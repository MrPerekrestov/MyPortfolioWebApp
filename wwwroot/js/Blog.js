window.addEventListener("load", () => {
    let contentContainer = document.querySelector(".content-container");
    async function GetBlogHtml() {
        return new Promise(resolve => {
            let getAboutXhr = new XMLHttpRequest();
            getAboutXhr.open("GET", "/blog", true);
            getAboutXhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
            getAboutXhr.onload = function () {
                ajaxFinished = true;
                resolve(getAboutXhr.response);
            };
            getAboutXhr.onerror = function () {
                resolve({ success: false }, { message: "Request was finished with error" });
            };
            getAboutXhr.send();
        });
    }

    document
        .getElementById("blog-button")
        .addEventListener("click", async function () {           
            ajaxFinished = false;            
            let result = await Promise.all([GetBlogHtml(), FadeInAnimation(animationDuration)]);           
            let progressImage = document.getElementById("progress-image");
            progressImage.style.display = "none";
            clearBlogDOM();
            contentContainer.innerHTML = result[0];
            let reactHydrateScript = document.querySelector("#react-script-container script").innerHTML;
            eval(reactHydrateScript);                        
            window.scrollTo(0, 0);
            FadeOutAnimation(animationDuration);  
            history.pushState("", document.title, window.location.origin
                + "/blog");
        });
});
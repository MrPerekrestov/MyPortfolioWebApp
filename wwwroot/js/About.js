window.addEventListener("load", () => {
    let contentContainer = document.querySelector(".content-container");
    async function GetAboutHtml() {
        return new Promise(resolve => {
            let getAboutXhr = new XMLHttpRequest();
            getAboutXhr.open("GET", "/about", true);
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
        .getElementById("about-button")
        .addEventListener("click", async function () {
            ajaxFinished = false;            
            let result = await Promise.all([GetAboutHtml(), FadeInAnimation(animationDuration)]);
            let progressImage = document.getElementById("progress-image");
            progressImage.style.display = "none";       
            clearBlogDOM();
            contentContainer.innerHTML = result[0];
            window.scrollTo(0, 0);            
            FadeOutAnimation(animationDuration);         
            history.pushState("", document.title, window.location.origin
                + "/about");
        });
});
﻿window.addEventListener("load", () => {
    let contentContainer = document.querySelector(".content-container");
    async function GetAboutHtml() {
        return new Promise(resolve => {
            let getAboutXhr = new XMLHttpRequest();
            getAboutXhr.open("GET", "/about", true);
            getAboutXhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
            getAboutXhr.onload = function () {
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
        .addEventListener("click", async function() {
            let result = await Promise.all([GetAboutHtml(), FadeInAnimation(animationDuration)]);
            contentContainer.innerHTML = result[0];
            anime({
                targets: ".content-container",
                opacity: [0, 1],
                duration: animationDuration,
                easing: "easeInOutQuad"
            });
            history.pushState("", document.title, window.location.origin
                + "/about");
        });
});
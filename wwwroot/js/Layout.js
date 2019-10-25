var animationDuration = 300;
var ajaxFinished = false;
function FadeInAnimation(time) {
    anime({
        targets: ".content-container",
        opacity: [1, 0],
        duration: time,
        easing: "easeInOutQuad",
        complete: function () {
            if (ajaxFinished) return;
            let progressImage = document.getElementById("progress-image");
            progressImage.style.display = "block";           
        }
    });

    anime({
        targets: ".background",
        filter: ["blur(2px)", "blur(0px)"],
        duration: time,
        easing: "easeInOutQuad"        
    });
    return new Promise(resolve => setTimeout(resolve, time));
}

function FadeOutAnimation(time) {
    anime({
        targets: ".content-container",
        opacity: [0, 1],
        duration: time,
        easing: "easeInOutQuad"        
    });

    anime({
        targets: ".background",
        filter: ["blur(0px)", "blur(2px)"],
        duration: time,
        easing: "easeInOutQuad"
    });
}

function clearBlogDOM() {
    let reactBlogContainer = document.getElementById("my-blog");
    if (reactBlogContainer !== "undefined" && reactBlogContainer !== null) {  
        if (!ReactDOM) return;
        ReactDOM.unmountComponentAtNode(reactBlogContainer);
    }
    let reactCommentsContainer = document.getElementById("blog-comments");
    if (reactCommentsContainer !== "undefined" && reactCommentsContainer !== null) {
        if (!ReactDOM) return;
        ReactDOM.unmountComponentAtNode(reactCommentsContainer);
    }
}
async function PopUpMessage(message, animationDuration, showTime) {
    let popUpContainer = document.getElementById("pop-up");
    let popUpMessageContainer = document.getElementById("pop-up-message");
    console.log(popUpContainer);
    popUpContainer.style.display = "flex";
    popUpMessageContainer.innerText = message;
    if (window.innerWidth < 500) {
        popUpMessageContainer.style.width = window.innerWidth * 0.8 + "px";
    }
    anime({
        targets: "#pop-up",
        opacity: ["0", "0.9"],
        width: ["0", "32rem"],
        duration: animationDuration,
        easing: "easeInOutQuad"
    });
    await new Promise(resolve => setTimeout(resolve, animationDuration));
    await new Promise(resolve => setTimeout(resolve, showTime));
    anime({
        targets: "#pop-up",
        opacity: ["0.9", "0"],
        width: ["32rem", "0"],
        duration: animationDuration,
        easing: "easeInOutQuad"
    });
    await new Promise(resolve => setTimeout(resolve, animationDuration));
    popUpContainer.style.display = "none";
}
window.addEventListener("load", async function () {  
    let background = document.querySelector(".background");   
    background.style.height = (document.documentElement.clientHeight + 120) + "px";     
    window.addEventListener("resize", function () {
        background.style.height = (document.documentElement.clientHeight + 120) + "px";      
    });
    document.body.style.display = "flex";
});

window.onpopstate = function () {
    window.location = window.location.href;
}


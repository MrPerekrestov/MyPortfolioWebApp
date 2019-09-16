function projectsLinkClick(projectId) {
    return new Promise(resolve => {
        let getProjectXhr = new XMLHttpRequest();
        getProjectXhr.open("GET", `/projects/${projectId}`, true);
        getProjectXhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
        getProjectXhr.onload = function () {
            resolve(getProjectXhr.response);
        };
        getProjectXhr.onerror = function () {
            resolve({ success: false }, { message: "Request was finished with error" })
        };
        getProjectXhr.send();
    });
}
"use strict";

window.addEventListener("load", function () {
    var contentContainer = document.querySelector(".content-container");
    var linkButtons = document.querySelectorAll(".link-button");
    if (linkButtons == "undefined" || linkButtons == null) return;
    var _iteratorNormalCompletion = true;
    var _didIteratorError = false;
    var _iteratorError = undefined;

    try {
        var _loop = function _loop() {
            var linkButton = _step.value;
            linkButton.addEventListener("click", async function () {
                var result = await Promise.all([projectsLinkClick(linkButton.getAttribute("project-id")), FadeInAnimation(animationDuration)]);
                contentContainer.innerHTML = result[0];
                anime({
                    targets: ".content-container",
                    opacity: [0, 1],
                    duration: animationDuration,
                    easing: "easeInOutQuad"
                });
                history.pushState("", document.title, window.location.origin + "/projects/" + linkButton.getAttribute("project-id"));
            });
        };

        for (var _iterator = linkButtons[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
            _loop();
        }
    } catch (err) {
        _didIteratorError = true;
        _iteratorError = err;
    } finally {
        try {
            if (!_iteratorNormalCompletion && _iterator.return != null) {
                _iterator.return();
            }
        } finally {
            if (_didIteratorError) {
                throw _iteratorError;
            }
        }
    }
});
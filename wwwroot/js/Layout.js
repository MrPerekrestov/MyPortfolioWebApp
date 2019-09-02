async function PopUpMessage(message, animationDuration, showTime) {
  let popUpContainer = document.getElementById("pop-up");
  let popUpMessageContainer = document.getElementById("pop-up-message");
  console.log(popUpContainer);
  popUpContainer.style.display = "flex";
  popUpMessageContainer.innerText = message;
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
window.addEventListener("load", () => {});

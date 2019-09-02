window.addEventListener("load", () => {
  let shade = document.getElementById("shade");
  let sendMailContainerOuter = document.getElementById(
    "send-mail-container-outer"
  );
  let sendMailYourMail = document.getElementById("send-mail-your-email");
  let sendMailSubject = document.getElementById("send-mail-subject");
  let sendMailText = document.getElementById("send-mail-text");
  let emailValidation = document.getElementById("send-mail-email-validation");
  let subjectValidation = document.getElementById(
    "send-mail-subject-validation"
  );
  let textValidation = document.getElementById("send-mail-text-validation");
  let sendEmailIsOpened = false;

  document.getElementById("send-email-image").addEventListener("click", () => {
    if (!sendEmailIsOpened) {
      sendMailSubject.value = "";
      sendMailText.value = "";
      sendMailYourMail.value = "";
      emailValidation.innerText = "";
      subjectValidation.innerText = "";
      textValidation.innerText = "";
      shade.style.display = "block";
      sendMailContainerOuter.style.display = "block";
      sendEmailIsOpened = true;

      anime({
        targets: ".shade",
        opacity: [0, 0.7],
        duration: 200,
        easing: "easeInOutQuad"
      });
      anime({
        targets: ".send-mail-container-outer",
        opacity: [0, 1],
        top: ["40%", "50%"],
        duration: 200,
        easing: "easeInOutQuad"
      });
    }
  });

  const closeEmail = async () => {
    if (sendEmailIsOpened) {
      anime({
        targets: ".shade",
        opacity: [0.7, 0],
        duration: 200,
        easing: "easeInOutQuad"
      });
      anime({
        targets: ".send-mail-container-outer",
        opacity: [1, 0],
        top: ["50%", "40%"],
        duration: 200,
        easing: "easeInOutQuad"
      });

      await new Promise(resolve => setTimeout(resolve, 200));
      shade.style.display = "none";
      sendMailContainerOuter.style.display = "none";
      sendEmailIsOpened = false;
    }
  };

  document
    .getElementById("email-cancel-button")
    .addEventListener("click", closeEmail);

  document
    .getElementById("email-send-button")
    .addEventListener("click", async () => {
      let emailValidationRegExp = /^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$/;
      let email = sendMailYourMail.value;
      let inputIsValid = true;
      if (!emailValidationRegExp.test(email)) {
        emailValidation.innerText = "e-mail has invalid format";
        inputIsValid = false;
      } else {
        emailValidation.innerText = "";
      }

      if (sendMailSubject.value.length == 0) {
        subjectValidation.innerText = "write your name, please";
        inputIsValid = false;
      } else {
        subjectValidation.innerText = "";
      }

      if (sendMailText.value.length < 3) {
        textValidation.innerText = "message is too short";
        inputIsValid = false;
      } else {
        textValidation.innerText = "";
      }

      if (!inputIsValid) return;

      let progressImage = document.getElementById("progress-image");
      progressImage.style.display = "block";
      await new Promise(resolve => setTimeout(resolve, 2000));
      progressImage.style.display = "none";

      await closeEmail();
      await PopUpMessage(
        "Your e-mail was successfully sent. I will answer as soon as posible 👍",
        500,
        5000
      );
    });
});
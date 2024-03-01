window.scrollToBottom = (element) => {
  element.scrollTo({
    top: element.scrollHeight,
    left: 0,
    behavior: 'smooth'
  });
}

let isAnimated;

window.setOnBottom = (el) => {
  el.scrollTop = el.scrollHeight;
/*
  if (!isAnimated) {
    isAnimated = true;
    el.classList.add("emergence");

    setTimeout(() => {
      el.classList.remove("emergence");
      isAnimated = false;
    }, 500);
  }*/
}
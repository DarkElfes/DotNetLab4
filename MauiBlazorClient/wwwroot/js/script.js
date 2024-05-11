window.scrollToBottom = (element) => {
  element.scrollTo({
    top: element.scrollHeight,
    left: 0,
    behavior: 'smooth'
  });
}
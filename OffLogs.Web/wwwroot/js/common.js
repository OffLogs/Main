window.clickOnElement = function (elementId) {
    console.log(1111 + " " + elementId)
    let element = document.getElementById(elementId);
    console.log(222 + " " + element)
    if (element) {
        element.click()
    }
};
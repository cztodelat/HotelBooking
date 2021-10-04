
let elements = document.getElementsByClassName("card__desc");

for (var i = 0; i < elements.length; i++) {
    if (elements[i].innerHTML.length > 480) {
        elements[i].innerHTML = elements[i].innerHTML.substring(0, 475) + " (more inside)";
    }
}

const btn = document.getElementById("cancel-modal");

var modal = new tingle.modal({
    footer: true,
    stickyFooter: false,
    closeMethods: ['overlay', 'button', 'escape'],
    closeLabel: "Close",
    cssClass: ['custom-class-1', 'custom-class-2'],
    onOpen: function () {
        console.log('modal open');
    },
    onClose: function () {
        console.log('modal closed');
    },
    beforeClose: function () {
        // here's goes some logic
        // e.g. save content before closing the modal
        return true; // close the modal
        return false; // nothing happens
    }
});

btn.addEventListener("click", function () {
    const modalWindow = document.getElementById("confirmationModel");
    modalWindow.classList.remove("hidden");
    modal.setContent(modalWindow);
    modal.open();
});

const btns = document.querySelectorAll(".modal-close");
for (var i = 0; i < btns.length; i++) {
    btns[i].addEventListener("click", function () {
        modal.close();
    });
}




//// add a button
//modal.addFooterBtn('Button label', 'tingle-btn tingle-btn--primary', function () {
//    // here goes some logic
//    modal.close();
//});

//// add another button
//modal.addFooterBtn('Dangerous action !', 'tingle-btn tingle-btn--danger', function () {
//    // here goes some logic
//    modal.close();
//});

// open modal


// close modal
modal.close();

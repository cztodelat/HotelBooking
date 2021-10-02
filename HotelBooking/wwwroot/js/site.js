// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let elements = document.getElementsByClassName("card__desc");

for (var i = 0; i < elements.length; i++) {
    if (elements[i].innerHTML.length > 480) {
        elements[i].innerHTML = elements[i].innerHTML.substring(0, 475) + " (more inside)";
    }
}
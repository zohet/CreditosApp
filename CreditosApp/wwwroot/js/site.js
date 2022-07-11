// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var system = {};

system.get = function (url, formdata, callback) {
    $.ajax({
        url: url,
        type: 'GET',
        processData: false,
        contentType: false,
        data: formdata,
        success: callback,
        async: false
    });
};

system.save = function (url, formdata, callback) {
    $.ajax({
        url: url,
        type: 'POST',
        processData: false,
        contentType: false,
        data: formdata,
        success: callback
    });
};

function StartLoading() {
    $(".overlay").css("display", "flex");
    $(".loader").css("display", "block");
}

function StopLoading() {
    $(".overlay").css("display", "none");
    $(".loader").css("display", "none");
}
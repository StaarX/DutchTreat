$(document).ready(function () {
    console.log("Hello pluralsight");

    var thForm = $("#theForm");
    thForm.hide();



    var button = $("#buyButton");
    button.on("click", () => {
        console.log("Buying item...");
    });

    var productInfo = $(".product-props li");
    productInfo.on("click", () => {
        console.log(`You have clicked on ${$(this).text()}`);
    });


    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popup-form");

    $loginToggle.on("click", () => {
        $popupForm.slideToggle(200);
    });




});

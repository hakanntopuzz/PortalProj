$(document).ready(function () {
    setNavigation();
    getAlert();

    $("body").tooltip({
        selector: '[data-toggle="tooltip"]',
        container: 'body'
    });

    var copyButton = document.getElementById('copyButton');
    if (typeof (copyButton) !== 'undefined' && copyButton !== null) {
        copyButton.addEventListener('click', copyToClipboard);
    }

    var path = window.location.href;
    $('ul a').each(function () {
        if (this.href === path) {
            $(this).addClass('active');
            $(this).next('ul').slideToggle();
        }
    });

    // Smooth Scroll
    $(".scrollspy a").on('click', function (event) {
        if (this.hash !== "") {
            event.preventDefault();
            var hash = this.hash;
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 600, function () {
                window.location.hash = hash;
            });
        }
    });
});

function setNavigation() {
    var path = window.location.pathname;
    path = path.replace(/\/$/, "");
    path = decodeURIComponent(path);

    $(".nav a").each(function () {
        var href = $(this).attr('href');
        if (path.substring(0, href.length) === href) {
            $(this).closest('.nav-item').addClass('active');
        }
    });
}

function getAlert() {
    $('.alertMessage').fadeIn(300);
    setTimeout(function () {
        $('.alertMessage').fadeOut(500);
    }, 2000);
}

function copyToClipboard() {
    var copyText = document.getElementById("clipboard").textContent;
    var textArea = document.createElement('textarea');
    textArea.textContent = copyText;
    document.body.append(textArea);
    textArea.select();
    if (document.execCommand('copy')) {
        $(textArea).hide();
        $(this).attr("data-original-title", "Kopyalandı!");
        $(this).tooltip('show');
    }
}

function showAlert(alertText) {
    Swal.fire({
        text: alertText,
        type: 'warning',
        showCloseButton: true,
        showCancelButton: false,
        showConfirmButton: false,
        focusConfirm: false
    });
}

function showSuccessAlert(message) {
    Swal.fire({
        text: message,
        type: 'success',
        showCloseButton: true,
        showCancelButton: false,
        showConfirmButton: false,
        focusConfirm: false
    });
}

function showAlertWithGeneralErrorMessage() {
    Swal.fire({
        text: "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.",
        showCloseButton: true,
        showConfirmButton: false
    });
}
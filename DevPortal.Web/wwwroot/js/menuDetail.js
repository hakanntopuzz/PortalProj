$(document).ready(function () {
    $("#deleteMenu").on("click", () => {
        deleteMenu();
    });

    function deleteMenu() {
        const id = $("#menuId").val();
        Swal.fire({
            title: 'Emin misiniz?',
            text: "Bu kayıt kalıcı olarak silinecektir!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#c70039',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Sil',
            cancelButtonText: 'Vazgeç'
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    dataType: 'json',
                    type: 'POST',
                    url: "/Menu/Delete",
                    data: { id: id },
                    success: function (result) {
                        if (result.isSuccess === true) {
                            redirectToWindowLocation(result.redirectUrl);
                        }
                        else {
                            if (result.message !== null) {
                                SweetAlertError(result.message);
                            }
                            else {
                                SweetAlertError();
                            }
                        }
                    },
                    error: function (err) {
                        console.log(err);
                        SweetAlertError();
                    }
                });
            }
        });
    }
});

function redirectToWindowLocation(redirectUrl) {
    window.location.href = redirectUrl;
}

function SweetAlertError() {
    Swal.fire('İşlem başarısız!', 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.', 'warning');
}

function SweetAlertError(message) {
    Swal.fire('İşlem başarısız!', message, 'warning');
}
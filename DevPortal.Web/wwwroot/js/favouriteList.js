$(document).ready(function () {
    initSort();
});

function initSort() {
    $('#listWithHandle').sortable({
        stop: function () {
            var data = $(this).sortable('serialize');
            sortFavourites(data);
        }
    });
}

function sortFavourites(data) {
    $.ajax({
        data: data,
        type: 'POST',
        url: Urls.SortFavouritePages,
        success: function (result) {
            if (result.isSuccess) {
                showSuccessAlert(result.message);
                listFavourites();
            }
            else {
                showAlert(result.message);
            }
        },
        error: function (err) {
            showAlertWithGeneralErrorMessage();
        }
    });
}

function listFavourites() {
    $.ajax({
        url: Urls.FavouritePagesPartial,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    }).success(function (result) {
        $('#favouritesDiv').html(result);
        initSort();
    })
        .error(function (xhr, status) {
            showAlert(status);
        });
}

function deleteFavouriteOnPage(id) {
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu kayıt kalıcı olarak silinecektir!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#c70039',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Sil',
        cancelButtonText: 'Vazgeç'
    }).then((confirmResult) => {
        if (confirmResult.value) {
            $.ajax({
                dataType: 'json',
                type: 'POST',
                url: Urls.DeleteFavouritePage,
                data: { id: id },
                success: function (deleteResult) {
                    if (deleteResult.isSuccess === true) {
                        showSuccessAlert(deleteResult.message);
                        listFavourites();
                    }
                    else {
                        if (deleteResult.message !== null) {
                            showAlert(deleteResult.message);
                        }
                        else {
                            showAlertWithGeneralErrorMessage();
                        }
                    }
                },
                error: function (err) {
                    showAlertWithGeneralErrorMessage();
                }
            });
        }
    });
}
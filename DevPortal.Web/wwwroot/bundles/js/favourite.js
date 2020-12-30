const favouritePageName = $("#favouritePageName").val();
const favouriteId = $("#favouriteId");
const addFavourite = $("#addFavourite");
const deleteFavourite = $("#deleteFavourite");

//Favori sorgulama
$(document).ready(function () {
    var pageUrl = window.location.pathname + window.location.search;
    getFavouritePage(pageUrl);
});

function getFavouritePage(pageUrl) {
    $.ajax({
        type: 'GET',
        url: Urls.GetFavouritePageByUserIdAndPageUrl,
        data: { pageUrl: pageUrl },
        contentType: "application/json",
        success: function (result) {
            if (result.isSuccess) {
                addFavourite.hide();
                deleteFavourite.show();
                favouriteId.attr("value", result.data.id);
            }
            else {
                deleteFavourite.hide();
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

//Favori ekleme
addFavourite.on("click", () => {
    var pageUrl = window.location.pathname + window.location.search;
    addFavorite(favouritePageName, pageUrl);
});

function addFavorite(pageName, pageUrl) {
    var favouritePage = {
        PageName: pageName,
        PageUrl: pageUrl,
    };
    $.ajax({
        type: 'POST',
        url: Urls.AddFavouritePage,
        data: JSON.stringify(favouritePage),
        contentType: "application/json",
        success: function (result) {
            if (result.isSuccess) {
                showSuccessAlert(result.message);

                getFavouritePage(pageUrl);
            }
            else {
                showAlert(result.message);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

//Favorilerden çıkarma
deleteFavourite.on("click", () => {
    var id = favouriteId.val();
    deleteFavorite(id);
});

function deleteFavorite(id) {
    $.ajax({
        type: 'POST',
        url: Urls.DeleteFavouritePage,
        data: { id: id },
        dataType: 'json',
        success: function (result) {
            if (result.isSuccess) {
                showSuccessAlert(result.message);

                addFavourite.show();
                deleteFavourite.hide();
            }
            else {
                showAlert(result.message);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}
var $select_databaseGroup = $(".ddlDatabaseGroup");
var $select_ddlDatabase = $(".ddlDatabase");

$select_databaseGroup.on("change", function () {
    var databaseGroupId = $(this).val();

    getDatabase(databaseGroupId);
});

function getDatabase(databaseGroupId) {
    if (databaseGroupId > 0) {
        $.ajax({
            type: "GET",
            url: "/DatabaseDependency/GetDatabasesByDatabaseGroupId?databaseGroupId=" + databaseGroupId,
            data: { "databaseGroupId": databaseGroupId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                clearDropDownList($select_ddlDatabase);

                $(data).each(function () {
                    $("<option />", {
                        val: this.id,
                        text: this.name
                    }).appendTo($select_ddlDatabase);
                });
            },
            error: function (data) {
                showAlert("Database bilgisi getirilirken bir hata oluştu.", data);
            }
        });
    }
    else {
        clearDropDownList($select_ddlDatabase);
    }
}

function clearDropDownList(selectInput, inputText) {
    if (inputText === undefined || inputText === "") {
        inputText = "Seçiniz";
    }
    selectInput.empty();
    selectInput.prepend("<option value='' selected='selected'>" + inputText + "</option>");
}
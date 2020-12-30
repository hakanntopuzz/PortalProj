var $select_applicationGroup = $(".ddlApplicationGroup");
var $select_ddlApplication = $(".ddlApplication");

$select_applicationGroup.on("change", function () {
    var applicationGroupId = $(this).val();

    getApplication(applicationGroupId);
});

function getApplication(applicationGroupId) {
    if (applicationGroupId > 0) {
        $.ajax({
            type: "GET",
            url: "/ApplicationDependency/GetApplicationsByGroupId?applicationGroupId=" + applicationGroupId,
            data: { "applicationGroupId": applicationGroupId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                clearDropDownList($select_ddlApplication);

                $(data).each(function () {
                    $("<option />", {
                        val: this.id,
                        text: this.name
                    }).appendTo($select_ddlApplication);
                });
            },
            error: function (data) {
                showAlert("Uygulama bilgisi getirilirken bir hata oluştu.", data);
            }
        });
    }
    else {
        clearDropDownList($select_ddlApplication);
    }
}

function clearDropDownList(selectInput, inputText) {
    if (inputText === undefined || inputText === "") {
        inputText = "Seçiniz";
    }
    selectInput.empty();
    selectInput.prepend("<option value='' selected='selected'>" + inputText + "</option>");
}
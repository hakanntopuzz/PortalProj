var $nugetPackagesId = $('#nugetPackagesId');
var $packageUrlId = $('#packageUrlId');
var $nugetPackageRootUrl;

window.addEventListener('load', () => {
    getNugetPackageRootUrl();
    getNugetPackagesName();
});

function getNugetPackageRootUrl() {
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    $.ajax({
        url: Urls.GetNugetPackageRootUrl,
        datatype: "JSON",
        contenttype: 'application/json',
        type: "GET",
        async: true,
        data: { __RequestVerificationToken: token },
        success: function (data) {
            if (data.length !== 0) {
                $nugetPackageRootUrl = data;
            }
        },
        error: function (xhr) {
        }
    });
}
function getNugetPackagesName() {
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    $.ajax({
        url: Urls.GetNugetPackagesName,
        datatype: "JSON",
        contenttype: 'application/json',
        type: "GET",
        async: true,
        data: { __RequestVerificationToken: token },
        success: function (data) {
            if (data.length !== 0) {
                $.each(data, function (index, value) {
                    $("#nugetPackagesId").append("<option value=" + value.title + ">" + value.title + "</option>");
                    $("#nugetPackagesId").selectpicker("refresh");
                });
            }
        },
        error: function (xhr) {
        }
    });
}

$nugetPackagesId.on("change", function () {
    var nugetPackage = $(this).val();
    $packageUrlId.html('');
    $packageUrlId.append($nugetPackageRootUrl + nugetPackage);
    $packageUrlId.attr("href", $nugetPackageRootUrl + nugetPackage);
    $("#lbl-nuget span.field-validation-error").removeClass("field-validation-error").addClass("field-validation-valid").html("");

});

$("#btn-add").click(function () {
    if ($("#nugetPackagesId").val() === "") {
        $("#lbl-nuget span.field-validation-valid").removeClass("field-validation-valid").addClass("field-validation-error").html("Lütfen nuget paketi seçiniz.");
        return false;
    }
    return true;
});
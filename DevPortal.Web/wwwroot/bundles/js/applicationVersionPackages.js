var $versionEnvironment = $('#versionEnvironment');
var defaultSortColumn = $("#deployment-project-table thead tr th").first().data("column");

var dataTable = $("#deployment-project-table").DataTable({
    "language": {
        "processing": "İşleniyor...",
        "paginate": {
            "first": "İlk",
            "last": "Son",
            "next": "Sonraki",
            "previous": "Geri"
        },
        "info": "_TOTAL_ kayıttan _START_ ve _END_ arasındaki kayıtlar gösterilmektedir.",
        "loadingRecords": "Yükleniyor...",
        "emptyTable": "Gösterilecek veri yok.",
        "infoEmpty": "Kayıt yok"
    },
    "columns": [
        {
            'data': 'version', 'render': function (value, type, data) {
                return data.name;
            }
        },
        {
            'render': function (value, display, data) {
                return "<a class='btn btn-link' type='button' data-toggle='tooltip' title='Detay' href=/database/detail/" + data.id + "><i class='far fa-eye' aria-hidden='true'></i></a>"
            },
            'orderable': false,
            'className': 'text-center'
        }
    ],
    "processing": true,
    "serverSide": true,
    "ajax": {
        "url": "DeploymentPackage/Index",
        "method": "POST",
        "data": function (data) {
            data.SearchText = $("#database-search").val();
            data.SortColumn = defaultSortColumn;
        }
    },
    "searching": false,
    "paging": true,
    "lengthChange": false,
    "pageLength": 10
});

function getApplicationList(environmentId) {
    debugger;
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    $.ajax({
        url: '/DeploymentPackage/GetApplicationList',
        datatype: "JSON",
        contenttype: 'application/json',
        type: "POST",
        async: true,
        data: { __RequestVerificationToken: token, "environmentId": environmentId },
        success: function (data) {
            if (data.isSuccess) {
                if (data.data.length !== 0) {
                    $.each(data.data, function (index, value) {
                        $("#application").append("<option value=" + value.id + ">" + value.name + "</option>");
                    });
                }
            }
        },
        error: function (xhr) {
        }
    });
}

$(document).ready(function () {
    $("#deployment-project-table thead tr th").on("click", function () {
        defaultSortColumn = $(this).data("column");
    });

    $("#application-search-button").on("click", function () {
        dataTable.ajax.reload();
    });

    $versionEnvironment.on("change", function () {
        var environmentId = $(this).val();

        if (environmentId > 0) {
            getApplicationList(environmentId);
        }
    })
});
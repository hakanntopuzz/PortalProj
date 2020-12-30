var $applicationGroupId = $("#ApplicationGroupId");
var $applicationName = $("#application-search");
var defaultSortColumn = $("#application-table thead tr th").first().data("column");

var dataTable = $("#application-table").DataTable({
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
        { 'data': 'name' },
        {
            'data': 'applicationGroupName', 'render': function (value, display, data) {
                return "<a href='" + location.origin + "/applicationgroup/detail/" + data.applicationGroupId + "'>" + (value === null ? '' : value) + "</a>";
            }
        },
        { 'data': 'status' },
        {
            'data': 'recordUpdateInfo.modifiedDate', 'render': function (value) {
                return new Date(value).toLocaleString("tr-TR");
            }
        },
        {
            'render': function (value, display, data) {
                return "<a class='btn btn-link' type='button' data-toggle='tooltip' title='Detay' href='/application/detail/" + data.id + "'\"><i class='far fa-eye' aria-hidden='true'></i></a>"
            },
            'orderable': false,
            'className': 'text-center'
        }
    ],
    "processing": true,
    "serverSide": true,
    "ajax": {
        "url": "Application/Index",
        "method": "POST",
        "data": function (data) {
            data.SearchText = $("#application-search").val();
            data.ApplicationGroupId = $("#ApplicationGroupId").val();
            data.SortColumn = defaultSortColumn;
        }
    },
    "searching": false,
    "paging": true,
    "lengthChange": false,
    "pageLength": 10
});

$(document).ready(function () {
    $("#application-table thead tr th").on("click", function () {
        defaultSortColumn = $(this).data("column");
    });

    $("#application-search-button").on("click", function () {
        dataTable.ajax.reload();
    });

    $("#application-search").on('keyup', function (event) {
        if (event.keyCode === 13) {
            dataTable.ajax.reload();
        }
    });

    $('#csv_export').click(function () {
        var applicationGroupId = $applicationGroupId.val();
        var applicationName = $applicationName.val();
        var url = Urls.ExportApplicationAsCsv + "?applicationGroupId=" + applicationGroupId + "&applicationName=" + applicationName;
        window.location.href = url;
    });

    $("#export-as-wiki").on("click", () => {
        exportApplicationsAsWiki();
    });

    $("#export-as-wiki-file").on("click", () => {
        exportApplicationsAsWikiFile();
    });
})

function exportApplicationsAsWiki() {
    var applicationGroupId = $applicationGroupId.val();
    var applicationName = $applicationName.val();

    var url = Urls.ExportApplicationAsWiki + "?applicationGroupId=" + applicationGroupId + "&applicationName=" + applicationName;
    $.ajax({
        url: url,
        type: 'get',
        success: function (response) {
            $('#applications-wiki-text').html(response);
            $('#export-as-wiki-modal').modal('show');
        }
    });
}

function exportApplicationsAsWikiFile() {
    var applicationGroupId = $applicationGroupId.val();
    var applicationName = $applicationName.val();
    var url = Urls.ExportApplicationAsWikiFile + "?applicationGroupId=" + applicationGroupId + "&applicationName=" + applicationName;
    window.location.href = url;
}
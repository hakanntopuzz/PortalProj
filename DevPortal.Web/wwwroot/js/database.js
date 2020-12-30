var $databaseGroupId = $("#DatabaseGroupId");
var $databaseName = $("#database-search");
var defaultSortColumn = $("#database-table thead tr th").first().data("column");

var dataTable = $("#database-table").DataTable({
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
            'data': 'databaseName', 'render': function (value, type, data) {
                return data.name;
            }
        },
        {
            'data': 'databaseGroupName', 'render': function (value, type, data) {
                return "<a href='" + location.origin + "/databasegroup/detail/" + data.databaseGroupId + "'>" + data.databaseGroupName + "</a>";
            }
        },
        {
            'data': 'databaseTypeName', 'render': function (value, type, data) {
                return "<a href='" + location.origin + "/databasetype/detail/" + data.databaseTypeId + "'>" + data.databaseTypeName + "</a>";
            }
        },
        {
            'data': 'recordUpdateInfo_ModifiedDate', 'render': function (value, type, data) {
                return new Date(data.recordUpdateInfo.modifiedDate).toLocaleString("tr-TR");
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
        "url": "Database/Index",
        "method": "POST",
        "data": function (data) {
            data.SearchText = $("#database-search").val();
            data.DatabaseGroupId = $("#DatabaseGroupId").val();
            data.SortColumn = defaultSortColumn;
        }
    },
    "searching": false,
    "paging": true,
    "lengthChange": false,
    "pageLength": 10
});

$(document).ready(function () {
    $("#database-table thead tr th").on("click", function () {
        defaultSortColumn = $(this).data("column");
    });

    $("#database-search-button").on("click", function () {
        dataTable.ajax.reload();
    });

    $("#database-search").on('keyup', function (event) {
        if (event.keyCode === 13) {
            dataTable.ajax.reload();
        }
    });

    $('#csv_export').click(function () {
        var databaseGroupId = $databaseGroupId.val();
        var databaseName = $databaseName.val();
        var url = Urls.ExportDatabaseAsCsv + "?databaseGroupId=" + databaseGroupId + "&databaseName=" + databaseName;
        window.location.href = url;
    });

    $("#export-as-wiki").on("click", () => {
        exportDatabasesAsWiki();
    });

    $("#export-as-wiki-file").on("click", () => {
        exportDatabasesAsWikiFile();
    });
})

function exportDatabasesAsWiki() {
    var databaseGroupId = $databaseGroupId.val();
    var databaseName = $databaseName.val();

    var url = Urls.ExportDatabaseAsWiki + "?databaseGroupId=" + databaseGroupId + "&databaseName=" + databaseName;
    $.ajax({
        url: url,
        type: 'get',
        success: function (response) {
            $('#databases-wiki-text').html(response);
            $('#export-as-wiki-modal').modal('show');
        }
    });
}

function exportDatabasesAsWikiFile() {
    var databaseGroupId = $databaseGroupId.val();
    var databaseName = $databaseName.val();
    var url = Urls.ExportDatabaseAsWikiFile + "?databaseGroupId=" + databaseGroupId + "&databaseName=" + databaseName;
    window.location.href = url;
}
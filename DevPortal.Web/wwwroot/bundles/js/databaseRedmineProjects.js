var $databaseGroupId = $("#DatabaseGroupId");
var $databaseName = $("#database-search");
var defaultSortColumn = $("#redmine-project-table thead tr th").first().data("column");

var dataTable = $("#redmine-project-table").DataTable({
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
            'data': 'projectName', 'render': function (value, display, data) {
                return "<a href='" + data.projectUrl + "' target='_blank'>" + (value === null ? '' : value) + "</a>";
            }
        },
        {
            'render': function (value, display, data) {
                return "<a class='btn btn-link' type='button' target='_blank' href='" + data.repositoryUrl + "'\">SVN Deposunu Güncelle</a>"
            },
            'orderable': false,
            'className': 'text-center'
        }
    ],
    "processing": true,
    "serverSide": true,
    "ajax": {
        "url": Urls.DatabaseRedmineProjects,
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
    $("#redmine-project-table thead tr th").on("click", function () {
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
})
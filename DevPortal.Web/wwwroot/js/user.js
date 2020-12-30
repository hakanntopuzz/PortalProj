var defaultSortColumn = $("#user-table thead tr th").first().data("column");

var dataTable = $("#user-table").DataTable({
    "language": {
        "processing": "İşleniyor...",
        "paginate": {
            "first": "İlk",
            "last": "Son",
            "next": "Sonraki",
            "previous": "Önceki"
        },
        "info": "_TOTAL_ kayıttan _START_ ve _END_ arasındaki kayıtlar gösterilmektedir.",
        "loadingRecords": "Yükleniyor...",
        "emptyTable": "Kayıtlı kullanıcı yok",
        "infoEmpty": "Kayıt yok"
    },
    "columns": [
        { 'data': 'firstName' },
        { 'data': 'lastName' },
        {
            'data': 'emailAddress', 'render': function (value, display, data) {
                return "<a href='" + location.href + "/detail/" + data.id + "'>" + (value === null ? '' : value) + "</a>";
            }
        },
        { 'data': 'userStatus' },
        { 'data': 'userType' },
        {
            'data': 'recordUpdateInfo.modifiedDate', 'render': function (value) {
                return new Date(value).toLocaleString("tr-TR");
            }
        },
        {
            'render': function (value, display, data) {
                return "<a class='btn btn-link' type='button' data-toggle='tooltip' title='Detay'  href=/user/detail/" + data.id + "><i class='far fa-eye' aria-hidden='true'></i></button>"
            },
            'orderable': false,
            'className': 'text-center'
        }
    ],
    "processing": true,
    "serverSide": true,
    "ajax": {
        "url": "User/Index",
        "method": "POST",
        "data": function (data) {
            data.SearchText = $("#user-search").val();
            data.SortColumn = defaultSortColumn;
        }
    },
    "searching": false,
    "paging": true,
    "lengthChange": false,
    "pageLength": 10
});

$(document).ready(function () {
    $("#user-table thead tr th").on("click", function () {
        defaultSortColumn = $(this).data("column");
    });

    $("#user-search-button").on('click', function () {
        dataTable.ajax.reload();
    });

    $("#user-search").on('keyup', function (event) {
        if (event.keyCode === 13) {
            dataTable.ajax.reload();
        }
    });
});
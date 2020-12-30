var defaultSortColumn = $("#menu-table thead tr th").first().data("column");

var dataTable = $("#menu-table").DataTable({
    "language": {

        //TODO: Dış kaynaklar kullanmayalım. Bileşenleri proje içine ekleyelim.
        "url": "//cdn.datatables.net/plug-ins/1.10.12/i18n/Turkish.json"
    },
    "columns": [
        { 'data': 'name' },
        {
            'data': 'parentName', 'render': function (value, display, data) {
                return "<a href='" + location.href + "/detail/" + data.parentId + "'>" + (value === null ? '' : value) + "</a>";
            }
        },
        { 'data': 'link' },
        { 'data': 'order' },
        { 'data': 'groupName' },
        { 'data': 'icon' },
        { 'data': 'description' },
        {
            'data': 'recordUpdateInfo.modifiedDate', 'render': function (value) {
                return new Date(value).toLocaleString("tr-TR");
            }
        },
        {
            'render': function (value, display, data) {
                return "<a class='btn btn-link' type='button' data-toggle='tooltip' title='Detay' href=/menu/detail/" + data.id + "><i class='far fa-eye' aria-hidden='true'></i></button>"
            },
            'orderable': false,
            'className': 'text-center'
        }
    ],
    "processing": true,
    "serverSide": true,
    "ajax": {
        "url": "Menu/Index",
        "method": "POST",
        "data": function (data) {
            data.SearchText = $("#menu-search").val();
            data.SortColumn = defaultSortColumn;
        }
    },
    "searching": false,
    "paging": true,
    "lengthChange": false,
    "pageLength": 10
});

$(document).ready(function () {
    $("#menu-table thead tr th").on("click", function () {
        defaultSortColumn = $(this).data("column");
    });

    $("#menu-search-button").on('click', function () {
        dataTable.ajax.reload();
    });

    $("#menu-search").on('keyup', function (event) {
        if (event.keyCode === 13) {
            dataTable.ajax.reload();
        }
    });
});
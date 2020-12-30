var $svnSearchText = $('#svn-repository-search');
var $searchButton = $('#search-repository-button');

var dataTable = $('#svn-repository-table').DataTable({
    "language": {

        //TODO: Dış kaynaklar kullanmayalım. Bileşenleri proje içine ekleyelim.
        "url": "//cdn.datatables.net/plug-ins/1.10.12/i18n/Turkish.json"
    },
    "columnDefs": [
        { "targets": 1, "type": "modifiedDate" }
    ],
    "processing": true,
    "serverSide": false,
    "searching": true,
    "paging": true,
    "lengthChange": false,
    "pageLength": 10
});

$(document).ready(function () {
    $svnSearchText.on('keyup', function (event) {
        if (event.keyCode === 13) {
            dataTable.search($(this).val()).draw();
        }
    });

    $searchButton.on('click', function () {
        dataTable.search($svnSearchText.val()).draw();
    });

    $.extend($.fn.dataTable.ext.type.order, {
        "modifiedDate-asc": function (a, b) {
            var aDate = new Date(a);
            var bDate = new Date(b);

            return aDate < bDate ? 1 : -1;
        },

        "modifiedDate-desc": function (a, b) {
            var aDate = new Date(a);
            var bDate = new Date(b);

            return aDate >= bDate ? 1 : -1;
        }
    });

    $("#export-as-wiki").on("click", () => {
        exportRepositoriesAsWiki();
    });

    $("#export-as-wiki-file").on("click", () => {
        exportRepositoriesAsWikiFile();
    });
});

function exportRepositoriesAsWiki() {
    $.ajax({
        url: Urls.ExportSvnRepositoriesAsWiki,
        type: 'get',
        success: function (response) {
            $('#repositories-wiki-text').html(response);
            $('#export-as-wiki-modal').modal('show');
        }
    });
}

function exportRepositoriesAsWikiFile() {
    window.location.href = Urls.ExportSvnRepositoriesAsWikiFile;
}
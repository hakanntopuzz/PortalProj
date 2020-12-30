var $applicationGroup = $('#applicationGroup');
var $application = $('#application');
var $environment = $('#environment');
var $applicationPhysicalPath = $('#applicationPhysicalPath');
var $btnSearch = $('#btnSearch');
var $btnSearchAchive = $('#selectArchive');//Button1
var $txtSearch = $('#txtSearch');
var $txtSearchArchive = $('#searchArchive');//Button2
var $form = $('#__AjaxAntiForgeryForm');
var $token = $('input[name="__RequestVerificationToken"]', $form);
var initialLoad = true;

var dataTable = $('#log-file-table').DataTable({
    "language": {

        //TODO: Dış kaynaklar kullanmayalım. Bileşenleri proje içine ekleyelim.
        "url": "//cdn.datatables.net/plug-ins/1.10.12/i18n/Turkish.json"
    },
    "columns": [
        {
            'data': 'name', 'render': function (value, type, data) {
                return '<a href=/Log/LogDetails?path=' + data.path + ' class = "font-weight-normal" target = "_blank">' + data.name + '</a >';
            }
        },
        {
            'data': 'modifiedDate', 'render': function (value, type, data) {
                return data.dateModified;
            }
        },
        {
            'data': 'size', 'render': function (value, type, data) {
                return data.size + ' KB';
            }
        },
        {
            'data': 'path', 'render': function (value, type, data) {
                return '<div class="text-center"><a href=/Log/DownloadLogFile?path=' + data.filePath + ' type="file" title="İndir" > <i class="fa fa-download" aria-hidden="true"></i ></a ></div>';
            },
            'orderable': false,
            'searchable': false
        }
    ],
    "order": [[1, "asc"]],
    "columnDefs": [
        { "targets": 1, "type": "modifiedDate" },
        { "targets": 2, "type": "size" }
    ],
    "processing": true,
    "serverSide": false,
    "ajax": {
        "url": "Log/GetResultFileListPartial",
        "method": "POST",
        "data": function (data) {
            data.__RequestVerificationToken = $token.val();
            data.physicalPath = $applicationPhysicalPath.val();
        },
        "dataSrc": function (data) {
            if (initialLoad) {
                initialLoad = false;
                return [];
            }
            if (!data.isSuccess) {
                $("#log-file-table_filter").parent().prev().empty();

                $('.toast').toast('show');

                return [];
            }
            createFilePathHtml();

            return data.data;
        }
    },
    "searching": true,
    "paging": true,
    "lengthChange": false,
    "pageLength": 10
});

function isNullOrEmpty(value) {
    return value === undefined || value === null || value.trim() === '';
}

function clearDropdown(dropdown) {
    dropdown.find('option').not(':first').remove();
}

function getApplicationList(applicationGruopId) {
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    $.ajax({
        url: '/Log/GetApplicationsList',
        datatype: "JSON",
        contenttype: 'application/json',
        type: "POST",
        async: true,
        data: { __RequestVerificationToken: token, "applicationGroupId": applicationGruopId },
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

function getEnvironmentList(applicationId) {
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    $.ajax({
        url: '/ApplicationEnvironment/GetEnvironmentList',
        datatype: "JSON",
        contenttype: 'environment/json',
        type: "POST",
        async: true,
        data: { __RequestVerificationToken: token, "applicationId": applicationId },
        success: function (data) {
            if (data.isSuccess) {
                if (data.data.length !== 0) {
                    $.each(data.data, function (index, value) {
                        $("#environment").append("<option value=" + value.logFilePath + ">" + value.environmentName + "</option>");
                    });
                }
            }
        },
        error: function (xhr) {
        }
    });
}

function validateSearch() {
    if (isNullOrEmpty($applicationGroup.val())) {
        $applicationGroup.addClass('is-invalid');
        return false;
    }
    $applicationGroup.removeClass('is-invalid');

    if (isNullOrEmpty($application.val())) {
        $application.addClass('is-invalid');
        return false;
    }
    $application.removeClass('is-invalid');

    if (isNullOrEmpty($environment.val())) {
        $environment.addClass('is-invalid');
        return false;
    }
    $environment.removeClass('is-invalid');

    return true;
}

function validateTextSearch() {
    if (isNullOrEmpty($applicationPhysicalPath.val())) {
        $applicationPhysicalPath.addClass('is-invalid');
        return false;
    }
    $applicationPhysicalPath.removeClass('is-invalid');

    return true;
}

function createFilePathHtml() {
    $("#log-file-table_filter")
        .parent()
        .prev()
        .html('<i style="cursor:pointer;" class="fa fa-file text-primary float-left mr-1 mt-2 btn-sm"></i><p class="mt-2" id="file-path">' + $applicationPhysicalPath.val() + '</p>');
}

function setApplicationPhysicalPathForButtonSearch() {
    var path = $applicationPhysicalPath.val();
    if (path.indexOf('archive') !== '-1') {
        var newPath = path.replace("\archive", "");
        $applicationPhysicalPath.val(newPath);
    }
}

function setApplicationPhysicalPathForArchiveButtonSearch() {
    var path = $applicationPhysicalPath.val();
    if (path.indexOf('archive') == '-1') {
        $applicationPhysicalPath.val(path + '\archive');
    }
}

$(document).ready(function () {
    if (!isNullOrEmpty($applicationPhysicalPath.val())) {
        $("#get-Physical-Path").trigger('click');
    }

    $applicationGroup.on("change", function () {
        var applicationGruopId = $(this).val();

        clearDropdown($application);
        clearDropdown($environment);

        if (applicationGruopId > 0) {
            $(this).removeClass('is-invalid');

            getApplicationList(applicationGruopId);
        }
    });

    $application.on("change", function () {
        var applicationId = $(this).val();

        clearDropdown($environment);

        if (applicationId > 0) {
            $(this).removeClass('is-invalid');

            getEnvironmentList(applicationId);
        }
    });

    $environment.on("change", function () {
        var filePath = $(this).val();

        if (!isNullOrEmpty(filePath)) {
            $(this).removeClass('is-invalid');

            $applicationPhysicalPath.val(filePath);
        }
    });

    $btnSearch.on("click", function () {
        var isSuccess = validateSearch();

        if (!isSuccess) {
            showDialog();
        }
        if (document.getElementById("selectArchive").checked == true) {
            setApplicationPhysicalPathForArchiveButtonSearch();

            dataTable.ajax.reload();
        }
        else {
            setApplicationPhysicalPathForButtonSearch();

            dataTable.ajax.reload();
        }
    });

    $txtSearch.on('click', function () {
        var isSuccess = validateTextSearch();

        if (!isSuccess) {
            showDialog();
        }
        if (document.getElementById("searchArchive").checked == true) {
            setApplicationPhysicalPathForArchiveButtonSearch();

            dataTable.ajax.reload();
        }
        else {
            setApplicationPhysicalPathForButtonSearch();

            clearDropdown($application);
            clearDropdown($environment);

            dataTable.ajax.reload();
        }
    });

    var input = document.getElementById("applicationPhysicalPath");

    input.addEventListener("keyup", function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            document.getElementById("txtSearch").click();
        }
    });

    function showDialog() {
        $('.toast').toast('show');
    }

    $.extend($.fn.dataTable.ext.type.order, {
        "modifiedDate-asc": function (a, b) {
            var aDate = new Date(a);
            var bDate = new Date(b);

            return aDate < bDate ? 1 : -1;
        },

        "modifiedDate-desc": function (a, b) {
            var aDate = new Date(a);
            var bDate = new Date(b);
            console.log(aDate);
            console.log(bDate);

            return aDate >= bDate ? 1 : -1;
        },

        "size-asc": function (a, b) {
            var aSize = parseInt(a.split(' KB')[0]);
            var bSize = parseInt(b.split(' KB')[0]);

            return aSize < bSize ? 1 : -1;
        },

        "size-desc": function (a, b) {
            var aSize = parseInt(a.split(' KB')[0]);
            var bSize = parseInt(b.split(' KB')[0]);

            return aSize >= bSize ? 1 : -1;
        }
    });
});
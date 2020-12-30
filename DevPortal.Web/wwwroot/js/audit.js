var defaultSortColumn = $("th[data-column='ModifiedDate']").data("column");
var dataTable;

$(document).ready(function () {
    $("#showAudit").on('click', function () {
        $("#auditModal").modal('show');
    });

    $("#showAudit").on('click', function () {
        if (!$.fn.DataTable.isDataTable('#audit-table')) {
            dataTable = $("#audit-table").DataTable({
                "language": {

                    //TODO: Kaynakları proje içinden kullanalım.
                    "url": "//cdn.datatables.net/plug-ins/1.10.12/i18n/Turkish.json"
                },
                "columns": [
                    { 'data': 'fieldName' },
                    { 'data': 'oldValue' },
                    { 'data': 'newValue' },
                    { 'data': 'modifiedBy' },
                    {
                        'data': 'modifiedDate', 'render': function (value) {
                            return new Date(value).toLocaleString("tr-TR");
                        }
                    }
                ],
                "drawCallback": function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(4, { page: 'current' }).data().each(function (group, i) {
                        group = new Date(group).toLocaleDateString();

                        if (last !== group) {
                            $(rows).eq(i).before(
                                $('<tr class="group table-start-group active fa"><td colspan="5">' + new Date(group).toLocaleDateString() + '</td></tr>').on('click', function () {
                                    $(this).toggleClass("active");
                                    var next = $(this).next();

                                    while (!next.hasClass("group") && next.is("tr")) {
                                        if (next.css("display") === "none") {
                                            next.show(300);
                                        } else {
                                            next.fadeOut(300);
                                        }
                                        next = next.next();
                                    }
                                })
                            );

                            last = new Date(group).toLocaleDateString();
                        }
                    });
                },
                "order": [[4, "desc"]],
                "createdRow": function (row, data, dataIndex) { },
                "processing": true,
                "serverSide": true,
                "ajax": {
                    "url": "/Audit/Index",
                    "method": "POST",
                    "data": function (data) {
                        data.RecordId = $("#RecordId").val();
                        data.TableName = $("#TableName").val();
                        data.SortColumn = defaultSortColumn;
                    }
                },
                "searching": false,
                "paging": true,
                "lengthChange": false,
                "pageLength": 10
            });

            $("#audit-table thead tr th").on("click", function () {
                defaultSortColumn = $(this).data("column");
            });
        } else {
            dataTable.ajax.reload();
        }
    });
});
var table = $('#StudentList');
var blockusers = [];

function StudentOperation(resp, type) {
    if (resp.IsError) {
        new PNotify({
            title: 'error',
            text: resp.Message,
            type: 'error',
            styling: 'bootstrap3'
        });
    }
    else {
        new PNotify({
            title: 'Student Delete',
            text: 'Student deleted successfully',
            type: 'success',
            styling: 'bootstrap3'
        });
        setTimeout(function () { window.location = "/admin/studentlist"; }, 2000);
    }
}

function BlockStudentCallFunction(resp, type) {
    if (resp.IsError) {
        new PNotify({
            title: 'Block Students',
            text: resp.Message,
            type: 'error',
            styling: 'bootstrap3'
        });
    }
    else {
        new PNotify({
            title: 'Block Students',
            text: 'Student blocked successfully',
            type: 'success',
            styling: 'bootstrap3'
        });
        $('.studchkhead').iCheck('uncheck');
        LoadStudentList();
    }
}

function LoadStudentList() {

    if ($.fn.DataTable.isDataTable('#StudentList')) {
        $('#StudentList').DataTable().destroy();
    }

    table = table.dataTable({
        "ajax": {
            "type": "GET",
            "url": "/admin/students/GetStudentList",
            "dataSrc": function (resp) {
                if (resp.IsError) {
                    new PNotify({
                        title: 'Student List',
                        text: resp.Message,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    return resp.Data;
                } else {
                    return resp.Data;
                }
            }
        },
        "language": {
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            },
            "emptyTable": "No data available in table",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "infoEmpty": "No entries found",
            "infoFiltered": "(filtered1 from _MAX_ total entries)",
            "lengthMenu": "_MENU_ entries",
            "search": "Search:",
            "zeroRecords": "No matching records found"
        },

        // setup buttons extentension: http://datatables.net/extensions/buttons/
        buttons: [
            { extend: 'print', className: 'btn dark btn-outline' },
            { extend: 'pdf', className: 'btn green btn-outline' },
            { extend: 'csv', className: 'btn purple btn-outline ' }
        ],

        // setup responsive extension: http://datatables.net/extensions/responsive/
        //responsive: {
        //    details: {
        //    }
        //},
        "order": [],
        "columns": [
            {
                "orderable": false,
                "sClass": "text-center",
                "data": "Id",
                "render": function (data, type, row) {
                    //return '<input data-id="' + data + '" type="checkbox" value="">';
                    return '<input type="checkbox" id="check-all" class="flat studchk" data-id="' + data + '"/>';
                }
            },
            {
                "orderable": false,
                "sClass": "text-center",
                "data": "Id",
                "render": function (data, type, row) {
                    return '<a href="#" title="Delete Student" class="DeleteStudent" data-url="/admin/student/delete/' + data + '"> <i class="fa fa-trash hoverhand redimp"></i></a>';
                }
            },
            {
                "orderable": false,
                "sClass": "text-center",
                "data": "Isblock",
                "render": function (data, type, row) {
                    if (data) {
                        return 'Yes';
                    }
                    else {
                        return 'No';
                    }
                }
            },
            {
                "data": "Name",
                "render": function (data, type, row) {
                    return '<a href="/admin/student/' + row.Id + '">' + data + '</a>';
                }
            },
            { "data": "NativeLanguage" },
            { "data": "FromAddress" },
            {
                "data": "BirthDate",
                "render": function (data, type, row) {
                    if (data !== null) {
                        var date = new Date(data.match(/\d+/)[0] * 1);
                        var month = date.getMonth() + 1;
                        return (month.length > 1 ? month : "0" + month) + "/" + date.getDate() + "/" + date.getFullYear();
                    }
                    else
                        return "";
                }
            },
            { "data": "SchoolGrade" }
        ],
        "lengthMenu": [
            [5, 10, 15, 20, -1],
            [5, 10, 15, 20, "All"] // change per page values here
        ],
        // set the initial value
        "pageLength": 10,

        "dom": "Blfrtip", // horizobtal scrollable datatable

        // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
        // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js).
        // So when dropdowns used the scrollable div should be removed.
        //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
    });

    table.on('draw.dt', function () {
        //setTimeout(function () {
        $('input.flat').iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green'
        });
        //}, 500);
        //setTimeout(
        //    function () {
        //        $("[data-toggle=confirmation]").confirmation({ container: "body", btnOkClass: "btn btn-sm btn-success", btnCancelClass: "btn btn-sm btn-danger", onConfirm: function (event, element) { } });
        //    }, 500);
    });
}

$(function () {

    LoadStudentList();

    $(document.body).on("click", ".DeleteStudent", function () {
        AjaxFunction("Get", $(this).attr("data-url"), "application/json", "json", "", StudentOperation);
    });

    $(document.body).on('ifChecked', '.studchkhead', function (event) {
        var checked = $(this).parent('[class*="icheckbox"]').hasClass("checked");
        $('.studchk').iCheck('check');
    });

    $(document.body).on('ifUnchecked', '.studchkhead', function (event) {
        var checked = $(this).parent('[class*="icheckbox"]').hasClass("checked");
        $('.studchk').iCheck('uncheck');
    });

    $(document.body).on('click', '.blockuser', function (event) {
        blockusers = [];
        $('.studchk').each(function (i, obj) {
            if ($(this).parent('[class*="icheckbox"]').hasClass("checked")) {
                blockusers.push($(this).attr("data-id"));
            }
        });

        if (blockusers.length > 0) {
            AjaxFunction("POST", "/admin/students/BlockStudents", "application/x-www-form-urlencoded;charset=UTF-8", "json", JSON.stringify({ blockstudentslist: blockusers }), BlockStudentCallFunction);
        }
        else {
            new PNotify({
                title: 'Block Students',
                text: 'Select at least one student',
                type: 'error',
                styling: 'bootstrap3'
            });
        }
    });
})
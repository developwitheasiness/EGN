var table = $('#GradeList');

function GradeOperation(resp, type) {
    if (resp.IsError) {
        new PNotify({
            title: 'Grade Save',
            text: resp.Message,
            type: 'error',
            styling: 'bootstrap3'
        });
    }
    else {
        new PNotify({
            title: 'Grade Save',
            text: 'Grade saved successfully',
            type: 'success',
            styling: 'bootstrap3'
        });
        $(".grade-modal").modal("hide");
        LoadGradeList();
    }
}

function DeleteGradeCallFunction(resp) {
    if (resp.IsError) {
        new PNotify({
            title: 'Delete Grade',
            text: resp.Message,
            type: 'error',
            styling: 'bootstrap3'
        });
    }
    else {
        new PNotify({
            title: 'Delete Grade',
            text: 'Grade deleted successfully',
            type: 'success',
            styling: 'bootstrap3'
        });
        $(".grade-modal").modal("hide");
        LoadGradeList();
    }
}

function LoadGradeList() {

    if ($.fn.DataTable.isDataTable('#GradeList')) {
        $('#GradeList').DataTable().destroy();
    }

    table = table.dataTable({
        "ajax": {
            "type": "GET",
            "url": "/admin/master/GetSchoolGradeList",
            "dataSrc": function (resp) {
                if (resp.IsError) {
                    new PNotify({
                        title: 'Grade List',
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
            "emptyTable": "No grades available in table",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "infoEmpty": "No entries found",
            "infoFiltered": "(filtered1 from _MAX_ total entries)",
            "lengthMenu": "_MENU_ entries",
            "search": "Search:",
            "zeroRecords": "No matching grades found"
        },
        buttons: [
            //{ extend: 'print', className: 'btn dark btn-outline' },
            //{ extend: 'pdf', className: 'btn green btn-outline' },
            { extend: 'csv', className: 'btn purple btn-outline ' }
        ],
        "order": [],
        "columns": [
            {
                "orderable": false,
                "sClass": "text-center",
                "data": "GradeId",
                "render": function (data, type, row) {
                    return '<a href="#" title="Delete Grade" data-id="' + data + '" class="deleteGrade" data-url="/admin/grade/delete/' + data + '"> <i class="fa fa-trash hoverhand redimp"></i></a>';
                }
            },
            {
                "data": "Grade",
                "render": function (data, type, row) {
                    return '<a href="#" data-toggle="modal" data-target=".grade-modal" data-id="' + row.GradeId + '" title="Edit Grade" class=editGrade>' + data + '</a>';
                }
            }
        ],
        "lengthMenu": [
            [5, 10, 15, 20, -1],
            [5, 10, 15, 20, "All"] // change per page values here
        ],
        // set the initial value
        "pageLength": 10,

        "dom": "Blfrtip" // horizontal scrollable datatable

        // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
        // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js).
        // So when dropdowns used the scrollable div should be removed.
        //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
    });

    table.on('draw.dt', function () {
        //setTimeout(function () {
        //    $('checkbox input').iCheck({
        //        checkboxClass: 'icheckbox_flat-green'
        //    });
        //}, 500);
    });
}

$(function () {

    $('.frmGrade').parsley();

    LoadGradeList();

    $(document.body).on("click", ".DeleteGrade", function () {
        AjaxFunction("Delete", $(this).attr("data-url"), "application/json", "json", GradeOperation);
    });

    $(".btnSaveGrade").click(function (e) {
        if ($(".frmGrade").parsley().validate()) {
            AjaxFunction($(".frmGrade").attr("method"), $(".frmGrade").attr("action"), "application/x-www-form-urlencoded;charset=UTF-8", "json",$(".frmGrade").serialize(), GradeOperation);
        }
    });

    $(document.body).on("click", ".editGrade", function () {
        $('.frmGrade').parsley().reset();
        $("#GradeId").val($(this).attr("data-id"));
        $("#Grade").val($(this).html());
    });

    $(".newgrade").on("click", function () {
        $('.frmGrade').parsley().reset();
        resetform('.frmGrade');
        //$("#GradeId").val("");
        //$("#Grade").val("");
    });

    $(document.body).on("click", ".deleteGrade", function () {
        AjaxFunction("Get", $(this).attr("data-url"), "application/json", "json", "", DeleteGradeCallFunction);
    });
})
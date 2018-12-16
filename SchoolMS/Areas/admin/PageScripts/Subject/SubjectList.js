var table = $('#SubjectList');

function GradeOperation(resp, type) {
    if (resp.IsError) {
        new PNotify({
            title: 'Subject Save',
            text: resp.Message,
            type: 'error',
            styling: 'bootstrap3'
        });
    }
    else {
        new PNotify({
            title: 'Subject Save',
            text: 'Subject saved successfully',
            type: 'success',
            styling: 'bootstrap3'
        });
        $(".subject-modal").modal("hide");
        LoadSubjectList();
    }
}

function DeleteSubjectCallFunction(resp) {
    if (resp.IsError) {
        new PNotify({
            title: 'Delete Subject',
            text: resp.Message,
            type: 'error',
            styling: 'bootstrap3'
        });
    }
    else {
        new PNotify({
            title: 'Delete Subject',
            text: 'Subject deleted successfully',
            type: 'success',
            styling: 'bootstrap3'
        });
        $(".subject-modal").modal("hide");
        LoadSubjectList();
    }
}

function LoadSubjectList() {

    if ($.fn.DataTable.isDataTable('#SubjectList')) {
        $('#SubjectList').DataTable().destroy();
    }

    table = table.dataTable({
        "ajax": {
            "type": "GET",
            "url": "/admin/master/GetSchoolSubjectList",
            "dataSrc": function (resp) {
                if (resp.IsError) {
                    new PNotify({
                        title: 'Subject List',
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
            "emptyTable": "No subjects available in table",
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
                "data": "SchoolSubjectId",
                "render": function (data, type, row) {
                    return '<a href="#" title="Delete Grade" data-id="' + data + '" class="DeleteSubject" data-url="/admin/subject/delete/' + data + '"> <i class="fa fa-trash hoverhand redimp"></i></a>';
                }
            },
            {
                "data": "Subject",
                "render": function (data, type, row) {
                    return '<a href="#" data-toggle="modal" grade-id="' + row.GradeId + '" data-target=".subject-modal" data-id="' + row.SchoolSubjectId + '" title="Edit Subject" class=editSubject>' + data + '</a>';
                }
            },
            {
                "data": "GradeName",
                "render": function (data, type, row) {
                    return '<span>' + data + '</span>';
                }
            }
        ],
        "lengthMenu": [
            [5, 10, 15, 20, -1],
            [5, 10, 15, 20, "All"] // change per page values here
        ],
        // set the initial value
        "pageLength": 10,

        "dom": "Blfrtip"
    });
}

function GetGradeListForSchoolSubject(resp, type)
{
    if (resp.IsError) {
        new PNotify({
            title: 'Get Grade List',
            text: resp.Message,
            type: 'error',
            styling: 'bootstrap3'
        });
    }
    else {
        var htmlGradeBind;

        $(resp.Data).each(function (index, value) {
            htmlGradeBind += '<option value="' + value.Value + '">' + value.Text + '</option>';
        });
        $("#GradeId").append(htmlGradeBind);
    }
}

$(function () {

    $('.frmSubject').parsley();

    LoadSubjectList();

    $(document.body).on("click", ".DeleteSubject", function () {
        AjaxFunction("Delete", $(this).attr("data-url"), "application/json", "json", GradeOperation);
    });

    $(".btnSaveSubject").click(function (e) {
        if ($(".frmSubject").parsley().validate()) {
            AjaxFunction($(".frmSubject").attr("method"), $(".frmSubject").attr("action"), "application/x-www-form-urlencoded;charset=UTF-8", "json", $(".frmSubject").serialize(), GradeOperation);
        }
    });

    $(document.body).on("click", ".editSubject", function () {
        $('.frmSubject').parsley().reset();
        $("#SchoolSubjectId").val($(this).attr("data-id"));
        $("#GradeId").val($(this).attr("grade-id"));
        $("#Subject").val($(this).html());
    });

    $(".newsubject").on("click", function () {
        $('.frmSubject').parsley().reset();
        resetform('.frmSubject');
    });

    $(document.body).on("click", ".DeleteSubject", function () {
        AjaxFunction("Get", $(this).attr("data-url"), "application/json", "json", "", DeleteSubjectCallFunction);
    });

    AjaxFunction("Get", "/admin/master/GetGradeListForSchoolSubject", "application/x-www-form-urlencoded;charset=UTF-8", "json", "", GetGradeListForSchoolSubject);
})
var table = $('#GSList');

function GSOperation(resp, type) {
    if (resp.IsError) {
        new PNotify({
            title: 'General Setting Save',
            text: resp.Message,
            type: 'error',
            styling: 'bootstrap3'
        });
    }
    else {
        new PNotify({
            title: 'General Setting Save',
            text: 'General Setting saved successfully',
            type: 'success',
            styling: 'bootstrap3'
        });
        $(".gs-modal").modal("hide");
        LoadGSList();
    }
}

//function DeleteGSCallFunction(resp) {
//    if (resp.IsError) {
//        new PNotify({
//            title: 'Delete General Setting',
//            text: resp.Message,
//            type: 'error',
//            styling: 'bootstrap3'
//        });
//    }
//    else {
//        new PNotify({
//            title: 'Delete General Setting',
//            text: 'General Setting deleted successfully',
//            type: 'success',
//            styling: 'bootstrap3'
//        });
//        $(".gs-modal").modal("hide");
//        LoadGSList();
//    }
//}

function LoadGSList() {

    if ($.fn.DataTable.isDataTable('#GSList')) {
        $('#GSList').DataTable().destroy();
    }

    table = table.dataTable({
        "ajax": {
            "type": "GET",
            "url": "/admin/master/GetGSList",
            "dataSrc": function (resp) {
                if (resp.IsError) {
                    new PNotify({
                        title: 'General Settings List',
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
            //{ extend: 'csv', className: 'btn purple btn-outline ' }
        ],
        "order": [],
        "columns": [
            {
                "data": "FieldValue",
                "render": function (data, type, row) {
                    return '<a title="Edit General Setting" href="#" data-id="' + row.GeneralSettingId + '" class="editGS">' + row.FieldValue + '</span>';
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

    $('.frmGS').parsley();

    LoadGSList();

    //$(document.body).on("click", ".DeleteSlider", function () {
    //    AjaxFunction("Delete", $(this).attr("data-url"), "application/json", "json", SliderOperation);
    //});

    $(".btnSaveGS").click(function (e) {
        if ($(".frmGS").parsley().validate()) {
            AjaxFunction($(".frmGS").attr("method"), $(".frmGS").attr("action"), "application/x-www-form-urlencoded;charset=UTF-8", "json", $(".frmGS").serialize(), GSOperation);
        }
    });

    $(document.body).on("click", ".editGS", function () {
        $('.frmGS').parsley().reset();
        $("#GeneralSettingId").val($(this).attr("data-id"));
        $("#FieldValue").val($(this).html());
        $(".gs-modal").modal("show");
    });

    $(".newslider").on("click", function () {
        $('.frmGS').parsley().reset();
        resetform('.frmGS');
        $(".gs-modal").modal("show");
    });

    //$(document.body).on("click", ".deleteGS", function () {
    //    AjaxFunction("Get", $(this).attr("data-url"), "application/json", "json", "", DeleteGSCallFunction);
    //});
})
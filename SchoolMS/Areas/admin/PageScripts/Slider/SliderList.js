var table = $('#SliderList');
var fileData = new FormData();

function SliderOperation(resp, type) {
    if (resp.IsError) {
        new PNotify({
            title: 'Slider Save',
            text: resp.Message,
            type: 'error',
            styling: 'bootstrap3'
        });
    }
    else {
        new PNotify({
            title: 'Slider Save',
            text: 'Slider saved successfully',
            type: 'success',
            styling: 'bootstrap3'
        });
        $(".slider-modal").modal("hide");
        LoadSliderList();
    }
}

function DeleteSliderCallFunction(resp) {
    if (resp.IsError) {
        new PNotify({
            title: 'Delete Slider',
            text: resp.Message,
            type: 'error',
            styling: 'bootstrap3'
        });
    }
    else {
        new PNotify({
            title: 'Delete Slider',
            text: 'Slider deleted successfully',
            type: 'success',
            styling: 'bootstrap3'
        });
        $(".slider-modal").modal("hide");
        LoadSliderList();
    }
}

function LoadSliderList() {

    if ($.fn.DataTable.isDataTable('#SliderList')) {
        $('#SliderList').DataTable().destroy();
    }

    table = table.dataTable({
        "ajax": {
            "type": "GET",
            "url": "/admin/master/GetSliderList",
            "dataSrc": function (resp) {
                if (resp.IsError) {
                    new PNotify({
                        title: 'Slider List',
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
            "emptyTable": "No slides available in table",
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
                "orderable": false,
                "sClass": "text-center",
                "data": "SliderConfigId",
                "render": function (data, type, row) {
                    return '<a href="#" title="Delete Grade" data-id="' + row.SliderConfigId + '" class="deleteSlider" data-url="/admin/slider/delete/' + data + '"> <i class="fa fa-trash hoverhand redimp"></i></a>';
                }
            },
            {
                "data": "Description",
                "render": function (data, type, row) {
                    return '<a href="#" data-toggle="modal" data-path="' + row.SliderImage + '" data-target=".grade-modal" data-id="' + row.SliderConfigId + '" title="Edit Grade" class="editSlider">' + data + '</a>';
                }
            },
            {
                "data": "SliderImage",
                "render": function (data, type, row) {
                    return '<img src="/' + row.SliderImage + '" width="100px" height="100px"/>';
                }
            }
        ],
        "lengthMenu": [
            [5, 10, 15, 20, -1],
            [5, 10, 15, 20, "All"] // change per page values here
        ],
        // set the initial value
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

function check(a) {
    var ValidImageTypes = ["image/gif", "image/jpeg", "image/png"];
    if ($.inArray(a.files[0]["type"], ValidImageTypes) < 0) {
        return true;
    }
    return false;
}

function FileSelected(sender) {
    if (check(sender))//check is you function to check extension
    {
        new PNotify({
            title: 'Upload Slider Image',
            text: "Please select image file only",
            type: 'error',
            styling: 'bootstrap3'
        });
        $('#Files').val('');
    }
}

$(function () {

    $('.frmSlider').parsley();

    LoadSliderList();

    $(document.body).on("click", ".DeleteSlider", function () {
        AjaxFunction("Delete", $(this).attr("data-url"), "application/json", "json", SliderOperation);
    });

    $(".btnSaveSlide").click(function (e) {
        if ($(".frmSlider").parsley().validate()) {

            fileData = new FormData();
            var fileUpload;

            fileUpload = $("#Files").get(0);

            var files = fileUpload.files;

            if (files.length <= 0) {
                new PNotify({
                    title: 'Upload Slider Image',
                    text: "Please upload file",
                    type: 'error',
                    styling: 'bootstrap3'
                });
                return;
            }
            else {
                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i]);
                }
            }

            //AjaxFunction('POST', '/admin/master/SaveSlider?SliderConfigId=' + $("#SliderConfigId").val() + "&Description=" + $("#Description").val(), "application/x-www-form-urlencoded;charset=UTF-8", "json", fileData, SliderOperation);
            $.ajax({
                url: '/admin/master/SaveSlider?SliderConfigId=' + $("#SliderConfigId").val() + "&Description=" + $("#Description").val(),
                type: "POST",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: fileData,
                async: false,
                beforeSend: function () {
                    $('.LockOn').show();
                },
                complete: function () {
                    $('.LockOn').hide();
                },
                success: function (result) {
                    $('.LockOn').hide();
                    if (result.IsError) {
                        new PNotify({
                            title: 'Slider Save',
                            text: result.Message,
                            type: 'error',
                            styling: 'bootstrap3'
                        });
                    }
                    else {
                        new PNotify({
                            title: 'Slider Save',
                            text: 'Slider saved successfully',
                            type: 'success',
                            styling: 'bootstrap3'
                        });
                        $(".slider-modal").modal("hide");
                        LoadSliderList();
                    }
                },
                error: function (err) {
                    $('.LockOn').hide();
                    new PNotify({
                        title: 'Slider Save',
                        text: "There is some error",
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    //alert(err.statusText);
                }
            });
        }
    });

    $(document.body).on("click", ".editSlider", function () {
        $('.frmSlider').parsley().reset();
        $('#Files').val('');
        $("#SliderConfigId").val($(this).attr("data-id"));
        $("#Description").val($(this).html());
        $(".imgSliderImage").attr("src", $(this).attr("data-path"));
        $(".slider-modal").modal("show");
    });

    $(".newslider").on("click", function () {
        $('#Files').val('');
        $('.frmSlider').parsley().reset();
        resetform('.frmSlider');
    });

    $(document.body).on("click", ".deleteSlider", function () {
        AjaxFunction("Get", $(this).attr("data-url"), "application/json", "json", "", DeleteSliderCallFunction);
    });
})
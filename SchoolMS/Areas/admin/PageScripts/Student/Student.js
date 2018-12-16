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
            title: 'Student Save',
            text: 'Student saved successfully',
            type: 'success',
            styling: 'bootstrap3'
        });
        setTimeout(function () { window.location = "/admin/Students/List"; }, 2000);
    }
}

$(function () {

    $('#BirthDate').daterangepicker({
        singleDatePicker: true,
        singleClasses: "picker_1"
    });
    //, function (start, end, label) {
        //console.log(start.toISOString(), end.toISOString(), label);
    //});
    $('.frmStudent').parsley();

    $(".frmStudent").submit(function (e) {
        //e.preventDefault();
        if ($(".frmStudent").parsley().validate()) {
            //AjaxFunction($(".frmStudent").attr("method"), $(".frmStudent").attr("action"), "json", $(".frmStudent").serialize(), StudentOperation);
        }
    });
});
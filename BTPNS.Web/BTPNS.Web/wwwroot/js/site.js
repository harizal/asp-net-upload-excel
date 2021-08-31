// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function datatableSearchOnEnter() {
    $('div.dataTables_filter input').unbind();
    $('div.dataTables_filter input').bind('keyup', function (e) {
        if (e.keyCode === 13) {
            datatable.search(this.value).draw();
        }
    });
}

$("form").submit(function (e) {
    e.preventDefault();
    if ($(this).valid()) {
        swal({
            title: "Are you sure you want to save?",
            icon: "info",
            buttons: true
        })
            .then((willDelete) => {
                if (willDelete) {
                    var selector = this.querySelector('.btn.btn-primary.ladda-button');
                    if (selector !== null) {
                        var l = Ladda.create(selector);
                        l.start();
                    }
                    $('.btn.btn-danger').attr("disabled", "disabled");
                    $(this).off('submit').submit();
                }
            });

    }
});

startLoadingButton = function (selector) {
    var l = Ladda.create(selector);
    l.start();
};

stopLoadingButton = function () {
    Ladda.stopAll();
};

//$(".years").datetimepicker({
//    format: "YYYY",
//    icons: {
//        next: 'fa fa-chevron-right',
//        previous: 'fa fa-chevron-left',
//    },
//    minDate: '1900-01-01'
//});

function getlengthMenu() {
    return [12, 25, 50, 100];
}
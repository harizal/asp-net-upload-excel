var uploadField = document.getElementById("ExcelFileImport");

uploadField.onchange = function () {
    var fileName = document.getElementById("ExcelFileImport").value;
    var idxDot = fileName.lastIndexOf(".") + 1;
    var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
    if (extFile == "xls" || extFile == "xlsx") {
        return;
    } else {
        swal("Error", "Only MS Excel file allowed", "error");
        this.value = "";
    }
};

$("#btnSubmit").click(function () {    
    $("#MainForm").submit();
});
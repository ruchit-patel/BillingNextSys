window.onload = function () {
   
    document.getElementById('dateReceived').valueAsDate = new Date();
    $("form").submit(function (e) {
        if (($('#chqpaychk').is(":checked")) && $("#chqnum").val() == "") {
            alert("Cheque number is empty");
            e.preventDefault();
            return false;
        }
    });

};
function chkenable() {
    if ($('#chqpaychk').is(":checked")) {
        $('#chqnum').prop('disabled', false);
    }
    else {
        $('#chqnum').val('');
        $('#chqnum').prop('disabled', true);
    }
}

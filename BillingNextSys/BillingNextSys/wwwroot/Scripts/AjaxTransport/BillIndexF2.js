var dtt;
$(document).ready(function () {

    var now = new Date();
    var month = (now.getMonth() + 1);
    var day = now.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    dtt = now.getFullYear() + '-' + month + '-' + day;

    $('#my_modal').on('show.bs.modal', function (e) {
        var bId = $(e.relatedTarget).data('bid');
        var dgid = $(e.relatedTarget).data('dgid');

        $('input[name=redate]').val(dtt);
        $('input[name=dphone]').val($(e.relatedTarget).data('debtorpnum'));
        $('input[name=compname]').val($(e.relatedTarget).data('companyname'));
        $('input[name=dgname]').val($(e.relatedTarget).data('bto'));
        $('#billno').text("Bill No: " + $(e.relatedTarget).data('bid')).css("fontWeight", "bold");
        $('#billto').text("Billed To: " + $(e.relatedTarget).data('bto')).css("fontWeight", "bold");
        $('#billamount').text("Bill Amount: " + $(e.relatedTarget).data('bamount')).css("fontWeight", "bold");
        displaymoredetails(bId, dgid);
    });
});

function displaymoredetails(inid, dgid) {
    var newInput = "";
    var options = {};
    options.url = "/Bill/Format2/Index?id=" + inid + "&&handler=SelectAll";
    options.type = "GET";
    options.dataType = "json";

    options.success = function (data) {
        data.forEach(function (element) {

            if (element.billdetails.billAmountOutstanding == 0) {

                newInput += "<tr><td>" + element.debtorName + "</td><td>" + element.billdetails.particularsName + "</td><td>" + element.billdetails.amount + "</td><td id='amtout-" + element.billdetails.billDetailsID + "'>" + element.billdetails.billAmountOutstanding + "</td></tr>";
            }
            else {
                newInput += "<tr><td>" + element.debtorName + "</td><td>" + element.billdetails.particularsName + "</td><td>" + element.billdetails.amount + "</td><td id='amtout-" + element.billdetails.billDetailsID + "'>" + element.billdetails.billAmountOutstanding + "</td><td><input type='number' style='width:100px;' id='amt-" + element.billdetails.billDetailsID + "' /></td><td>Cheque Payment? &nbsp;<input type='checkbox' checked id='chkpay-" + element.billdetails.billDetailsID + "' onclick='chkenable(this.id)' ></td><td><input type='number' id='chknum-" + element.billdetails.billDetailsID + "' style='width:90px;'/></td><td><input type='date' id='dt-" + element.billdetails.billDetailsID + "' value='" + dtt + "' style='width:150px;' /></td><td><button onclick='savepaymt(" + element.billdetails.billDetailsID + ");' class='btn btn-default' ><img src='/images/save.png' alt='save' height='20px' width='20px' /></button><input type='hidden' id='dgid-" + element.billdetails.billDetailsID + "' value='" + dgid + "' ></td></tr>";
            }
        });
        document.getElementById('rowbody1').innerHTML = newInput;

    };
    options.error = function () {
        $("#msg1").html("Error while making Ajax call!");
    };
    $.ajax(options);
}

function chkenable(strid) {
    if (document.getElementById(strid).checked) {
        document.getElementById("chknum-" + strid.substr(strid.indexOf('-') + 1)).disabled = false;;
    }
    else {

        document.getElementById("chknum-" + strid.substr(strid.indexOf('-') + 1)).disabled = true;
    }
}

function savepaymt(stid) {
    if (document.getElementById('chkpay-' + stid).checked && document.getElementById("chknum-" + stid).value == "") {
        alert("Please provide cheque number.");
        return true;
    }
    if (parseFloat($("#amt-" + stid).val()) > parseFloat($("#amtout-" + stid).text())) {
        alert("The amout is more than outstanding amount.");
        $("#amt-" + stid).val('');
    }
    else {

        var options = {};
        options.url = "/Bill/Format1/Index?dgid=" + $("#dgid-" + stid).val() + "&&handler=InsertReceived";
        options.type = "POST";

        var obj = {};
        obj.ReceivedAmount = $("#amt-" + stid).val();
        obj.ReceivedDate = $("#dt-" + stid).val();

        if (document.getElementById('chkpay-' + stid).checked) {
            obj.ChequePaymet = true;
            obj.ChequeNumber = $("#chknum-" + stid).val();
        }
        else {
            obj.ChequePaymet = false;
        }
        obj.BillDetailsID = stid;



        options.data = JSON.stringify(obj);
        options.contentType = "application/json; charset=utf-8";
        options.dataType = "json";

        options.beforeSend = function (xhr) {
            xhr.setRequestHeader("MY-XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        };
        options.success = function (msg) {
            var bnum = $('#billno').text();
            displaymoredetails(bnum.substr(bnum.indexOf(':') + 2), $("#dgid-" + stid).val());
        };
        options.error = function () {
            alert("Error Occured While Executing Transaction.");
        };
        $.ajax(options);
    }
}
function sendsms() {

    var options = {};
    options.url = "/Bill/Format2/Index?debname=" + $('input[name=dgname]').val() + "&&companyname=" + $('input[name=compname]').val() + "&&debphone=" + $('input[name=dphone]').val() + "&&handler=SendSms";
    options.type = "POST";

    var obj = {};
    obj.ReceivedAmount = $('input[name=recamt]').val();
    obj.ReceivedDate = $('input[name=redate]').val();

    if (document.getElementById('chqpayy').checked) {
        obj.ChequePaymet = true;
    }
    else {
        obj.ChequePaymet = false;
    }
    options.data = JSON.stringify(obj);
    options.contentType = "application/json; charset=utf-8";
    options.dataType = "json";

    options.beforeSend = function (xhr) {
        xhr.setRequestHeader("MY-XSRF-TOKEN",
            $('input:hidden[name="__RequestVerificationToken"]').val());
    };
    options.success = function (msg) {

        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }

        toastr.success(msg, "Message Sent");

    };
    options.error = function () {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }

        toastr.error("Error Occured While Sending SMS", "Error Occured While Sending SMS");

    };
    $.ajax(options);
    return false;
}

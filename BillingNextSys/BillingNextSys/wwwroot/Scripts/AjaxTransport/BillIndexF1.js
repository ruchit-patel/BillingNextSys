var dtt;
var dgid;


$(document).ready(function () {

    var now = new Date();
    var month = (now.getMonth() + 1);
    var day = now.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    dtt = now.getFullYear() + '-' + month + '-' + day;

    $('#redateadv').val(dtt);

     function alignModal(){
         var modalDialog = $(this).find(".loader");
        
         // Applying the top margin on modal dialog to align it vertically center
         modalDialog.css("margin-top", Math.max(0, ($(window).height() - modalDialog.height()) / 2));
     }
     // Align modal when it is displayed
     $(".modal").on("shown.bs.modal", alignModal);
    
     // Align modal when user resize the window
     $(window).on("resize", function(){
         $(".modal:visible").each(alignModal);
     });   

    $('#my_modal').on('show.bs.modal', function (e) {

        var bId = $(e.relatedTarget).data('bid');
         dgid = $(e.relatedTarget).data('dgid');
        $('input[name=redate]').val(dtt);
        $('input[name=dphone]').val($(e.relatedTarget).data('debtorpnum'));
        $('input[name=compname]').val($(e.relatedTarget).data('companyname'));
        $('input[name=dgname]').val($(e.relatedTarget).data('bto'));

        $('#billno').text("Bill No: " + $(e.relatedTarget).data('bid')).css("fontWeight", "bold");
        $('#billto').text("Billed To: " + $(e.relatedTarget).data('bto')).css("fontWeight", "bold");
        $('#billamount').text("Bill Amount: " + $(e.relatedTarget).data('bamount')).css("fontWeight", "bold");
        displaymoredetails(bId, dgid);
    });

    $(document).on('show.bs.modal', '.modal', function (event) {
        var zIndex = 1040 + (10 * $('.modal:visible').length);
        $(this).css('z-index', zIndex);
        setTimeout(function () {
            $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
        }, 0);
    });


    $('#my_modal').on('hidden.bs.modal', function () {
        $('#dist_amt').val("");
        $('#chqnumadv').val('');
        $('#chqpayyadv').prop('checked', true);
        document.getElementById('rowAdvanceBody').innerHTML='';
        $('#rowAdvance').show();
    });

});

function loadFromAdvance()
{
    var options = {};
    options.url = "/Bill/Format1/Index?handler=AllAdvanceSettle";
    options.type = "POST";

    var obj = {};
    obj.billid = $("#loadAllFromAdvance").attr("data-billid");
    obj.amt= $("#loadAllFromAdvance").attr("data-amt");

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

function chkenableadv()
{
    if($('#chqpayyadv').is(":checked"))
    {
        $('#chqnumadv').prop('disabled', false);
    }
    else
    {
        $('#chqnumadv').val('');
        $('#chqnumadv').prop('disabled', true);
    }
}

function distribute(amt){

  $("#rowbody1 :input").each(function(e){	
    if(this.id.includes('amt-'))
    {
        if(amt>=parseFloat($("#amtout-"+this.id.substr(4)).html()))
        {
            this.value=$("#amtout-"+this.id.substr(4)).html();
            amt=amt-($("#amtout-"+this.id.substr(4)).html()); 
        }
        else{
            this.value=amt;
            amt=amt-this.value;
        }
    }
  });

  if(amt>0)
  {
    $("#confirmationModal").modal();
    $("#amtid").html(amt);
  }
}

function addRowAdv(amt)
{
    if (document.getElementById('chqpayyadv').checked && document.getElementById("chqnumadv").value == "") {
        alert("Please provide cheque number.");
        return true;
    }
    var chequepayment="Cash";
    var chequenumber=null;
    if($('#chqpayyadv').is(":checked"))
    {
        chequepayment="Cheque";
        chequenumber=$('#chqnumadv').val();
    }
    var newInput = "<tr><td>Advance Payment</td><td>" +amt + "</td><td>0</td><td>"+amt+"</td><td>"+chequepayment+"</td><td>"+chequenumber+"</td><td>"+$('#redateadv').val()+"</td><td><button id='btnAddAdvance' onclick='addadvance(" + amt + ");' class='btn btn-default' ><img src='/images/save.png' alt='save' height='20px' width='20px' /></button></td></tr>";
    document.getElementById('rowAdvanceBody').innerHTML = newInput;
}

function addadvance(amt)
{
  
    var options = {};
        options.url = "/Bill/Format1/Index?handler=AmtAdvance";
        options.type = "POST";

        var obj = {};
        obj.AdvanceAmount=amt;
        obj.ChequePaymet=false;
        if($('#chqpayyadv').is(":checked"))
        {
            obj.ChequePaymet=true;
            obj.ChequeNumber=$('#chqnumadv').val();
        }
        
        obj.ReceivedDate=$('#redateadv').val();
        obj.DebtorGroupID=dgid;

        options.data = JSON.stringify(obj);
        options.contentType = "application/json; charset=utf-8";
        options.dataType = "json";

        options.beforeSend = function (xhr) {
            xhr.setRequestHeader("MY-XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        };
        options.success = function (msg) {
           if(msg=='1')
            {
                $('#btnAddAdvance').hide();
                $('#rowAdvance').hide();
            }

        };
        options.error = function () {
            alert("Error Occured While Executing Transaction.");
        };
        $.ajax(options);
}

function displaymoredetails(inid, dgid) {
    $(document).ajaxStart(function () {
        $("#wait").modal({ show: true, backdrop: 'static' });
        $("#wait").show();
    });


    $(document).ajaxComplete(function () {
        $("#wait").modal('hide');
        $("#wait").hide();
    });

    $("#loadAllFromAdvance").css("visibility", "hidden");
    var newInput = "";
    var options = {};
    options.url = "/Bill/Format1/Index?id=" + inid + "&&handler=SelectAll";
    options.type = "GET";
    options.dataType = "json";
    var sum = 0;
    options.success = function (data) {
        data.forEach(function (element) {

            if (element.billAmountOutstanding == 0) {
                newInput += "<tr><td>" + element.particularsName + "</td><td>" + element.amount + "</td><td id='amtout-" + element.billDetailsID + "'>" + element.billAmountOutstanding + "</td></tr>";
            }
            else {
                sum += element.billAmountOutstanding;
                newInput += "<tr><td>" + element.particularsName + "</td><td>" + element.amount + "</td><td id='amtout-" + element.billDetailsID + "'>" + element.billAmountOutstanding + "</td><td><input type='number'  id='amt-" + element.billDetailsID + "' /></td><td>Cheque Payment? &nbsp;<input type='checkbox' checked id='chkpay-" + element.billDetailsID + "' onclick='chkenable(this.id)' ></td><td><input type='number' id='chknum-" + element.billDetailsID + "' style='width:100px;'/></td><td><input type='date' id='dt-" + element.billDetailsID + "' value='" + dtt + "' style='width:150px;' /></td><td><button onclick='savepaymt(" + element.billDetailsID + ");' class='btn btn-default' ><img src='/images/save.png' alt='save' height='20px' width='20px' /></button><td><button onclick='advancepaymt(" + element.billDetailsID + "," + element.advancePayAmount + ");' class='btn btn-default'> <i class='fa fa-bolt' aria-hidden='true'></i> ₹ " + element.advancePayAmount + "  </button><input type='hidden' id='dgid-" + element.billDetailsID + "' value='" + dgid + "' ></td></tr>";
            }
        });
        
        if (sum > 0 && data[0].advancePayAmount > 0) {
            console.log("here");
            $("#loadAllFromAdvance").css("visibility", "visible");
            $("#loadAllFromAdvance").attr("data-amt", data[0].advancePayAmount);
            $("#loadAllFromAdvance").attr("data-billid",inid);
        }

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

function advancepaymt(billdetid, advanceamt)
{
    var options = {};
    options.url = "/Bill/Format1/Index?billdetid=" + billdetid + "&&advancepayamt=" + advanceamt + "&&handler=AdvancePaySettle";
    options.type = "GET";
    options.dataType = "json";

    options.success = function (data){
        var bnum = $('#billno').text();
        displaymoredetails(bnum.substr(bnum.indexOf(':') + 2), $("#dgid-" + billdetid).val());
    };
    options.error = function () {
        alert("Error executing transaction!");
    };
    $.ajax(options);

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

﻿var counter = 0;
var errcnt = 0;
var diffcount = 0;
window.onload = function () {
    //filladress();
    yearfill();
    document.getElementById("del--1").style.display = "none";
    document.getElementById("deloperation").style.display = "none";
    key('ctrl+1', function () { document.getElementById("addoperations").click(); return false });
    key('ctrl+2', function () { document.getElementById("deloperation").click(); return false });
    counter = +($('#lastcount').val()) - 1;
    fillDebtorInfo(-1);
    resizeel();
    calculateGrandTotal();
};

function resizeel() {
    for (i = -1; i <= counter; i++) {
        var particulars = document.getElementById('particulars-' + i);
        if (particulars.value.length > 0) {
            particulars.style.width = ((particulars.value.length + 1) * 8) + 'px';
        } else {
            particulars.style.width = ((particulars.getAttribute('placeholder').length + 1) * 8) + 'px';
        }
    }
}


function fillDebtorInfo(num) {
    var dataList = document.getElementById('debname-' + num);

    var dinfoa = $("#billtoid").val();
    var opt = {};
    opt.url = "/Bill/Format2/Create?id=" + dinfoa + "&&handler=BillToDebtorInfo";
    opt.type = "GET";

    opt.dataType = "json";

    opt.success = function (data) {
        $("#debname-" + num).empty();

        data.forEach(function (element) {
            var option = document.createElement('option');

            option.value = element.debtorName;
            option.id = element.debtorID;

            dataList.appendChild(option);
        });
    };
    opt.error = function () {

        var option = document.createElement('option');

        option.value = "Couldn't Fetch Debtor Information";

        dataList.appendChild(option);
    };
    $.ajax(opt);


}

function editcont(btnid, count) {
    diffcount = $('#amount-' + count).text().replace(/[^0-9.]/g, '');
    $('#amountinput-' + count).val($('#amountinput-' + count).val().replace(/[^0-9.]/g, ''));

    setSelectedIndex(document.getElementById("yearsel-" + count), $('#year-' + count).text());
    document.getElementById(btnid).style.display = "none";
    document.getElementById("save-" + count).style.display = "inline-block";
    document.getElementById("dt-" + count).style.display = "none";
    document.getElementById("dname-" + count).style.display = "inline-block";
    document.getElementById("pp-" + count).style.display = "none";
    document.getElementById("particulars-" + count).style.display = "inline-block";
    document.getElementById("year-" + count).style.display = "none";
    document.getElementById("yearsel-" + count).style.display = "inline-block";
    document.getElementById("period-" + count).style.display = "none";
    document.getElementById("periodinput-" + count).style.display = "inline-block";
    document.getElementById("amount-" + count).style.display = "none";
    document.getElementById("amountinput-" + count).style.display = "inline-block";
    fillDebtorInfo(count);

}




function delcont(btnid, count) {
    var options = {};

    var billamt = $('#amount-' + btnid.substring(4)).text().replace(/[^0-9.]/g, '').trim();


    options.url = "/Bill/Format1/Edit?id=" + count + "&&billamt=" + billamt + "&&debid=" + $('#billtoid').val() + "&&handler=DeleteBillDetails";
    options.type = "DELETE";
    options.dataType = "html";
    options.beforeSend = function (xhr) {
        xhr.setRequestHeader("MY-XSRF-TOKEN",
            $('input:hidden[name="__RequestVerificationToken"]').val());
    };
    options.success = function (msg) {

        if (msg == '"Deleted Successfully! Remove Row."') {

            document.getElementById("row-" + btnid.substring(4)).style.display = "none";
            document.getElementById("row-" + btnid.substring(4)).innerHTML = "<p>dummy row</p>";
            calculateGrandTotal();


            var billlid = $('#invoicenum').text();
            var options1 = {};
            options1.url = "/Bill/Format1/Edit?id=" + billlid + "&&handler=UpdateBill";
            options1.type = "PUT";

            var obj1 = {};
            obj1.BillNumber = billlid;
            obj1.BilledTo = $('#to').text();
            obj1.BillAmount = $('#grandtotal').text().substring(1, $('#grandtotal').text().length);
            obj1.InvoiceDate = $('#invcdt').val();
            obj1.DebtorGroupID = $('#billtoid').val();
            obj1.BillDate = $('#billdate').val();
            obj1.BillDelivered = $('#billdel').val();
            obj1.SecretUnlockCode = $('#secode').val();
            obj1.MessageSent = $('#messsent').val();
            obj1.BranchID = $('#branchid').val();
            obj1.CompanyID = $('#companyId').val();


            options1.data = JSON.stringify(obj1);

            options1.contentType = "application/json";
            options1.dataType = "html";

            options1.beforeSend = function (xhr) {
                xhr.setRequestHeader("MY-XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            };
            options1.success = function (msg) {
            };
            options1.error = function () {
                alert("Error Occured while updating.");
            };
            $.ajax(options1);


        }
        else {
            alert("Exception occured while handling the return data.");
        }
    };
    options.error = function () {
        alert("Error Occured while deleting.");
    };
    $.ajax(options);
}






function savecont(btnid, billid) {
    var ttamt = parseFloat($("#amountinput-" + btnid.substring(5)).val()) - diffcount;
    var options = {};
    options.url = "/Bill/Format1/Edit?id=" + billid + "&&damt=" + ttamt + "&&handler=UpdateBillDetails";
    options.type = "PUT";


    var obj = {};
    obj.BillDetailsID = billid;

    obj.ParticularsName = $("#particulars-" + btnid.substring(5)).val();
    obj.TaxableValue = $("#amountinput-" + btnid.substring(5)).val();
    obj.Period = $("#periodinput-" + btnid.substring(5)).val();
    obj.YearInfo = $("#yearsel-" + btnid.substring(5)).val();
    obj.Amount = $("#amountinput-" + btnid.substring(5)).val();
    obj.CompanyID = $('#companyId').val();

    var pinfoa = $("#parti-" + btnid.substring(5)).find('option[value="' + obj.ParticularsName + '"]').attr('id');
    if (pinfoa == null || pinfoa === "undefined") {
        alert("Particulars not set properly");
        errcnt = 1;
        return false;
    }
    $('#pid-' + btnid.substring(5)).val(pinfoa);


    obj.ParticularsID = pinfoa;
    obj.DebtorGroupID = $('#billtoid').val();
    obj.BillNumber = $('#invoicenum').text();

    var dnamm = $("#dname-" + btnid.substring(5)).val();
    var dinfoa = $("#debname-" + btnid.substring(5)).find('option[value="' + dnamm + '"]').attr('id');
    if (dinfoa == null || dinfoa === "undefined") {
        alert("Sub-Debtor not set properly");
        errcnt = 1;
        return false;
    }
    $('#did-' + btnid.substring(5)).val(dinfoa);


    obj.DebtorID = dinfoa;
    if (obj.DebtorID == null || obj.DebtorID == "") {
        alert("Sub-Debtor is Not Set Correctly");
        errcnt = 1;
        return false;
    }
    if (obj.ParticularsID == null || obj.ParticularsID == "") {
        alert("Particulars id Not Set Correctly");
        errcnt = 1;
        return false;
    }


    options.data = JSON.stringify(obj);
    options.contentType = "application/json";
    options.dataType = "html";

    options.beforeSend = function (xhr) {
        xhr.setRequestHeader("MY-XSRF-TOKEN",
            $('input:hidden[name="__RequestVerificationToken"]').val());
    };
    options.success = function (msg) {



        var billlid = $('#invoicenum').text();
        var options1 = {};
        options1.url = "/Bill/Format2/Edit?id=" + billlid + "&&handler=UpdateBill2";
        options1.type = "PUT";

        var obj1 = {};
        obj1.BillNumber = billlid;
        obj1.BilledTo = $('#to').text();
        obj1.BillAmount = $('#grandtotal').text().substring(1, $('#grandtotal').text().length);
        obj1.InvoiceDate = $('#invcdt').val();
        obj1.DebtorGroupID = $('#billtoid').val();
        obj1.BillDate = $('#billdate').val();
        obj1.BillDelivered = $('#billdel').val();
        obj1.SecretUnlockCode = $('#secode').val();
        obj1.MessageSent = $('#messsent').val();
        obj1.BranchID = $('#branchid').val();
        obj1.CompanyID = $('#companyId').val();

        options1.data = JSON.stringify(obj1);

        options1.contentType = "application/json";
        options1.dataType = "html";

        options1.beforeSend = function (xhr) {
            xhr.setRequestHeader("MY-XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        };
        options1.success = function (msg) {
        };
        options1.error = function () {
            alert("Error Occured while updating bill amount.");
        };
        $.ajax(options1);





        var countid = btnid.substring(5);

        document.getElementById("pp-" + countid).innerHTML = obj.ParticularsName;
        document.getElementById("dt-" + countid).innerHTML = $('#dname-' + btnid.substring(5)).val();
        document.getElementById("amountinput-" + countid).innerHTML = obj.TaxableValue;
        //$('#year-'+countid).text(''+);
        $('#period-' + countid).text('' + obj.Period);
        $('#amount-' + countid).text('' + obj.TaxableValue);
        $('#year-' + countid).text($("#yearsel-" + countid + " option:selected").text());

        document.getElementById(btnid).style.display = "none";
        document.getElementById("et-" + billid).style.display = "inline-block";
        document.getElementById("dt-" + btnid.substring(5)).style.display = "inline-block";
        document.getElementById("dname-" + btnid.substring(5)).style.display = "none";
        document.getElementById("pp-" + btnid.substring(5)).style.display = "inline-block";
        document.getElementById("particulars-" + btnid.substring(5)).style.display = "none";
        document.getElementById("period-" + btnid.substring(5)).style.display = "inline-block";
        document.getElementById("periodinput-" + btnid.substring(5)).style.display = "none";
        document.getElementById("year-" + btnid.substring(5)).style.display = "inline-block";
        document.getElementById("yearsel-" + btnid.substring(5)).style.display = "none";
        document.getElementById("amount-" + btnid.substring(5)).style.display = "inline-block";
        document.getElementById("amountinput-" + btnid.substring(5)).style.display = "none";
        errcnt = 0;

    };
    options.error = function () {
        alert("Error Occured while updating.");
    };
    $.ajax(options);

}


function setSelectedIndex(s, v) {

    for (var i = 0; i < s.options.length; i++) {

        if (s.options[i].text == v) {

            s.options[i].selected = true;

            return;

        }

    }

}





function fillparticulars(pname, idval) {

    var dataList = document.getElementById("parti-" + idval);

    var options = {};
    options.url = "/Bill/Format1/Create?str=" + pname + "&&handler=Particulars";
    options.type = "GET";
    options.dataType = "json";

    options.success = function (data) {

        $("#parti-" + idval).empty();
        data.forEach(function (element) {

            var option = document.createElement('option');
            option.value = element.particularsName;
            option.id = element.particularsID;
            dataList.appendChild(option);


        });

    };
    options.error = function () {
        var option = document.createElement('option');

        option.value = "Couldn't Fetch Particulars Information";

        dataList.appendChild(option);
    };
    $.ajax(options);

}

function fillvalues(dname) {
    var dataList = document.getElementById('tosearch');

    var options = {};
    options.url = "/Bill/Format1/Create?str=" + dname + "&&handler=BillTo";
    options.type = "GET";
    options.dataType = "json";
    options.success = function (data) {

        $("#tosearch").empty();
        data.forEach(function (element) {

            var option = document.createElement('option');

            option.value = element.debtorGroupName;
            option.id = element.debtorGroupID;

            dataList.appendChild(option);

        });

    };
    options.error = function () {
        var option = document.createElement('option');

        option.value = "Couldn't Fetch Debtor Information";

        dataList.appendChild(option);
    };
    $.ajax(options);
}




function fillpdetails(idval) {
    var x = document.getElementById("parti-" + idval);
    var i;
    for (i = 0; i < x.options.length; i++) {
        if (x.options[i].value != $('#particulars-' + idval).val()) {
            $('#particulars' + idval).val('');
        }
    }
    var particulars = document.getElementById('particulars-' + idval);
    if (particulars.value.length > 0) {
        particulars.style.width = ((particulars.value.length + 1) * 8) + 'px';
    } else {
        particulars.style.width = ((particulars.getAttribute('placeholder').length + 1) * 8) + 'px';
    }
    var pinfoa = $("#parti-" + idval + " option[value='" + $('#particulars-' + idval).val() + "']").attr('id');
    var options = {};
    options.url = "/Bill/Format1/Create?id=" + pinfoa + "&&handler=ParticularsDetails";
    options.type = "GET";
    options.dataType = "json";
    options.success = function (data) {
        $("#amountinput-" + idval).val(data.amount);
        calcamount(data.amount, idval);
    };
    options.error = function () {
        $("#saccode-" + idval).val("-1");
        $("#taxableval-" + idval).val("-1");
    };
    $.ajax(options);
}

function calcamount(pamount, idval) {
    var taxableval = document.getElementById('amountinput-' + idval);
    if (taxableval.value.length > 0) {
        taxableval.style.width = ((taxableval.value.length + 3) * 8) + 'px';
    } else {
        taxableval.style.width = ((taxableval.getAttribute('placeholder').length + 3) * 8) + 'px';
    }
    var total = +pamount;
    document.getElementById('amount-' + idval).innerHTML = total.toFixed(0);

    calculateGrandTotal();
}

function addInput() {
    counter++;


    var rowid = 'row-' + counter;
    var newRow = $("<tr id='row-" + counter + "'>");
    var cols = "";
    cols += '<td><input id="dname-' + counter + '" style="width:250px;" placeholder="Debtor Name.." list="debname-' + counter + '" autocomplete="off"><datalist id="debname-' + counter + '"></datalist> </td>';
    cols += '<td><input id="particulars-' + counter + '" style="width:250px;" placeholder="Particulars.." list="parti-' + counter + '" autocomplete="off"  onpaste="this.oninput();" oninput="fillparticulars($(this).val(),' + counter + ');"  onfocusout="fillpdetails(' + counter + ')"><datalist id="parti-' + counter + '"></datalist></td>';
    cols += '<td class="text-center"><select id="yearsel-' + counter + '"></select></td>';
    cols += '<td class="text-center"><input style="width:65px; " type="text" placeholder="Period.." id="periodinput-' + counter + '" value=' + $('#yearser').val() + '></td>';
    cols += '<td class="text-center count-me">  <p id="amount-' + counter + '" style="display: none;"></p>  <input style="width:65px;" type="number" placeholder="Amount.." id="amountinput-' + counter + '" onchange="calcamount($(this).val(),' + counter + ');" onpaste="this.onchange();" oninput="this.onchange();" onkeyup="this.onchange();" /></td>';
    var ooptions = $("#yearsel--1 > option").clone();


    document.getElementById("deloperation").style.display = "inline";
    newRow.append(cols);
    $('#billdet > tbody > tr').eq(counter).after(newRow);
    $('#yearsel-' + counter).append(ooptions);
    fillDebtorInfo(counter);
    // $("table.order-list").append(newRow);


}
function removeRow() {
    var comval = $('#lastcount').val();

    if (counter == comval) {
        document.getElementById("deloperation").style.display = "none";
        $('#row-' + counter).remove();
        counter -= 1;
        calculateGrandTotal();
    }
    else {
        $('#row-' + counter).remove();
        counter -= 1;
        calculateGrandTotal();
    }
}

function calculateGrandTotal() {
    var grandTotal = 0;

    $('tr').each(function () {
        // var sum = 0;
        $(this).find('.count-me').each(function () {
            var combat = $(this).text().replace(/[^0-9.]/g, '');
            if (!isNaN(combat) && combat.length !== 0) {

                grandTotal += parseFloat(combat);
            }
        });

    });

    document.getElementById('grandtotal').innerHTML = '&#8377;' + grandTotal.toFixed(0);
    convertNumberToWords(grandTotal.toFixed(0));
}

function convertNumberToWords(amount) {
    var words = new Array();
    words[0] = '';
    words[1] = 'One';
    words[2] = 'Two';
    words[3] = 'Three';
    words[4] = 'Four';
    words[5] = 'Five';
    words[6] = 'Six';
    words[7] = 'Seven';
    words[8] = 'Eight';
    words[9] = 'Nine';
    words[10] = 'Ten';
    words[11] = 'Eleven';
    words[12] = 'Twelve';
    words[13] = 'Thirteen';
    words[14] = 'Fourteen';
    words[15] = 'Fifteen';
    words[16] = 'Sixteen';
    words[17] = 'Seventeen';
    words[18] = 'Eighteen';
    words[19] = 'Nineteen';
    words[20] = 'Twenty';
    words[30] = 'Thirty';
    words[40] = 'Forty';
    words[50] = 'Fifty';
    words[60] = 'Sixty';
    words[70] = 'Seventy';
    words[80] = 'Eighty';
    words[90] = 'Ninety';
    amount = amount.toString();
    var atemp = amount.split(".");
    var number = atemp[0].split(",").join("");
    var n_length = number.length;
    var words_string = "";
    if (n_length <= 9) {
        var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
        var received_n_array = new Array();
        for (var i = 0; i < n_length; i++) {
            received_n_array[i] = number.substr(i, 1);
        }
        for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
            n_array[i] = received_n_array[j];
        }
        for (var i = 0, j = 1; i < 9; i++ , j++) {
            if (i == 0 || i == 2 || i == 4 || i == 7) {
                if (n_array[i] == 1) {
                    n_array[j] = 10 + parseInt(n_array[j]);
                    n_array[i] = 0;
                }
            }
        }
        value = "";
        for (var i = 0; i < 9; i++) {
            if (i == 0 || i == 2 || i == 4 || i == 7) {
                value = n_array[i] * 10;
            } else {
                value = n_array[i];
            }
            if (value != 0) {
                words_string += words[value] + " ";
            }
            if ((i == 1 && value != 0) || (i == 0 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Crores ";
            }
            if ((i == 3 && value != 0) || (i == 2 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Lakhs ";
            }
            if ((i == 5 && value != 0) || (i == 4 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Thousand ";
            }
            if (i == 6 && value != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                words_string += "Hundred and ";
            } else if (i == 6 && value != 0) {
                words_string += "Hundred ";
            }
        }
        words_string = words_string.split("  ").join(" ");
    }
    //return ;
    document.getElementById('inwords').innerHTML = words_string + '.';
}


function editbill() {
    saveopen();
    if (errcnt == 0) {

        var countdebout = 0;
        var billlid = $('#invoicenum').text();
        var options = {};
        options.url = "/Bill/Format2/Edit?id=" + billlid + "&&handler=UpdateBill2";
        options.type = "PUT";

        var obj = {};
        obj.BillNumber = billlid;
        obj.BilledTo = $('#to').text();
        obj.BillAmount = $('#grandtotal').text().substring(1, $('#grandtotal').text().length);
        obj.InvoiceDate = $('#invcdt').val();
        obj.DebtorGroupID = $('#billtoid').val();
        obj.BillDate = $('#billdate').val();
        obj.BillDelivered = $('#billdel').val();
        obj.SecretUnlockCode = $('#secode').val();
        obj.MessageSent = $('#messsent').val();
        obj.BranchID = $('#branchid').val();
        obj.CompanyID = $('#companyId').val();

        options.data = JSON.stringify(obj);

        options.contentType = "application/json";
        options.dataType = "html";

        options.beforeSend = function (xhr) {
            xhr.setRequestHeader("MY-XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        };
        options.success = function (msg) {

            $('#billdet tr').each(function (row, tr) {

                if (typeof ($(tr).find('td:eq(0)').find("input").val()) !== "undefined") {
                    var comval = $('#lastcount').val();
                    var rowid = $(tr).attr('id').substring(4);
                    if (rowid >= comval) {

                        var options = {};
                        options.url = "/Bill/Format2/Create?handler=InsertBillDetails";
                        options.type = "POST";


                        var obj = {};
                        var detoutstandingamt = parseFloat($(tr).find('td:eq(4)').find("input").val());

                        if (isNullOrWhitespace($(tr).find('td:eq(0)').find("input").val())) {
                            return;
                        }

                        obj.ParticularsName = $(tr).find('td:eq(1)').find("input").val();
                        obj.TaxableValue = $(tr).find('td:eq(4)').find("input").val();
                        obj.Period = $(tr).find('td:eq(3)').find("input").val();
                        obj.YearInfo = $(tr).find('td:eq(2)').find(":selected").val();
                        obj.Amount = $(tr).find('td:eq(4)').find("input").val();
                        obj.CompanyID = $('#companyId').val();

                        var inputid = $(tr).find('td:eq(1)').find("input").val();
                        obj.ParticularsID = $(tr).find('td:eq(1)').find("datalist").find('option[value="' + inputid + '"]').attr('id');




                        obj.DebtorGroupID = $('#billtoid').val();
                        obj.BillNumber = $('#invoicenum').text();

                        var dinputid = $(tr).find('td:eq(0)').find("input").val();
                        obj.DebtorID = $(tr).find('td:eq(0)').find("datalist").find('option[value="' + dinputid + '"]').attr('id');
                        if (obj.DebtorID == null || obj.DebtorID == "undefined") {
                            return;
                        }
                        if (!isNaN(detoutstandingamt) && obj.ParticularsID != null && typeof obj.ParticularsID !== "undefined" && !isNaN(obj.ParticularsID)) {
                            countdebout += detoutstandingamt;
                           

                   }



                        options.data = JSON.stringify(obj);

                        options.contentType = "application/json; charset=utf-8";
                        options.dataType = "json";

                        options.beforeSend = function (xhr) {
                            xhr.setRequestHeader("MY-XSRF-TOKEN",
                                $('input:hidden[name="__RequestVerificationToken"]').val());
                        };
                        options.success = function (msg) {


                        };
                        options.error = function () {

                            alert("error");
                        };
                        $.ajax(options);
                    }
                }

            });
            savedebout(countdebout);

        };
        options.error = function () {
            alert("Error Occured while updating.");
        };
        $.ajax(options);
    }
    else if (errcnt == 1) {

        return;

    }
}

function savedebout(debamt) {

    //here to update the final bill amt
    var grndtotal = calcprevtotal();
    var ttamt = grndtotal + debamt;
    var billamtstr = $('#invoicenum').text();
    var options1 = {};
    options1.url = "/Bill/Format2/Edit?bnum=" + billamtstr + "&&billamt=" + ttamt + "&&handler=UpdateBillAmt";
    options1.type = "GET";
    options1.dataType = "json";
    options1.success = function (data) {

        var options = {};
        options.url = "/Bill/Format1/Edit?debout=" + debamt + "&&dgid=" + $('#billtoid').val() + "&&handler=UpdateDebOut";
        options.type = "GET";
        options.dataType = "json";

        options.success = function (data) {
            window.location.pathname = "/Bill/Format2/Index";
        };
        options.error = function () {
            alert("Error Updating Debtor Oustanding Amount. If the problem persist, close the current window and manually update the debtor outstanding amount from ledger.");
            savedebout(debamt);
        };
        $.ajax(options);

    };
    options1.error = function () {
        alert("Error updating correct bill amount");
    };
    $.ajax(options1);
}

function yearfill() {
    var dt = new Date();
    var yr = (dt.getFullYear()) + "";
    var mnth = dt.getMonth();
    var yrprv = dt.getFullYear() - 1 + "";
    var yrnxt = dt.getFullYear() + 1 + "";
    var yearstr = "";

    if (mnth == 0 || mnth == 1 || mnth == 2) {
        yearstr = yrprv + "-" + yr.substring(2);
    }
    else {
        yearstr = yr + "-" + yrnxt.substring(2);
    }
    $('#yearser').val(yearstr);
}

function saveopen() {
    var lastcnt = ($('#lastcount').val()) - 1;
    $('#billdet tr').each(function (row, tr) {
        if (typeof ($(tr).find('td:eq(0)').find("input").val()) !== "undefined") {
            var rowid = $(tr).attr('id').substring(4);
            if (rowid <= lastcnt) {
                var rowele = document.getElementById("row-" + rowid);
                if (window.getComputedStyle(rowele).display !== "none") {
                    var savebtid = document.getElementById("save-" + rowid);
                    if (window.getComputedStyle(savebtid).display !== "none") {
                        savebtid.click();
                        return false;
                    }
                }
            }
        }

    });

}


function calcprevtotal() {

    var grandTotal = 0;
    var lastcnt = ($('#lastcount').val()) - 1;
    $('#billdet tr').each(function (row, tr) {
        if (typeof ($(tr).find('td:eq(0)').find("input").val()) !== "undefined") {
            var rowid = $(tr).attr('id').substring(4);
            if (rowid <= lastcnt) {
                var rowele = document.getElementById("row-" + rowid);
                if (window.getComputedStyle(rowele).display !== "none") {
                    $(this).find('.count-me').each(function () {
                        var combat = $(this).text().replace(/[^0-9.]/g, '');
                        if (!isNaN(combat) && combat.length !== 0) {
                            grandTotal += parseFloat(combat);

                        }
                    });
                }
            }
        }

    });
    return grandTotal;
}

function isNullOrWhitespace(input) {
    return !input || !input.trim();
}
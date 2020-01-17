window.onload = function () {
    yearfill();
    var lastbnum = $("#billnum").val();

    $("#bnum").val(+lastbnum + 1);

    document.getElementById("deloperation").style.display = "none";
    key('ctrl+1', function () { document.getElementById("addoperations").click(); return false });
    key('ctrl+2', function () { document.getElementById("deloperation").click(); return false });
};
var counter = -1;

function fillparticulars(pname, idval) {

    var dataList = document.getElementById("parti" + idval);

    var options = {};
    options.url = "/Bill/Format1/Create?str=" + pname + "&&handler=Particulars";
    options.type = "GET";
    options.dataType = "json";

    options.success = function (data) {

        $("#parti" + idval).empty();
        data.forEach(function (element) {

            var option = document.createElement('option');
            option.value = element.particularsName;
            option.id = element.particularsID;
            dataList.appendChild(option);


        });

    };
    options.error = function () {
        var option = document.createElement('option');
        // Set the value using the item in the JSON array.
            option.value = "Couldn't Fetch Particulars Information";
        // Add the <option> element to the <datalist>.
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
            // Set the value using the item in the JSON array.
                option.value = element.debtorGroupName;
            option.id = element.debtorGroupID;
            // Add the <option> element to the <datalist>.
                dataList.appendChild(option);

        });

    };
    options.error = function () {
        var option = document.createElement('option');
        // Set the value using the item in the JSON array.
            option.value = "Couldn't Fetch Debtor Information";
        // Add the <option> element to the <datalist>.
            dataList.appendChild(option);
    };
    $.ajax(options);
}




function filladress() {

    var x = document.getElementById("tosearch");
    var i;
    for (i = 0; i < x.options.length; i++) {
        if (x.options[i].value != $('#toinfo').val()) {
            return false;
        }
    }

    var dinfoa = $("#tosearch option[value='" + $('#toinfo').val() + "']").attr('id');
    var options = {};
    options.url = "/Bill/Format1/Create?id=" + dinfoa + "&&handler=BillToDetails";
    options.type = "GET";
    options.dataType = "json";
    options.success = function (data) {
        document.getElementById('toaddress').innerHTML = data.debtorGroupAddress;
        fillDebtorInfo(-1);

    };
    options.error = function () {
        $("#toaddress").val("Couldn't load data");
        $("#togstin").val("Couldn't load data");
    };
    $.ajax(options);

}


function fillDebtorInfo(idval) {
    var dataList = document.getElementById('debname' + idval);
    var dinfoa = $("#tosearch option[value='" + $('#toinfo').val() + "']").attr('id');
    var opt = {};
    opt.url = "/Bill/Format2/Create?id=" + dinfoa + "&&handler=BillToDebtorInfo";
    opt.type = "GET";

    opt.dataType = "json";

    opt.success = function (data) {

        $("#debname" + idval).empty();
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


function fillpdetails(idval) {
    var x = document.getElementById("parti" + idval);
   
    var particulars = document.getElementById('particulars' + idval);
    if (particulars.value.length > 0) {
        particulars.style.width = ((particulars.value.length + 1) * 8) + 'px';
    } else {
        particulars.style.width = ((particulars.getAttribute('placeholder').length + 1) * 8) + 'px';
    }
    var pinfoa = $("#parti" + idval + " option[value='" + $('#particulars' + idval).val() + "']").attr('id');
    var options = {};
    options.url = "/Bill/Format1/Create?id=" + pinfoa + "&&handler=ParticularsDetails";
    options.type = "GET";
    options.dataType = "json";
    options.success = function (data) {
        $("#pamount" + idval).val(data.amount);
        calcamount(data.amount, idval);
    };
    options.error = function () {
        $("#toaddress" + idval).val("-1");
        $("#taxableval" + idval).val("-1");
    };
    $.ajax(options);
}

function calcamount(pamount, idval) {
    var pamount = document.getElementById('pamount' + idval);
    if (pamount.value.length > 0) {
        pamount.style.width = ((pamount.value.length + 6) * 8) + 'px';
    } else {
        pamount.style.width = ((pamount.getAttribute('placeholder').length + 3) * 8) + 'px';
    }
    calculateGrandTotal();
}

function addInput() {
    counter++;
    var rowid = 'row' + counter;
    var newRow = $("<tr id='row" + counter + "'>");
    var cols = "";
    cols += '<td><input id="dname' + counter + '" style="width:250px;" class="form-control" placeholder="Debtor Name.." list="debname' + counter + '" autocomplete="off" ><datalist id="debname' + counter + '"></datalist></td>';
    cols += '<td ><input id="particulars' + counter + '" class="form-control" style="width:250px;" placeholder="Particulars.." list="parti' + counter + '" autocomplete="off"  onpaste="this.oninput();" oninput="fillparticulars($(this).val(),' + counter + ');"  onfocusout="fillpdetails(' + counter + ')"><datalist id="parti' + counter + '"></datalist></td>';
    cols += '<td class="text-center"><select id="year' + counter + '" class="form-control" > </select></td>';
    cols += '<td class="text-center"> <input id="period' + counter + '" class="form-control" style="width:120px;" placeholder="E.g. 2018-19 " list="periodlist' + counter + '" value=' + $('#yearser').val() + '><datalist id="periodlist' + counter + '"></datalist></td>';
    cols += '<td class="text-right count-me" ><input class="form-control pull-right" style="text-align:right; width:120px; " type="number" id="pamount' + counter + '" onchange="calcamount($(this).val(),' + counter + ');" onpaste="this.onchange();" oninput="this.onchange();" onkeyup="this.onchange();"/></td>';

    document.getElementById("deloperation").style.display = "inline";
    newRow.append(cols);
    $('#billdet > tbody > tr').eq(counter).after(newRow);
    var $optionss = $("#year-1 > option").clone();
    $('#year' + counter).append($optionss);
    fillDebtorInfo(counter);

}
function removeRow() {
    if (counter == 0) {
        document.getElementById("deloperation").style.display = "none";
        $('#row' + counter).remove();
        counter -= 1;
        calculateGrandTotal();
    }
    else if (counter == -1) {
        document.getElementById("deloperation").style.display = "none";
        calculateGrandTotal();
    }
    else {
        $('#row' + counter).remove();
        counter -= 1;
        calculateGrandTotal();
    }
}

function calculateGrandTotal() {
    var grandTotal = 0;

    $('tr').each(function () {
        // var sum = 0;
        $(this).find('.count-me').each(function () {
            var combat = $(this).find("input").val();
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


function savebill() {
    var countdebout = 0;
    var debgid;
    var options = {};
    options.url = "/Bill/Format1/Create?handler=InsertBill";
    options.type = "POST";


    var obj = {};
    obj.BillNumber = $('#bnum').val();
    obj.BilledTo = $("#tosearch option[value='" + $('#toinfo').val() + "']").val();
    obj.BillAmount = $('#grandtotal').text().substring(1, $('#grandtotal').text().length);
    obj.InvoiceDate = $('#invcdt').val();
    obj.DebtorGroupID = $("#tosearch option[value='" + $('#toinfo').val() + "']").attr('id');
    debgid = obj.DebtorGroupID;

    if (obj.DebtorGroupID == null || obj.DebtorGroupID == "") {
        alert("Debtor not selected correctly");
        return false;
    }


    var inputid1 = $('#dname-1').val();
    if ($('#debname-1').find('option[value="' + inputid1 + '"]').attr('id') == null) {
        alert("Sub-Debtor is not set properly");
        return false;
    }
    var inputid2 = $('#particulars-1').val();
    if ($('#parti-1').find('option[value="' + inputid2 + '"]').attr('id') == null) {
        alert("Particulars is not set properly");
        return false;
    }

    options.data = JSON.stringify(obj);

    options.contentType = "application/json; charset=utf-8";
    options.dataType = "json";

    options.beforeSend = function (xhr) {
        xhr.setRequestHeader("MY-XSRF-TOKEN",
            $('input:hidden[name="__RequestVerificationToken"]').val());
    };
    options.success = function (msg) {
        if (msg == "Added Successfully!") {
            $('#billdet tr').each(function (row, tr) {
                if (typeof ($(tr).find('td:eq(0)').find("input").val()) !== "undefined") {

                    var options = {};
                    options.url = "/Bill/Format1/Create?handler=InsertBillDetails";
                    options.type = "POST";

                    var detoutstandingamt = parseFloat($(tr).find('td:eq(4)').find("input").val());


                    if (isNullOrWhitespace($(tr).find('td:eq(0)').find("input").val())) {
                        return;
                    }

                    var obj = {};
                    obj.ParticularsName = $(tr).find('td:eq(1)').find("input").val();
                    obj.YearInfo = $(tr).find('td:eq(2)').find("select").val();
                    obj.Period = $(tr).find('td:eq(3)').find("input").val();
                    obj.Amount = $(tr).find('td:eq(4)').find("input").val();
                    obj.TaxableValue = $(tr).find('td:eq(4)').find("input").val();
                    obj.CompanyID = $('#companyid').val();

                    var inputid = $(tr).find('td:eq(1)').find("input").val();

                    obj.ParticularsID = $(tr).find('td:eq(1)').find("datalist").find('option[value="' + inputid + '"]').attr('id');




                    obj.BillNumber = $('#bnum').val();
                    obj.DebtorGroupID = $("#tosearch option[value='" + $('#toinfo').val() + "']").attr('id');

                    var debid = $(tr).find('td:eq(0)').find("input").val();
                    obj.DebtorID = $(tr).find('td:eq(0)').find("datalist").find('option[value="' + debid + '"]').attr('id');

                    if (obj.DebtorID == null || obj.DebtorID == "undefined") {
                        alert("Sub Debtor not set properly! Skipping the entry in bill");
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

            });
        }
        else if (msg == "Update 1") {
            var curr = $('#bnum').val();
            var curr = (+curr + 1);
            $('#bnum').val(curr);
            savebill();
        }

        savedebout(countdebout, debgid);

    };
    options.error = function () {

        alert("error in bill");
    };
    $.ajax(options);
}

function savedebout(debamt, debgid) {

    var billamtstr = $('#bnum').val();
    var options1 = {};
    options1.url = "/Bill/Format2/Edit?bnum=" + billamtstr + "&&billamt=" + debamt + "&&handler=UpdateBillAmt";
    options1.type = "GET";
    options1.dataType = "json";
    options1.success = function (data) {

        var options = {};
        options.url = "/Bill/Format1/Edit?debout=" + debamt + "&&dgid=" + debgid + "&&handler=UpdateDebOut";
        options.type = "GET";
        options.dataType = "json";

        options.success = function (data) {
            //redirect
            window.location.pathname = "/Bill/Format2/Index";

        };
        options.error = function () {
            alert("Error Updating Debtor Oustanding Amount. If the problem persist, close the current window and manually update the debtor outstanding amount from ledger.");
            savedebout(debamt, debgid);
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
    $('#period-1').val(yearstr);
}


function isNullOrWhitespace(input) {
    return !input || !input.trim();
}
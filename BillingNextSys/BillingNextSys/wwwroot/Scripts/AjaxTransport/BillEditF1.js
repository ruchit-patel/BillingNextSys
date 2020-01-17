var diffcount = 0;
var counter = 0;
var errcnt = 0;
window.onload = function () {
    document.getElementById("del--1").style.display = "none";
    filladress();
    document.getElementById("deloperation").style.display = "none";
    key('ctrl+1', function () { document.getElementById("addoperations").click(); return false });
    key('ctrl+2', function () { document.getElementById("deloperation").click(); return false });
    counter = +($('#lastcount').val()) - 1;
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

        var taxableval = document.getElementById('taxableval-' + i);
        if (taxableval.value.length > 0) {
            taxableval.style.width = ((taxableval.value.length + 3) * 8) + 'px';
        } else {
            taxableval.style.width = ((taxableval.getAttribute('placeholder').length + 3) * 8) + 'px';
        }
    }
}

function editcont(btnid, count) {
    diffcount = $('#pamount-' + count).text().replace(/[^0-9.]/g, '');
    document.getElementById(btnid).style.display = "none";
    document.getElementById("save-" + count).style.display = "inline-block";
    document.getElementById("pp-" + count).style.display = "none";
    document.getElementById("particulars-" + count).style.display = "inline-block";
    document.getElementById("psac-" + count).style.display = "none";
    document.getElementById("saccode-" + count).style.display = "inline-block";
    document.getElementById("ptamount-" + count).style.display = "none";
    document.getElementById("taxableval-" + count).style.display = "inline-block";
    $('#taxableval-' + count).val($('#taxableval-' + count).val().replace(/[^0-9.]/g, ''));
    $('#cgst-' + count).val($('#cgst-' + count).val().replace(/[^0-9.]/g, ''));
    $('#sgst-' + count).val($('#sgst-' + count).val().replace(/[^0-9.]/g, ''));
    $('#pamount-' + count).val($('#pamount-' + count).val().replace(/[^0-9.]/g, ''));
}


function delcont(btnid, count) {
    var options = {};

    var billamt = $('#pamount-' + btnid.substring(4)).text().replace(/[^0-9.]/g, '').trim();

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
            obj1.BilledTo = document.getElementById('toinfo').value;
            obj1.BillAmount = $('#grandtotal').text().substring(1, $('#grandtotal').text().length);
            obj1.InvoiceDate = $('#invcdt').val();
            obj1.PlaceOfSupply = $('#placeofsup').find(":selected").val();
            obj1.DebtorGroupID = $('#billtoid').val();
            obj1.SeriesName = $('#seriesinfo').val();
            obj1.BillDate = $('#billdate').val();
            obj1.BillActNum = $('#billactnum').val();
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
    var ttamt = parseFloat($("#pamount-" + btnid.substring(5)).text()) - diffcount;

    var options = {};
    options.url = "/Bill/Format1/Edit?id=" + billid + "&&damt=" + ttamt + "&&handler=UpdateBillDetails";
    options.type = "PUT";


    var obj = {};
    obj.BillDetailsID = billid;
    obj.ParticularsName = $("#particulars-" + btnid.substring(5)).val();
    obj.TaxableValue = $("#taxableval-" + btnid.substring(5)).val();
    obj.CGSTAmount = $("#cgst-" + btnid.substring(5)).text();
    obj.SGSTAmount = $("#sgst-" + btnid.substring(5)).text();
    obj.Amount = $("#pamount-" + btnid.substring(5)).text();
    obj.CompanyID = $('#companyId').val();


    var pinfoa = $("#parti-" + btnid.substring(5)).find('option[value="' + obj.ParticularsName + '"]').attr('id');
    if (pinfoa == null || pinfoa === "undefined") {
        alert("Particulars not set properly");
        errcnt = 1;
        return false;
    }
    $('#pid-' + btnid.substring(5)).val(pinfoa);


    obj.ParticularsID = pinfoa;
    obj.DebtorGroupID = $('#billtoid').val();;
    obj.BillNumber = $('#invoicenum').text();


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
        options1.url = "/Bill/Format1/Edit?id=" + billlid + "&&handler=UpdateBill";
        options1.type = "PUT";

        var obj1 = {};
        obj1.BillNumber = billlid;
        obj1.BilledTo = document.getElementById('toinfo').value;
        obj1.BillAmount = $('#grandtotal').text().substring(1, $('#grandtotal').text().length);
        obj1.InvoiceDate = $('#invcdt').val();
        obj1.PlaceOfSupply = $('#placeofsup').find(":selected").val();
        obj1.DebtorGroupID = $('#billtoid').val();
        obj1.SeriesName = $('#seriesinfo').val();
        obj1.BillDate = $('#billdate').val();
        obj1.BillActNum = $('#billactnum').val();
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




        var countid = btnid.substring(5);
      
        document.getElementById("pp-" + countid).innerHTML=obj.ParticularsName;
        document.getElementById("ptamount-" + countid).innerHTML = obj.TaxableValue;
        $('cgst-' + countid).text('' + obj.CGSTAmount);
        $('sgst-' + countid).text('' + obj.SGSTAmount);
        $('pamount-' + countid).text('' + obj.Amount);
        document.getElementById(btnid).style.display = "none";
        document.getElementById("et-" + billid).style.display = "inline-block";
        document.getElementById("pp-" + btnid.substring(5)).style.display = "inline-block";
        document.getElementById("particulars-" + btnid.substring(5)).style.display = "none";
        document.getElementById("psac-" + btnid.substring(5)).style.display = "inline-block";
        document.getElementById("saccode-" + btnid.substring(5)).style.display = "none";
        document.getElementById("ptamount-" + btnid.substring(5)).style.display = "inline-block";
        document.getElementById("taxableval-" + btnid.substring(5)).style.display = "none";

        errcnt = 0;
    };
    options.error = function () {
        alert("Error Occured while updating.");
    };
    $.ajax(options);

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
      
            option.value = "Couldn't Fetch Debtor Information";
   
            dataList.appendChild(option);
    };
    $.ajax(options);
}


function filladress() {

    var x = document.getElementById("tosearch");
    var i;
    for (i = 0; i < x.options.length; i++) {
        if (x.options[i].value != $('#toinfo').val()) {
            $('#toinfo').val('');
        }
    }

    var dinfoa = $("#tosearch option[value='" + $('#toinfo').val() + "']").attr('id');

    if (dinfoa == null || dinfoa === "undefined") {
        dinfoa = $('#billtoid').val();
    }
    $('#billtoid').val(dinfoa);
    var options = {};
    options.url = "/Bill/Format1/Create?id=" + dinfoa + "&&handler=BillToDetails";
    options.type = "GET";
    options.dataType = "json";
    options.success = function (data) {
        document.getElementById('toaddress').innerHTML = data.debtorGroupAddress;
        document.getElementById('togstin').innerHTML = data.debtorGSTIN;
    };
    options.error = function () {
        $("#toaddress").val("Couldn't load data");
        $("#togstin").val("Couldn't load data");
    };
    $.ajax(options);

}

function fillpdetails(idval) {
    var x = document.getElementById("parti-" + idval);
    var i;

    for (i = 0; i < x.options.length; i++) {
        if (x.options[i].value != $('#particulars' + idval).val()) {
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
        $("#saccode-" + idval).val(data.sacCode);
        $("#taxableval-" + idval).val(data.amount);
        calcamount(data.amount, idval);
    };
    options.error = function () {
        $("#saccode-" + idval).val("-1");
        $("#taxableval-" + idval).val("-1");
    };
    $.ajax(options);
}

function calcamount(pamount, idval) {
    var taxableval = document.getElementById('taxableval-' + idval);
    if (taxableval.value.length > 0) {
        taxableval.style.width = ((taxableval.value.length + 3) * 8) + 'px';
    } else {
        taxableval.style.width = ((taxableval.getAttribute('placeholder').length + 3) * 8) + 'px';
    }

    var cgst = (+(pamount) * 9) / 100;
    var sgst = cgst;
    var total = +pamount + cgst + sgst;
    document.getElementById('cgst-' + idval).innerHTML = cgst.toFixed(2);
    document.getElementById('sgst-' + idval).innerHTML = sgst.toFixed(2);
    document.getElementById('pamount-' + idval).innerHTML = total.toFixed(0);
    calculateGrandTotal();
}

function addInput() {
    counter++;
    var rowid = 'row-' + counter;
    var newRow = $("<tr id='row-" + counter + "'>");
    var cols = "";
    cols += '<td><input id="particulars-' + counter + '" style="width:250px;" placeholder="Particulars.." list="parti-' + counter + '" autocomplete="off"  onpaste="this.oninput();" oninput="fillparticulars($(this).val(),' + counter + ');"  onfocusout="fillpdetails(' + counter + ')"><datalist id="parti-' + counter + '"></datalist></td>';
    cols += '<td class="text-center"><input style="width:65px;" type="number" placeholder="SAC Code.." id="saccode-' + counter + '" readonly /></td>';
    cols += '<td class="text-center"><input style="width:65px;" type="number" placeholder="Amount.." min="0" id="taxableval-' + counter + '" onchange="calcamount($(this).val(),' + counter + ');" onpaste="this.onchange();" oninput="this.onchange();" onkeyup="this.onchange();" /></td>';
    cols += '<td class="text-center" id="cgst-' + counter + '"></td>';
    cols += '<td class="text-center" id="sgst-' + counter + '"></td>';
    cols += '<td class="text-right count-me" id="pamount-' + counter + '"></td>';

    document.getElementById("deloperation").style.display = "inline";
    newRow.append(cols);
    $('#billdet > tbody > tr').eq(counter).after(newRow);
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
        options.url = "/Bill/Format1/Edit?id=" + billlid + "&&handler=UpdateBill";
        options.type = "PUT";

        var obj = {};
        obj.BillNumber = billlid;
        obj.BilledTo = document.getElementById('toinfo').value;
        obj.BillAmount = $('#grandtotal').text().substring(1, $('#grandtotal').text().length);
        obj.InvoiceDate = $('#invcdt').val();
        obj.PlaceOfSupply = $('#placeofsup').find(":selected").val();
        obj.DebtorGroupID = $('#billtoid').val();
        obj.SeriesName = $('#seriesinfo').val();
        obj.BillDate = $('#billdate').val();
        obj.BillActNum = $('#billactnum').val();
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
                        options.url = "/Bill/Format1/Create?handler=InsertBillDetails";
                        options.type = "POST";

                        var detoutstandingamt = parseFloat($(tr).find('td:eq(5)').text());

                        if (isNullOrWhitespace($(tr).find('td:eq(0)').find("input").val())) {
                            return;
                        }
                       
                                var obj = {};
                        obj.ParticularsName = $(tr).find('td:eq(0)').find("input").val();
                        obj.Amount = $(tr).find('td:eq(5)').text();
                        obj.CGSTAmount = $(tr).find('td:eq(3)').text();
                        obj.SGSTAmount = $(tr).find('td:eq(4)').text();
                        obj.TaxableValue = $(tr).find('td:eq(2)').find("input").val();
                        obj.CompanyID = $('#companyId').val();
                        var inputid = $(tr).find('td:eq(0)').find("input").val();

                        obj.ParticularsID = $(tr).find('td:eq(0)').find("datalist").find('option[value="' + inputid + '"]').attr('id');

                        if (!isNaN(detoutstandingamt) && obj.ParticularsID != null && typeof obj.ParticularsID !== "undefined" && !isNaN(obj.ParticularsID)) {
                            countdebout += detoutstandingamt;
                                }

                        obj.BillNumber = $('#invoicenum').text();
                        obj.DebtorGroupID = $('#billtoid').val();

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
            window.location.pathname = "/Bill/Format1/Index";

        };
        options.error = function () {
            alert("Error Updating Debtor Oustanding Amount.If the problem persist, close the current window and manually update the debtor outstanding amount from ledger.");
            savedebout(debamt)
        };
        $.ajax(options);
    };
    options1.error = function () {
        alert("Error updating correct bill amount");
    };
    $.ajax(options1);
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


function isNullOrWhitespace(input) {
    return !input || !input.trim();
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


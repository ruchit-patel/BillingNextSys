window.onload = function () {
    document.getElementById('dateReceived').valueAsDate = new Date();
    $("form").submit(function () {
        var idz = $("#debtorGroupNames option[value='" + $('#debtorGroupId').val() + "']").attr('id');
        console.log(idz);
        if (idz == undefined || idz == null || idz == 0) {
            alert("Debtor Group not set properly.");
        }
        else {
            addDebtorGroupID();
        }
    });
};

function filldebtor(dname) {

    var dataList = document.getElementById('debtorGroupNames');

    var options = {};
    options.url = "/AdvancePay/Create?str=" + dname + "&&handler=DebtorNames";
    options.type = "GET";
    options.dataType = "json";
    options.success = function (data) {

        $("#debtorGroupNames").empty();
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

function addDebtorGroupID() {
    var x = document.getElementById("debtorGroupId").value;

    var options = {};
    options.url = "/AdvancePay/Create?str=" + x + "&&handler=DebtorNames";
    options.type = "GET";
    options.dataType = "json";
    options.success = function (data) {
 
        data.forEach(function (element) {

            if (x == element.debtorGroupName) {
               
                $("#hiddenDebtorGroupID").val('');
                $("#hiddenDebtorGroupID").val($("#debtorGroupNames option[value='" + $('#debtorGroupId').val() + "']").attr('id'));
            }
            else {
                alert("Debtor Group not set properly.");
                return false;
            }
        });
    };
    options.error = function () {
        return false;
    };
    $.ajax(options);
}


   
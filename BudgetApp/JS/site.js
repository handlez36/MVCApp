
/// <reference path="../Scripts/jquery-1.5.1-vsdoc.js" />
/// <reference path="../Scripts/jquery-ui-1.8.11.js" />
/// <reference path="../Scripts/jquery.jeditable.js" />


var creditcardentry = {};

$(document).ready(function () {
    creditcardentry.expandform();

    $('#entry-date').datepicker();

    $('.creditlistcell').editable("CreditCard/UpdateField",
    {
        submitdata: {
            EntryId: function () {
                return $("#EntryId").html();
            }
        },
        callback: function (value, settings) {
            // Calculate remaining balance after price change
            var edit_amount = parseFloat($('#entry-amount').html());
            var amount_paid = parseFloat($('#amount-paid').html());
            var remaining_balance = eval(edit_amount - amount_paid);
            $('#amount-remaining').html("" + remaining_balance);

            // Update Payments List
            var request = $.get("CreditCard/ListPayments", function (data) {
                alert(data);
            });

        }
    });
});

creditcardentry.expandform = function () {
    $("#addentry").click(function (e) {
        $("#editform").fadeIn('slow');

        e.PreventDefault();
        return false;
    })
};